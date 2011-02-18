using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeEntityExtensionMethods {
        public static GrapeEntity GetActualParent(this GrapeEntity entity) {
            if (entity.Parent != null) {
                if (entity.Parent is GrapeBlock) {
                    return ((GrapeBlock)entity.Parent).Parent;
                }

                return entity.Parent;
            }

            return null;
        }

        public static string GetPotentialEntityName(this GrapeEntity entity) {
            if (entity is GrapeFunction) {
                return ((GrapeFunction)entity).Name;
            } else if (entity is GrapeVariable) {
                return ((GrapeVariable)entity).Name;
            } else if (entity is GrapeClass) {
                return ((GrapeClass)entity).Name;
            } else if (entity is GrapePackageDeclaration) {
                return ((GrapePackageDeclaration)entity).PackageName;
            }

            return "";
        }

        public static GrapeClass GetBaseClass(this GrapeClass c, GrapeAst ast) {
            if (c.Inherits == null) {
                return null;
            }

            string inheritsQualifiedId = c.Inherits is GrapeIdentifierExpression ? ((GrapeIdentifierExpression)c.Inherits).Identifier : ((GrapeMemberExpression)c.Inherits).GetMemberExpressionQualifiedId();
            return GrapeAstUtilities.Instance.GetClassWithNameFromImportedPackagesInFile(ast, inheritsQualifiedId, c.FileName);
        }

        private static List<GrapeFunction> GetFunctionsWithNameInInheritanceTree(this GrapeClass c,  GrapeAst ast, string name) {
            List<GrapeFunction> functions = new List<GrapeFunction>();
            foreach (GrapeEntity entity in c.Block.Children) {
                if (entity is GrapeFunction && ((GrapeFunction)entity).Name == name) {
                    functions.Add((GrapeFunction)entity);
                }
            }

            GrapeClass baseClass = c.GetBaseClass(ast);
            if (baseClass != null) {
                functions.AddRange(baseClass.GetFunctionsWithNameInInheritanceTree(ast, name));
            }

            return functions;
        }

        public static GrapeFunction GetOverridingFunction(this GrapeFunction function, GrapeCodeGeneratorConfiguration config, out string errorMessage) {
            errorMessage = "";
            GrapeClass c = function.GetLogicalParentOfEntityType<GrapeClass>();
            if (c != null && c.Inherits != null) {
                GrapeClass baseClass = c.GetBaseClass(config.Ast);
                if (baseClass != null) {
                    List<GrapeFunction> functions = baseClass.GetFunctionsWithNameInInheritanceTree(config.Ast, function.Name);
                    if (functions.Count == 0) {
                        errorMessage = "Cannot find function with name '" + function.Name + "' in base classes.";
                        return null;
                    }

                    string[] modifiers = c.GetAppropriateModifiersForEntityAccess(config, functions[0]);
                    List<GrapeVariable> parameters = function.Parameters;
                    List<GrapeFunction> functionsWithSignature = GrapeTypeCheckingUtilities.Instance.GetFunctionWithSignature(config, functions, null, function.Name, function.ReturnType, parameters, ref errorMessage);
                    List<GrapeFunction> functionsToRemove = new List<GrapeFunction>();
                    foreach (GrapeFunction f in functionsWithSignature) {
                        if (f.Modifiers.Contains("sealed")) {
                            functionsToRemove.Add(f);
                        }
                    }

                    functionsToRemove.ForEach(f => functionsWithSignature.Remove(f));
                    if (functionsWithSignature.Count == 0) {
                        errorMessage = "'" + GrapeTypeCheckingUtilities.Instance.GetFunctionSignatureString(config, function.Name, function.ReturnType, parameters) + "': no suitable function found to override.";
                        return null;
                    }

                    if (errorMessage != "") {
                        return null;
                    }

                    return functionsWithSignature[0];
                } else {
                    string inheritsQualifiedId = c.Inherits is GrapeIdentifierExpression ? ((GrapeIdentifierExpression)c.Inherits).Identifier : ((GrapeMemberExpression)c.Inherits).GetMemberExpressionQualifiedId();
                    errorMessage = "Cannot finding base class '" + inheritsQualifiedId + "'.";
                }
            } else {
                errorMessage = "Cannot find class parent of function '" + function.Name + "'.";
            }

            return null;
        }

        public static bool IsClassInInheritanceTree(this GrapeClass c, GrapeCodeGeneratorConfiguration config, GrapeClass testingClass) {
            return GrapeTypeCheckingUtilities.Instance.IsTypeInClassInheritanceTree(config, testingClass.Name, c);
        }

        public static string GetPotentialModifiersOfEntity(this GrapeEntity entity) {
            if (entity is GrapeField) {
                return ((GrapeField)entity).Modifiers;
            } else if (entity is GrapeFunction) {
                return ((GrapeFunction)entity).Modifiers;
            } else if (entity is GrapeClass) {
                return ((GrapeClass)entity).Modifiers;
            }

            return null;
        }

        public static GrapePackageDeclaration GetPackage(this GrapeEntity entity) {
            GrapeEntity currentParent = entity;
            while (currentParent != null) {
                if (currentParent is GrapePackageDeclaration) {
                    return currentParent as GrapePackageDeclaration;
                }

                currentParent = currentParent.Parent;
            }

            return null;
        }

        public static string[] GetAppropriateModifiersForEntityAccess(this GrapeClass c, GrapeCodeGeneratorConfiguration config, GrapeEntity entityBeingAccessed) {
            List<string> modifiers = new List<string>();
            GrapeClass accessParentClass = entityBeingAccessed.GetLogicalParentOfEntityType<GrapeClass>();
            if (c == accessParentClass) {
                string accessModifiers = entityBeingAccessed.GetPotentialModifiersOfEntity();
                if (accessModifiers != null) {
                    foreach (string modifier in accessModifiers.Split(' ')) {
                        modifiers.Add(modifier);
                    }
                }
            } else {
                string accessModifiers = entityBeingAccessed.GetPotentialModifiersOfEntity();
                if (accessModifiers != null) {
                    GrapePackageDeclaration currentPackage = c.GetPackage();
                    GrapePackageDeclaration accessPackage = entityBeingAccessed.GetPackage();
                    if (currentPackage == accessPackage || (currentPackage != null && accessPackage != null && currentPackage.PackageName == accessPackage.PackageName)) {
                        if (accessModifiers.Contains("public")) {
                            modifiers.Add("public");
                        }

                        if (accessModifiers.Contains("internal")) {
                            modifiers.Add("internal");
                        }
                    }

                    if (c.Inherits != null && GrapeTypeCheckingUtilities.Instance.GetTypeNameForTypeAccessExpression(config, c.Inherits) == accessParentClass.Name) {
                        if (accessModifiers.Contains("protected")) {
                            modifiers.Add("protected");
                        }
                    }
                }
            }

            if (modifiers.Count == 0) {
                modifiers.Add("public");
            }

            return modifiers.ToArray();
        }

        public static List<GrapeFunction> GetConstructors(this GrapeClass c) {
            List<GrapeFunction> list = new List<GrapeFunction>();
            if (c.Block != null) {
                foreach (GrapeEntity e in c.Block.Children) {
                    if (e is GrapeFunction && ((GrapeFunction)e).Type == GrapeFunction.GrapeFunctionType.Constructor) {
                        list.Add((GrapeFunction)e);
                    }
                }
            }

            return list;
        }

        public static List<GrapeFunction> GetDestructors(this GrapeClass c) {
            List<GrapeFunction> list = new List<GrapeFunction>();
            if (c.Block != null) {
                foreach (GrapeEntity e in c.Block.Children) {
                    if (e is GrapeFunction && ((GrapeFunction)e).Type == GrapeFunction.GrapeFunctionType.Destructor) {
                        list.Add((GrapeFunction)e);
                    }
                }
            }

            return list;
        }

        public static bool IsLogicalChildOfEntityType<TEntity>(this GrapeEntity entity)
            where TEntity : GrapeEntity {
            if (entity is TEntity) {
                return true;
            } else if (entity.Parent is TEntity) {
                return true;
            } else if (entity.Parent != null) {
                return entity.Parent.IsLogicalChildOfEntityType<TEntity>();
            }

            return false;
        }

        public static TEntity GetLogicalParentOfEntityType<TEntity>(this GrapeEntity entity)
            where TEntity : GrapeEntity {
            if (entity is TEntity) {
                return (TEntity)entity;
            } else if (entity.Parent is TEntity) {
                return (TEntity)entity.Parent;
            } else if (entity.Parent != null) {
                return entity.Parent.GetLogicalParentOfEntityType<TEntity>();
            }

            return default(TEntity);
        }
    }
}
