using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeClassValidator : IAstNodeValidator {
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
                return typeof(GrapeClass);
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

        private bool ValidateModifiers(GrapeClass f, out string errorMessage) {
            errorMessage = "";
            bool hasAccessModifier = false;
            bool isStatic = false;
            bool isAbstract = false;
            bool isSealed = false;
            bool isOverride = false;
            foreach (string modifier in f.Modifiers.Split(' ')) {
                if (IsModifierAccessModifier(modifier)) {
                    if (hasAccessModifier) {
                        errorMessage = "A class cannot have multiple access modifiers.";
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

            if (isStatic && isAbstract) {
                errorMessage = "A class cannot be declared static and abstract at the same time.";
                return false;
            }

            if (isSealed) {
                errorMessage = "A class cannot be declared sealed.";
                return false;
            }

            if (isOverride) {
                errorMessage = "A class cannot be declared override.";
                return false;
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeClass c = obj as GrapeClass;
                if (c != null) {
                    string modifiersErrorMessage;
                    if (!ValidateModifiers(c, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = c.FileName, Offset = c.Offset, Length = c.Length });
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
