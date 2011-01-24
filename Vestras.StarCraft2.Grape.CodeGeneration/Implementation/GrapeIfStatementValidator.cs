using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeIfStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeIfStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeIfStatement s = obj as GrapeIfStatement;
                if (s != null) {
                    if (!GrapeTypeCheckingUtilities.DoesExpressionResolveToType(Config.Ast, s.Condition, "bool") && !GrapeTypeCheckingUtilities.DoesExpressionResolveToType(Config.Ast, s.Condition, "bool_base")) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to type 'bool_base'.", FileName = s.FileName, Offset = s.Condition.Offset, Length = s.Condition.Length });
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
