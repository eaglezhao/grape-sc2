using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeThrowStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeThrowStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeThrowStatement s = obj as GrapeThrowStatement;
                if (s != null) {
                    if (s.ThrowExpression == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A throw keyword must be followed by an object expression returning an exception value.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage = "";
                    if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.ThrowExpression, "exception", ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve throw expression to type 'exception'. " + errorMessage, FileName = s.FileName, Offset = s.ThrowExpression.Offset, Length = s.ThrowExpression.Length });
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
