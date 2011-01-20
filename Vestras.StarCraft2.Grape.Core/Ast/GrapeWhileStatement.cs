using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeWhileStatement : GrapeStatement {
        public GrapeExpression Condition { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
