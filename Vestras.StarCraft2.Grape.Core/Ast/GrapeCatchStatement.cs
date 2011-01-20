using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeCatchStatement : GrapeStatement {
        public GrapeVariable Variable { get; internal set; }
        public GrapeTryStatement TryStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
