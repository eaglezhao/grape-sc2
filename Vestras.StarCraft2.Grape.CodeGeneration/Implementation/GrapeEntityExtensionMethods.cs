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

        public static bool IsClassInInheritanceTree(this GrapeClass c, GrapeCodeGeneratorConfiguration config, GrapeClass testingClass) {
            //GrapeClass currentClass = c;
            //while (currentClass != null) {
            //    if (currentClass.Name == testingClass.Name) {
            //        return true;
            //    }
            //
            //    if (currentClass.Inherits == null) {
            //        break;
            //    }
            //
            //    currentClass = GrapeAstUtilities.Instance.GetClassWithNameFromImportedPackagesInFile(config.Ast, GrapeTypeCheckingUtilities.Instance.GetTypeNameForTypeAccessExpression(config, currentClass.Inherits), currentClass.FileName);
            //}
            //
            //return false;
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
