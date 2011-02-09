using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export]
    internal class GrapeAstUtilities {
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;
        private GrapeAst lastAst;
        private Dictionary<string, List<string>> packageFileNames = new Dictionary<string, List<string>>();
        internal static GrapeAstUtilities Instance;

        private void PopulatePackageFileNames(GrapeAst ast) {
            if (lastAst != ast) {
                packageFileNames.Clear();
                foreach (GrapePackageDeclaration packageDeclaration in GetEntitiesOfType(typeof(GrapePackageDeclaration))) {
                    if (!packageFileNames.ContainsKey(packageDeclaration.PackageName)) {
                        List<string> list = new List<string>();
                        list.Add(packageDeclaration.FileName);
                        packageFileNames.Add(packageDeclaration.PackageName, list);
                    } else {
                        packageFileNames[packageDeclaration.PackageName].Add(packageDeclaration.FileName);
                    }
                }

                lastAst = ast;
            }
        }

        public IEnumerable<GrapeEntity> GetEntitiesOfType(Type type) {
            if (GrapeSkeletonParser.allEntities.ContainsKey(type)) {
                return GrapeSkeletonParser.allEntities[type];
            }

            return new List<GrapeEntity>();
        }

        public IEnumerable<GrapeEntity> GetEntitiesOfTypeInFile(string fileName, Type type) {
            if (GrapeSkeletonParser.allEntitiesWithFileFilter.ContainsKey(fileName) && GrapeSkeletonParser.allEntitiesWithFileFilter[fileName].ContainsKey(type)) {
                return GrapeSkeletonParser.allEntitiesWithFileFilter[fileName][type];
            }

            return new List<GrapeEntity>();
        }

        public IEnumerable<string> GetOtherPackagesInPackageName(string name) {
            return GetSegmentsInQualifiedId(name, false);
        }

        public IList<string> GetSegmentsInQualifiedId(string name, bool includeOriginal) {
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

        public IEnumerable<GrapeFunction> GetFunctionsWithNameFromImportedPackagesInFile(GrapeCodeGeneratorConfiguration config, string functionName, string fileName, GrapeClass c) {
            PopulatePackageFileNames(config.Ast);
            List<string> importedPackageFiles = new List<string>();
            IEnumerable<GrapeEntity> importDeclarationsInFile = GetEntitiesOfTypeInFile(fileName, typeof(GrapeImportDeclaration));
            foreach (GrapeImportDeclaration importDeclaration in importDeclarationsInFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.ImportedPackage) {
                        foreach (string packageFileName in pair.Value) {
                            importedPackageFiles.Add(packageFileName);
                        }
                    }
                }
            }

            IList<string> segmentsInVariableName = GetSegmentsInQualifiedId(functionName, true);
            string actualFunctionName = segmentsInVariableName[segmentsInVariableName.Count - 1];
            string actualQualifiedId = "";
            for (int i = 0; i < segmentsInVariableName.Count - 1; i++) {
                actualQualifiedId += segmentsInVariableName[i] + ".";
            }

            actualQualifiedId = actualQualifiedId.Trim('.');
            if (actualQualifiedId == "this") {
                List<GrapeFunction> l = new List<GrapeFunction>();
                foreach (GrapeFunction f in GetEntitiesOfTypeInFile(fileName, typeof(GrapeFunction))) {
                    if (f.Name == actualFunctionName && f.GetLogicalParentOfEntityType<GrapeClass>() == c) {
                        l.Add(f);
                    }
                }

                return l;
            } else if (actualQualifiedId == "base") {
                List<GrapeFunction> l = new List<GrapeFunction>();
                foreach (GrapeFunction f in GetEntitiesOfTypeInFile(fileName, typeof(GrapeFunction))) {
                    if (f.Name == actualFunctionName && f.GetLogicalParentOfEntityType<GrapeClass>() == GetClassWithNameFromImportedPackagesInFile(config.Ast, typeCheckingUtils.GetTypeNameForTypeAccessExpression(config, c.Inherits), fileName)) {
                        l.Add(f);
                    }
                }

                return l;
            }

            IEnumerable<GrapeEntity> packagesInFile = GetEntitiesOfTypeInFile(fileName, typeof(GrapePackageDeclaration));
            foreach (GrapePackageDeclaration packageDeclaration in packagesInFile) {
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

            List<GrapeFunction> list = new List<GrapeFunction>();
            foreach (string importedPackageFile in importedPackageFiles) {
                IEnumerable<GrapeEntity> functions = GetEntitiesOfTypeInFile(importedPackageFile, typeof(GrapeFunction));
                foreach (GrapeFunction f in functions) {
                    if (f.Name == actualFunctionName && (f.GetLogicalParentOfEntityType<GrapeClass>() == c || c.IsClassInInheritanceTree(config, f.GetLogicalParentOfEntityType<GrapeClass>()))) {
                        list.Add(f);
                    }
                }
            }

            IEnumerable<GrapeEntity> packages = GetEntitiesOfType(typeof(GrapePackageDeclaration));
            IEnumerable<GrapeEntity> allFunctions = GetEntitiesOfType(typeof(GrapeFunction));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualQualifiedId) {
                    foreach (GrapeFunction f in allFunctions) {
                        if (f.Name == actualFunctionName && (f.GetLogicalParentOfEntityType<GrapeClass>() == c || c.IsClassInInheritanceTree(config, f.GetLogicalParentOfEntityType<GrapeClass>()))) {
                            list.Add(f);
                        }
                    }
                }
            }

            if (list.Count > 1) {
                GrapeClass mostTopLevelClassInInheritanceTree = c;
                foreach (GrapeFunction function in list) {
                    GrapeClass functionClass = function.GetLogicalParentOfEntityType<GrapeClass>();
                    if (!mostTopLevelClassInInheritanceTree.IsClassInInheritanceTree(config, functionClass)) {
                        mostTopLevelClassInInheritanceTree = functionClass;
                    }
                }

                List<GrapeFunction> functionsToRemove = new List<GrapeFunction>();
                foreach (GrapeFunction function in list) {
                    if (function.GetLogicalParentOfEntityType<GrapeClass>() != mostTopLevelClassInInheritanceTree) {
                        functionsToRemove.Add(function);
                    }
                }

                functionsToRemove.ForEach(f => list.Remove(f));
            }

            return list;
        }

        public IEnumerable<GrapeVariable> GetVariablesWithNameFromImportedPackagesInFile(GrapeCodeGeneratorConfiguration config, string variableName, string fileName, GrapeEntity e) {
            PopulatePackageFileNames(config.Ast);
            List<string> importedPackageFiles = new List<string>();
            IEnumerable<GrapeEntity> importDeclarationsInFile = GetEntitiesOfTypeInFile(fileName, typeof(GrapeImportDeclaration));
            foreach (GrapeImportDeclaration importDeclaration in importDeclarationsInFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.ImportedPackage) {
                        foreach (string packageFileName in pair.Value) {
                            importedPackageFiles.Add(packageFileName);
                        }
                    }
                }
            }

            IList<string> segmentsInVariableName = GetSegmentsInQualifiedId(variableName, true);
            string actualVariableName = segmentsInVariableName[segmentsInVariableName.Count - 1];
            string actualQualifiedId = "";
            for (int i = 0; i < segmentsInVariableName.Count - 1; i++) {
                actualQualifiedId += segmentsInVariableName[i] + ".";
            }

            actualQualifiedId = actualQualifiedId.Trim('.');
            GrapeClass c = e as GrapeClass;
            if (actualQualifiedId == "this" && c != null) {
                List<GrapeVariable> l = new List<GrapeVariable>();
                foreach (GrapeVariable v in GetEntitiesOfTypeInFile(fileName, typeof(GrapeVariable))) {
                    if (v.Name == actualVariableName && v.GetActualParent() == e) {
                        l.Add(v);
                    }
                }

                return l;
            } else if (actualQualifiedId == "base" && c != null) {
                List<GrapeVariable> l = new List<GrapeVariable>();
                foreach (GrapeVariable v in GetEntitiesOfTypeInFile(fileName, typeof(GrapeVariable))) {
                    if (v.Name == actualVariableName && v.GetActualParent() == GetClassWithNameFromImportedPackagesInFile(config.Ast, typeCheckingUtils.GetTypeNameForTypeAccessExpression(config, c.Inherits), fileName)) {
                        l.Add(v);
                    }
                }

                return l;
            }

            IEnumerable<GrapeEntity> packagesInFile = GetEntitiesOfTypeInFile(fileName, typeof(GrapePackageDeclaration));
            foreach (GrapePackageDeclaration packageDeclaration in packagesInFile) {
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

            List<GrapeVariable> list = new List<GrapeVariable>();
            foreach (string importedPackageFile in importedPackageFiles) {
                IEnumerable<GrapeEntity> variables = GetEntitiesOfTypeInFile(importedPackageFile, typeof(GrapeVariable));
                foreach (GrapeVariable v in variables) {
                    if (v.Name == actualVariableName && v.GetActualParent() == e) {
                        list.Add(v);
                    }
                }
            }

            GrapeFunction parentFunction = e.GetLogicalParentOfEntityType<GrapeFunction>();
            if (parentFunction != null) {
                foreach (GrapeVariable param in parentFunction.Parameters) {
                    if (param.Name == actualVariableName && param.GetActualParent() == parentFunction) {
                        list.Add(param);
                    }
                }
            }

            IEnumerable<GrapeEntity> packages = GetEntitiesOfType(typeof(GrapePackageDeclaration));
            IEnumerable<GrapeEntity> allVariables = GetEntitiesOfType(typeof(GrapeVariable));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualQualifiedId) {
                    foreach (GrapeVariable v in allVariables) {
                        if (v.Name == actualVariableName && v.GetActualParent() == e) {
                            list.Add(v);
                        }
                    }
                }
            }

            foreach (string importedPackageFile in importedPackageFiles) {
                IEnumerable<GrapeEntity> fields = GetEntitiesOfTypeInFile(importedPackageFile, typeof(GrapeField));
                foreach (GrapeField f in fields) {
                    if (f.Name == variableName && f.GetActualParent() == e) {
                        list.Add(f);
                    }
                }
            }

            IEnumerable<GrapeEntity> allFields = GetEntitiesOfType(typeof(GrapeField));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualQualifiedId) {
                    foreach (GrapeField f in allFields) {
                        if (f.Name == actualVariableName && f.GetActualParent() == e) {
                            list.Add(f);
                        }
                    }
                }
            }

            return list;
        }

        public GrapeClass GetClassWithNameFromImportedPackagesInFile(GrapeAst ast, string className, string fileName) {
            PopulatePackageFileNames(ast);
            List<string> importedPackageFiles = new List<string>();
            IEnumerable<GrapeEntity> importDeclarationsInFile = GetEntitiesOfTypeInFile(fileName, typeof(GrapeImportDeclaration));
            foreach (GrapeImportDeclaration importDeclaration in importDeclarationsInFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.ImportedPackage) {
                        foreach (string packageFileName in pair.Value) {
                            importedPackageFiles.Add(packageFileName);
                        }
                    }
                }
            }

            IList<string> packagesInTypeName = GetSegmentsInQualifiedId(className, true);
            string actualTypeName = packagesInTypeName[packagesInTypeName.Count - 1];
            string actualPackageName = "";
            for (int i = 0; i < packagesInTypeName.Count - 1; i++) {
                actualPackageName += packagesInTypeName[i] + ".";
            }

            actualPackageName = actualPackageName.Trim('.');
            IEnumerable<GrapeEntity> packagesInFile = GetEntitiesOfTypeInFile(fileName, typeof(GrapePackageDeclaration));
            foreach (GrapePackageDeclaration packageDeclaration in packagesInFile) {
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
                IEnumerable<GrapeEntity> classes = GetEntitiesOfTypeInFile(importedPackageFile, typeof(GrapeClass));
                foreach (GrapeClass c in classes) {
                    if (c.Name == className) {
                        return c;
                    }
                }
            }

            IEnumerable<GrapeEntity> packages = GetEntitiesOfType(typeof(GrapePackageDeclaration));
            IEnumerable<GrapeEntity> allClasses = GetEntitiesOfType(typeof(GrapeClass));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualPackageName) {
                    foreach (GrapeClass c in allClasses) {
                        if (c.Name == actualTypeName) {
                            return c;
                        }
                    }
                }
            }

            return null;
        }

        private GrapeAstUtilities() {
            Instance = this;
        }
    }
}
