using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator)), Export]
    internal sealed class GrapeVariableValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
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
                return new Type[] { typeof(GrapeVariable) };
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
                    if (!v.IsLogicalChildOfEntityType<GrapeFunction>()) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A variable must be the child of a function.", FileName = v.FileName, Offset = v.Offset, Length = v.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage = "";
                    if (v.Type != null && !typeCheckingUtils.DoesTypeExist(Config, v.Type, v.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, v.Type) + "' could not be found. " + errorMessage, FileName = v.FileName, Offset = v.Type.Offset, Length = v.Type.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (v.Value != null && !typeCheckingUtils.DoesExpressionResolveToType(Config, v, v.Value, v.Type, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, v.Type) + "'. " + errorMessage, FileName = v.FileName, Offset = v.Value.Offset, Length = v.Value.Length });
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
