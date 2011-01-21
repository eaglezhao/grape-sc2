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
                GrapeVariable v = obj as GrapeVariable;
                if (v != null) {
                    if (!(v.Parent is GrapeFunction)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A variable must be the child of a function.", FileName = v.FileName, Offset = v.Offset, Length = v.Length });
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
