using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeBreakStatement : GrapeStatement {
        public override bool CanHaveBlock {
            get { return false; }
        }
    }
}
