using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeForEachStatement : GrapeStatement {
        public GrapeVariable Variable { get; internal set; }
        public GrapeExpression ContainerExpression { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
