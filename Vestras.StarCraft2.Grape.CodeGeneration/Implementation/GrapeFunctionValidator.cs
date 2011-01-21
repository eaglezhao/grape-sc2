﻿using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeFunctionValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        private static readonly string[] AccessModifiers = new string[] {
            "public",
            "private",
            "protected",
            "internal"
        };

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type NodeType {
            get {
                return typeof(GrapeFunction);
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
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Top level function declarations are not allowed.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (!(f.Parent is GrapeClass)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A function must be the child of a class.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (f.Parent != null && f.Parent is GrapeClass && ((GrapeClass)f.Parent).Modifiers.Contains("static")) {
                        if (!f.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare instance members in a static class.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    string modifiersErrorMessage;
                    if (!ValidateModifiers(f, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}