using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeVariableValidator : IAstNodeValidator {
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
                return typeof(GrapeField);
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

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeField f = obj as GrapeField;
                if (f != null) {
                    if (!(f.Parent is GrapeFunction-)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A field must be the child of a function.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
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
