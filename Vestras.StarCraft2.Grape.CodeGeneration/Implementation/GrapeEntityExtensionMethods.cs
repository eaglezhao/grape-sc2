using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeEntityExtensionMethods {
        public static GrapeEntity GetActualParent(this GrapeEntity entity) { // left for compat
            if (entity.Parent != null) {
                return entity.Parent;
            }

            return null;
        }

        public static string GetPotentialEntityName(this GrapeEntity entity) {
            if (entity is GrapeMethod) {
                return ((GrapeMethod)entity).Name;
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

            string inheritsQualifiedId = c.Inherits.ToString();
            return GrapeAstUtilities.Instance.GetClassWithNameFromImportedPackagesInFile(ast, inheritsQualifiedId, c.FileName);
        }

        private static List<GrapeMethod> GetMethodsWithNameInInheritanceTree(this GrapeClass c, GrapeAst ast, string name) {
            List<GrapeMethod> methods = new List<GrapeMethod>();
            foreach (GrapeEntity entity in c.GetChildren()) {
                if (entity is GrapeMethod && ((GrapeMethod)entity).Name == name) {
                    methods.Add((GrapeMethod)entity);
                }
            }

            GrapeClass baseClass = c.GetBaseClass(ast);
            if (baseClass != null) {
                methods.AddRange(baseClass.GetMethodsWithNameInInheritanceTree(ast, name));
            }

            return methods;
        }

        public static GrapeMethod GetOverridingMethod(this GrapeMethod method, GrapeCodeGeneratorConfiguration config, out string errorMessage) {
            errorMessage = "";
            GrapeClass c = method.GetLogicalParentOfEntityType<GrapeClass>();
            if (c != null && c.Inherits != null) {
                GrapeClass baseClass = c.GetBaseClass(config.Ast);
                if (baseClass != null) {
                    List<GrapeMethod> methods = baseClass.GetMethodsWithNameInInheritanceTree(config.Ast, method.Name);
                    if (methods.Count == 0) {
                        errorMessage = "Cannot find function with name '" + method.Name + "' in base classes.";
                        return null;
                    }

                    GrapeModifier.GrapeModifierType modifiers = c.GetAppropriateModifiersForEntityAccess(config, methods[0]);
                    IEnumerable<GrapeVariable> parameters = method.Parameters;
                    List<GrapeMethod> methodsWithSignature = GrapeTypeCheckingUtilities.Instance.GetMethodsWithSignature(config, methods, GrapeModifier.GrapeModifierType.Public, method.Name, method.ReturnType, (new List<GrapeVariable>(parameters)), ref errorMessage);
                    List<GrapeMethod> methodsToRemove = new List<GrapeMethod>();
                    foreach (GrapeMethod m in methodsWithSignature) {
                        if (m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Sealed)) {
                            methodsToRemove.Add(m);
                        }
                    }

                    methodsToRemove.ForEach(f => methodsWithSignature.Remove(f));
                    if (methodsWithSignature.Count == 0) {
                        errorMessage = "'" + GrapeTypeCheckingUtilities.Instance.GetMethodSignatureString(config, method.Name, method.ReturnType, (new List<GrapeVariable>(parameters))) + "': no suitable function found to override.";
                        return null;
                    }

                    if (errorMessage != "") {
                        return null;
                    }

                    return methodsWithSignature[0];
                } else {
                    string inheritsQualifiedId = c.Inherits.ToString();
                    errorMessage = "Cannot finding base class '" + inheritsQualifiedId + "'.";
                }
            } else {
                errorMessage = "Cannot find class parent of function '" + method.Name + "'.";
            }

            return null;
        }

        public static bool IsClassInInheritanceTree(this GrapeClass c, GrapeCodeGeneratorConfiguration config, GrapeClass testingClass) {
            return GrapeTypeCheckingUtilities.Instance.IsTypeInClassInheritanceTree(config, testingClass.Name, c);
        }

        public static GrapeModifier.GrapeModifierType GetPotentialModifiersOfEntity(this GrapeEntity entity) {
            if (entity is GrapeField) {
                return ((GrapeField)entity).Modifiers;
            } else if (entity is GrapeMethod) {
                return ((GrapeMethod)entity).Modifiers;
            } else if (entity is GrapeClass) {
                return ((GrapeClass)entity).Modifiers;
            }

            return GrapeModifier.GrapeModifierType.Default;
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

        public static GrapeModifier.GrapeModifierType GetAppropriateModifiersForEntityAccess(this GrapeClass c, GrapeCodeGeneratorConfiguration config, GrapeEntity entityBeingAccessed) {
            GrapeModifier.GrapeModifierType modifiers = GrapeModifier.GrapeModifierType.Default;
            GrapeClass accessParentClass = entityBeingAccessed.GetLogicalParentOfEntityType<GrapeClass>();
            if (c == accessParentClass) {
                GrapeModifier.GrapeModifierType accessModifiers = entityBeingAccessed.GetPotentialModifiersOfEntity();
                if (accessModifiers != 0) {
                    modifiers |= accessModifiers;
                }
            } else {
                GrapeModifier.GrapeModifierType accessModifiers = entityBeingAccessed.GetPotentialModifiersOfEntity();
                if (accessModifiers != 0) {
                    GrapePackageDeclaration currentPackage = c.GetPackage();
                    GrapePackageDeclaration accessPackage = entityBeingAccessed.GetPackage();
                    if (currentPackage == accessPackage || (currentPackage != null && accessPackage != null && currentPackage.PackageName == accessPackage.PackageName)) {
                        if (accessModifiers.Contains(GrapeModifier.GrapeModifierType.Public)) {
                            modifiers |= GrapeModifier.GrapeModifierType.Public;
                        }

                        if (accessModifiers.Contains(GrapeModifier.GrapeModifierType.Internal)) {
                            modifiers |= GrapeModifier.GrapeModifierType.Internal;
                        }
                    }

                    if (c.Inherits != null && GrapeTypeCheckingUtilities.Instance.GetTypeNameForTypeAccessExpression(config, c.Inherits) == accessParentClass.Name) {
                        if (accessModifiers.Contains(GrapeModifier.GrapeModifierType.Protected)) {
                            modifiers |= GrapeModifier.GrapeModifierType.Protected;
                        }
                    }
                }
            }

            if (modifiers == GrapeModifier.GrapeModifierType.Default) {
                modifiers = GrapeModifier.GrapeModifierType.Public;
            }

            return modifiers;
        }

        public static List<GrapeMethod> GetConstructors(this GrapeClass c) {
            List<GrapeMethod> list = new List<GrapeMethod>();
            if (c != null) {
                foreach (GrapeEntity e in c.GetChildren()) {
                    if (e is GrapeMethod && ((GrapeMethod)e).Type == GrapeMethod.GrapeMethodType.Constructor) {
                        list.Add((GrapeMethod)e);
                    }
                }
            }

            return list;
        }

        public static List<GrapeMethod> GetDestructors(this GrapeClass c) {
            List<GrapeMethod> list = new List<GrapeMethod>();
            if (c != null) {
                foreach (GrapeEntity e in c.GetChildren()) {
                    if (e is GrapeMethod && ((GrapeMethod)e).Type == GrapeMethod.GrapeMethodType.Destructor) {
                        list.Add((GrapeMethod)e);
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
            if (entity == null) {
                return default(TEntity);
            }

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
