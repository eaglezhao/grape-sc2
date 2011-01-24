using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeAstUtilities {
        private static GrapeAst lastAst;
        private static Dictionary<string, List<string>> packageFileNames = new Dictionary<string, List<string>>();

        private static void PopulatePackageFileNames(GrapeAst ast) {
            if (lastAst != ast) {
                packageFileNames.Clear();
                foreach (GrapePackageDeclaration packageDeclaration in GetEntitiesOfType<GrapePackageDeclaration>(ast)) {
                    if (!packageFileNames.ContainsKey(packageDeclaration.PackageName)) {
                        List<string> list = new List<string>();
                        list.Add(packageDeclaration.FileName);
                        packageFileNames.Add(packageDeclaration.PackageName, list);
                    } else {
                        packageFileNames[packageDeclaration.PackageName].Add(packageDeclaration.FileName);
                    }
                }
            }
        }

        public static IEnumerable<TEntity> GetEntitiesOfType<TEntity>(IEnumerable<GrapeEntity> entities)
            where TEntity : GrapeEntity {
            List<TEntity> list = new List<TEntity>();
            foreach (GrapeEntity entity in entities) {
                if (entity is TEntity) {
                    list.Add((TEntity)entity);
                }

                if (entity is GrapeStatement) {
                    if (((GrapeStatement)entity).CanHaveBlock) {
                        list.AddRange(GetEntitiesOfType<TEntity>(((GrapeStatement)entity).Block.Children));
                    }
                } else if (entity is GrapeEntityWithBlock) {
                    list.AddRange(GetEntitiesOfType<TEntity>(((GrapeEntityWithBlock)entity).Block.Children));
                } else if (entity is GrapeBlock) {
                    list.AddRange(GetEntitiesOfType<TEntity>(((GrapeBlock)entity).Children));
                } else if (entity is GrapePackageDeclaration) {
                    list.AddRange(GetEntitiesOfType<TEntity>(((GrapePackageDeclaration)entity).Children));
                }
            }

            return list;
        }

        public static IEnumerable<TEntity> GetEntitiesOfType<TEntity>(GrapeAst ast)
            where TEntity : GrapeEntity {
            return GetEntitiesOfType<TEntity>(ast.Children);
        }

        public static IEnumerable<TEntity> GetEntitiesOfTypeInFile<TEntity>(IEnumerable<GrapeEntity> entities, string fileName)
            where TEntity : GrapeEntity {
            IEnumerable<TEntity> enumerable = GetEntitiesOfType<TEntity>(entities);
            List<TEntity> list = new List<TEntity>();
            foreach (TEntity entity in enumerable) {
                if (entity.FileName == fileName) {
                    list.Add(entity);
                }
            }

            return list;
        }

        public static IEnumerable<TEntity> GetEntitiesOfTypeInFile<TEntity>(GrapeAst ast, string fileName)
            where TEntity : GrapeEntity {
            return GetEntitiesOfTypeInFile<TEntity>(ast.Children, fileName);
        }

        private static IEnumerable<GrapePackageDeclaration> GetPackageDeclarationsInFile(GrapeAst ast, string fileName) {
            return GetEntitiesOfTypeInFile<GrapePackageDeclaration>(ast, fileName);
        }

        public static IEnumerable<string> GetOtherPackagesInPackageName(string name) {
            return GetSegmentsInQualifiedId(name, false);
        }

        public static IList<string> GetSegmentsInQualifiedId(string name, bool includeOriginal) {
            List<string> packages = new List<string>();
            string currentName = name;
            int lastIndexOfDot = 0;
            while ((lastIndexOfDot = currentName.LastIndexOf('.')) > -1) {
                currentName = currentName.Substring(0, lastIndexOfDot);
                packages.Add(currentName);
            }

            if (includeOriginal) {
                int dotIndex = name.LastIndexOf('.');
                if (dotIndex > -1) {
                    packages.Add(name.Substring(dotIndex + 1, name.Length - (dotIndex + 1)));
                } else {
                    packages.Add(name);
                }
            }

            return packages;
        }

        public static GrapeClass GetClassWithNameFromImportedPackagesInFile(GrapeAst ast, string className, string fileName) {
            PopulatePackageFileNames(ast);
            List<string> importedPackageFiles = new List<string>();
            foreach (GrapeImportDeclaration importDeclaration in GetEntitiesOfTypeInFile<GrapeImportDeclaration>(ast, fileName)) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.ImportedPackage) {
                        foreach (string packageFileName in pair.Value) {
                            importedPackageFiles.Add(packageFileName);
                        }
                    }
                }
            }

            IEnumerable<GrapePackageDeclaration> packagesInSpecifiedFile = GetPackageDeclarationsInFile(ast, fileName);
            IList<string> packagesInTypeName = GetSegmentsInQualifiedId(className, true);
            string actualTypeName = packagesInTypeName[packagesInTypeName.Count - 1];
            string actualPackageName = "";
            for (int i = 0; i < packagesInTypeName.Count - 1; i++) {
                actualPackageName += packagesInTypeName[i] + ".";
            }

            actualPackageName = actualPackageName.Trim('.');
            foreach (GrapePackageDeclaration packageDeclaration in packagesInSpecifiedFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == packageDeclaration.PackageName) {
                        IEnumerable<string> otherPackagesInPackage = GetOtherPackagesInPackageName(packageDeclaration.PackageName);
                        foreach (string otherPackage in otherPackagesInPackage) {
                            foreach (KeyValuePair<string, List<string>> childPair in packageFileNames) {
                                if (childPair.Key == otherPackage) {
                                    importedPackageFiles.AddRange(childPair.Value);
                                }
                            }
                        }

                        importedPackageFiles.AddRange(pair.Value);
                    }
                }
            }

            foreach (string importedPackageFile in importedPackageFiles) {
                foreach (GrapeClass c in GetEntitiesOfTypeInFile<GrapeClass>(ast, importedPackageFile)) {
                    if (c.Name == className) {
                        return c;
                    }
                }
            }

            foreach (GrapePackageDeclaration packageDeclaration in GetEntitiesOfType<GrapePackageDeclaration>(ast)) {
                if (packageDeclaration.PackageName == actualPackageName) {
                    foreach (GrapeClass c in GetEntitiesOfType<GrapeClass>(packageDeclaration.Children)) {
                        if (c.Name == actualTypeName) {
                            return c;
                        }
                    }
                }
            }

            return null;
        }
    }
}
