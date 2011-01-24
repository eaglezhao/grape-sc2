using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator)), Export]
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

                    if (v.Type != null && !GrapeTypeCheckingUtilities.DoesTypeExist(Config.Ast, v.Type, v.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + GrapeTypeCheckingUtilities.GetTypeNameForTypeAccessExpression(v.Type) + "' could not be found.", FileName = v.FileName, Offset = v.Type.Offset, Length = v.Type.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (v.Value != null && !GrapeTypeCheckingUtilities.DoesExpressionResolveToType(Config.Ast, v.Value, v.Type)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + GrapeTypeCheckingUtilities.GetTypeNameForTypeAccessExpression(v.Type) + "'.", FileName = v.FileName, Offset = v.Value.Offset, Length = v.Value.Length });
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
