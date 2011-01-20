using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeThrowStatement : GrapeStatement {
        public GrapeExpression ThrowExpression { get; internal set; }
        public override bool CanHaveBlock {
            get { return false; }
        }
    }
}
