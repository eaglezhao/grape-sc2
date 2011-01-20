using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeReturnStatement : GrapeStatement {
        public GrapeExpression ReturnValue { get; internal set; }
        public override bool CanHaveBlock {
            get { return false; }
        }
    }
}
