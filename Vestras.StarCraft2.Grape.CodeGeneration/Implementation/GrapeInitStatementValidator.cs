using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeInitStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeInitStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeInitStatement s = obj as GrapeInitStatement;
                if (s != null) {
                    // TODO: implement signature validation
                }
            }

            return true;
        }
    }
}
