using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeBreakStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeBreakStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeBreakStatement s = obj as GrapeBreakStatement;
                if (s != null) {
                    if (!s.IsLogicalChildOfEntityType<GrapeWhileStatement>() || !s.IsLogicalChildOfEntityType<GrapeForEachStatement>()) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A break statement must be the logical child of a while or foreach statement.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
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
