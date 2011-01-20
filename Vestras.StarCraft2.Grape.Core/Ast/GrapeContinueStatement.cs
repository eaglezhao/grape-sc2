using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeContinueStatement : GrapeStatement {
        public override bool CanHaveBlock {
            get { return false; }
        }
    }
}
