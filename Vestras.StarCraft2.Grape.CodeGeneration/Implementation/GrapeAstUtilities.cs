using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
                foreach (GrapePackageDeclaration packageDeclaration in GetEntitiesOfType(ast, typeof(GrapePackageDeclaration))) {
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

        public IEnumerable<GrapeEntity> GetEntitiesOfType(GrapeAst ast, Type type) {
            return ast.GetChildrenRecursive().Where(delegate(GrapeEntity e) {
                return e.GetType() == type;
            });
        }

        public IEnumerable<GrapeEntity> GetEntitiesOfTypeInFile(GrapeAst ast, string fileName, Type type) {
            return ast.GetChildrenRecursive().Where(delegate(GrapeEntity e) {
                return e.GetType() == type && e.FileName == fileName;
            });
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

        public IEnumerable<GrapeMethod> GetMethodsWithNameFromImportedPackagesInFile(GrapeCodeGeneratorConfiguration config, string methodName, string fileName, GrapeClass c) {
            PopulatePackageFileNames(config.Ast);
            List<string> importedPackageFiles = new List<string>();
            IEnumerable<GrapeEntity> importDeclarationsInFile = GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapeImportDeclaration));
            foreach (GrapeImportDeclaration importDeclaration in importDeclarationsInFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.PackageName) {
                        foreach (string packageFileName in pair.Value) {
                            importedPackageFiles.Add(packageFileName);
                        }
                    }
                }
            }

            IList<string> segmentsInVariableName = GetSegmentsInQualifiedId(methodName, true);
            string actualFunctionName = segmentsInVariableName[segmentsInVariableName.Count - 1];
            string actualQualifiedId = "";
            for (int i = 0; i < segmentsInVariableName.Count - 1; i++) {
                actualQualifiedId += segmentsInVariableName[i] + ".";
            }

            actualQualifiedId = actualQualifiedId.Trim('.');
            if (actualQualifiedId == "this") {
                List<GrapeMethod> l = new List<GrapeMethod>();
                foreach (GrapeMethod m in GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapeMethod))) {
                    if (m.Name == actualFunctionName && m.GetLogicalParentOfEntityType<GrapeClass>() == c) {
                        l.Add(m);
                    }
                }

                return l;
            } else if (actualQualifiedId == "base") {
                List<GrapeMethod> l = new List<GrapeMethod>();
                foreach (GrapeMethod m in GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapeMethod))) {
                    if (m.Name == actualFunctionName && m.GetLogicalParentOfEntityType<GrapeClass>() == GetClassWithNameFromImportedPackagesInFile(config.Ast, typeCheckingUtils.GetTypeNameForTypeAccessExpression(config, c.Inherits), fileName)) {
                        l.Add(m);
                    }
                }

                return l;
            }

            IEnumerable<GrapeEntity> packagesInFile = GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapePackageDeclaration));
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

            List<GrapeMethod> list = new List<GrapeMethod>();
            foreach (string importedPackageFile in importedPackageFiles) {
                IEnumerable<GrapeEntity> methods = GetEntitiesOfTypeInFile(config.Ast, importedPackageFile, typeof(GrapeMethod));
                foreach (GrapeMethod m in methods) {
                    if (m.Name == actualFunctionName && (m.GetLogicalParentOfEntityType<GrapeClass>() == c || c.IsClassInInheritanceTree(config, m.GetLogicalParentOfEntityType<GrapeClass>()))) {
                        list.Add(m);
                    }
                }
            }

            IEnumerable<GrapeEntity> packages = GetEntitiesOfType(config.Ast, typeof(GrapePackageDeclaration));
            IEnumerable<GrapeEntity> allMethods = GetEntitiesOfType(config.Ast, typeof(GrapeMethod));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualQualifiedId) {
                    foreach (GrapeMethod m in allMethods) {
                        if (m.Name == actualFunctionName && (m.GetLogicalParentOfEntityType<GrapeClass>() == c || c.IsClassInInheritanceTree(config, m.GetLogicalParentOfEntityType<GrapeClass>()))) {
                            list.Add(m);
                        }
                    }
                }
            }

            if (list.Count > 1) {
                GrapeClass mostTopLevelClassInInheritanceTree = c;
                foreach (GrapeMethod method in list) {
                    GrapeClass functionClass = method.GetLogicalParentOfEntityType<GrapeClass>();
                    if (!mostTopLevelClassInInheritanceTree.IsClassInInheritanceTree(config, functionClass)) {
                        mostTopLevelClassInInheritanceTree = functionClass;
                    }
                }

                List<GrapeMethod> methodsToRemove = new List<GrapeMethod>();
                foreach (GrapeMethod method in list) {
                    if (method.GetLogicalParentOfEntityType<GrapeClass>() != mostTopLevelClassInInheritanceTree) {
                        methodsToRemove.Add(method);
                    }
                }

                methodsToRemove.ForEach(f => list.Remove(f));
            }

            return list;
        }

        public IEnumerable<GrapeVariable> GetVariablesWithNameFromImportedPackagesInFile(GrapeCodeGeneratorConfiguration config, string variableName, string fileName, GrapeEntity e) {
            PopulatePackageFileNames(config.Ast);
            List<string> importedPackageFiles = new List<string>();
            IEnumerable<GrapeEntity> importDeclarationsInFile = GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapeImportDeclaration));
            foreach (GrapeImportDeclaration importDeclaration in importDeclarationsInFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.PackageName) {
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
                foreach (GrapeVariable v in GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapeVariable))) {
                    if (v.Name == actualVariableName && v.GetActualParent() == e) {
                        l.Add(v);
                    }
                }

                return l;
            } else if (actualQualifiedId == "base" && c != null) {
                List<GrapeVariable> l = new List<GrapeVariable>();
                foreach (GrapeVariable v in GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapeVariable))) {
                    if (v.Name == actualVariableName && v.GetActualParent() == GetClassWithNameFromImportedPackagesInFile(config.Ast, typeCheckingUtils.GetTypeNameForTypeAccessExpression(config, c.Inherits), fileName)) {
                        l.Add(v);
                    }
                }

                return l;
            }

            IEnumerable<GrapeEntity> packagesInFile = GetEntitiesOfTypeInFile(config.Ast, fileName, typeof(GrapePackageDeclaration));
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
                IEnumerable<GrapeEntity> variables = GetEntitiesOfTypeInFile(config.Ast, importedPackageFile, typeof(GrapeVariable));
                foreach (GrapeVariable v in variables) {
                    if (v.Name == actualVariableName && (v.GetActualParent() == e || v.GetLogicalParentOfEntityType<GrapeMethod>() == e)) {
                        list.Add(v);
                    }
                }
            }

            GrapeMethod parentMethod = e.GetLogicalParentOfEntityType<GrapeMethod>();
            if (parentMethod != null) {
                foreach (GrapeVariable param in parentMethod.Parameters) {
                    if (param.Name == actualVariableName && param.GetActualParent() == parentMethod) {
                        list.Add(param);
                    }
                }
            }

            IEnumerable<GrapeEntity> packages = GetEntitiesOfType(config.Ast, typeof(GrapePackageDeclaration));
            IEnumerable<GrapeEntity> allVariables = GetEntitiesOfType(config.Ast, typeof(GrapeVariable));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualQualifiedId) {
                    foreach (GrapeVariable v in allVariables) {
                        if (v.Name == actualVariableName && (v.GetActualParent() == e || v.GetLogicalParentOfEntityType<GrapeMethod>() == e)) {
                            list.Add(v);
                        }
                    }
                }
            }

            foreach (string importedPackageFile in importedPackageFiles) {
                IEnumerable<GrapeEntity> fields = GetEntitiesOfTypeInFile(config.Ast, importedPackageFile, typeof(GrapeField));
                foreach (GrapeField f in fields) {
                    if (f.Field.Name == variableName && f.GetActualParent() == e) {
                        list.Add(f.Field);
                    }
                }
            }

            IEnumerable<GrapeEntity> allFields = GetEntitiesOfType(config.Ast, typeof(GrapeField));
            foreach (GrapePackageDeclaration packageDeclaration in packages) {
                if (packageDeclaration.PackageName == actualQualifiedId) {
                    foreach (GrapeField f in allFields) {
                        if (f.Field.Name == actualVariableName && f.GetActualParent() == e) {
                            list.Add(f.Field);
                        }
                    }
                }
            }

            return list;
        }

        public GrapeClass GetClassWithNameFromImportedPackagesInFile(GrapeAst ast, string className, string fileName) {
            PopulatePackageFileNames(ast);
            List<string> importedPackageFiles = new List<string>();
            IEnumerable<GrapeEntity> importDeclarationsInFile = GetEntitiesOfTypeInFile(ast, fileName, typeof(GrapeImportDeclaration));
            foreach (GrapeImportDeclaration importDeclaration in importDeclarationsInFile) {
                foreach (KeyValuePair<string, List<string>> pair in packageFileNames) {
                    if (pair.Key == importDeclaration.PackageName) {
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
            IEnumerable<GrapeEntity> packagesInFile = GetEntitiesOfTypeInFile(ast, fileName, typeof(GrapePackageDeclaration));
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
                IEnumerable<GrapeEntity> classes = GetEntitiesOfTypeInFile(ast, importedPackageFile, typeof(GrapeClass));
                foreach (GrapeClass c in classes) {
                    if (c.Name == className) {
                        return c;
                    }
                }
            }

            IEnumerable<GrapeEntity> packages = GetEntitiesOfType(ast, typeof(GrapePackageDeclaration));
            IEnumerable<GrapeEntity> allClasses = GetEntitiesOfType(ast, typeof(GrapeClass));
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
