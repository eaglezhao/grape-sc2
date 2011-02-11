using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeFunctionValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeVariableValidator variableValidator = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        private static readonly string[] AccessModifiers = new string[] {
            "public",
            "private",
            "protected",
            "internal"
        };

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeFunction) };
            }
        }

        private bool IsModifierAccessModifier(string modifier) {
            foreach (string accessModifier in AccessModifiers) {
                if (modifier == accessModifier) {
                    return true;
                }
            }

            return false;
        }

        private bool ValidateModifiers(GrapeFunction f, out string errorMessage) {
            errorMessage = "";
            bool hasAccessModifier = false;
            bool isStatic = false;
            bool isAbstract = false;
            bool isSealed = false;
            bool isOverride = false;
            foreach (string modifier in f.Modifiers.Split(' ')) {
                if (IsModifierAccessModifier(modifier)) {
                    if (hasAccessModifier) {
                        errorMessage = "A function cannot have multiple access modifiers.";
                        return false;
                    }

                    hasAccessModifier = true;
                } else {
                    if (modifier == "static") {
                        isStatic = true;
                    } else if (modifier == "abstract") {
                        isAbstract = true;
                    } else if (modifier == "sealed") {
                        isSealed = true;
                    } else if (modifier == "override") {
                        isOverride = true;
                    }
                }
            }

            if (isStatic) {
                if (isAbstract || isSealed || isOverride) {
                    errorMessage = "A function cannot be declared static and abstract, sealed or override at the same time.";
                    return false;
                }
            } else if (isAbstract && isSealed) {
                errorMessage = "A function cannot be declared abstract and sealed at the same time.";
                return false;
            } else if (isAbstract && isOverride) {
                errorMessage = "A function cannot be declared abstract and override at the same time.";
                return false;
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeFunction f = obj as GrapeFunction;
                if (f != null) {
                    if (f.Parent == null || f.Parent is GrapePackageDeclaration) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Top level function declarations are not allowed.", FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (!f.IsLogicalChildOfEntityType<GrapeClass>()) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A function must be the child of a class.", FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (f.Parent != null && f.Parent is GrapeClass && ((GrapeClass)f.Parent).Modifiers.Contains("static")) {
                        if (!f.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare instance members in a static class.", FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    GrapeClass c = f.GetLogicalParentOfEntityType<GrapeClass>();
                    if (f.Type == GrapeFunction.GrapeFunctionType.Constructor || f.Type == GrapeFunction.GrapeFunctionType.Destructor) {
                        if (f.Name != c.Name) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A ctor/dctor must have the same name as the parent class.", FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (f.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A ctor/dctor cannot be declared static.", FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (c.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A static class cannot have ctors/dctors.", FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        if (f.Name == c.Name) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A function cannot have the same name as the parent class.", FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    if (f.Modifiers.Contains("abstract") && f.Block.Children.Count > 0) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "An abstract function cannot have an implementation.", FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage;
                    if (f.Modifiers.Contains("override")) {
                        GrapeFunction baseFunction = f.GetOverridingFunction(Config, out errorMessage);
                        if (baseFunction == null) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    if (f.ReturnType != null && !typeCheckingUtils.DoesTypeExist(Config, f.ReturnType, f.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.ReturnType) + "' could not be found.", FileName = f.FileName, Entity = f.ReturnType });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeEntity type = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(Config, f.ReturnType as GrapeMemberExpression, f, out errorMessage)))[0];
                    if (type is GrapeClass) {
                        GrapeClass typeClass = type as GrapeClass;
                        if (typeClass.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare a function of static type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.ReturnType) + "'.", FileName = f.FileName, Entity = f.ReturnType });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    string modifiersErrorMessage;
                    if (!ValidateModifiers(f, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    foreach (GrapeVariable param in f.Parameters) {
                        bool isParamValid = variableValidator.ValidateNode(param);
                        if (!isParamValid) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
