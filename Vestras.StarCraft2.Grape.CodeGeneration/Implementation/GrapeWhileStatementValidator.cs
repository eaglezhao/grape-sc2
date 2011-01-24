using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeWhileStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeWhileStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeWhileStatement s = obj as GrapeWhileStatement;
                if (s != null) {
                    if (!GrapeTypeCheckingUtilities.DoesExpressionResolveToType(Config.Ast, s.Condition, "bool_base")) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'bool_base'.", FileName = s.FileName, Offset = s.Condition.Offset, Length = s.Condition.Length });
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
