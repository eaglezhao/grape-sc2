using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeDeleteStatement : GrapeStatement {
        public GrapeExpression Value { get; internal set; }
        public override bool CanHaveBlock {
            get { return false; }
        }
    }
}
