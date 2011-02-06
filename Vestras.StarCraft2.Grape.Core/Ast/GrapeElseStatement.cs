using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeElseStatement : GrapeStatement {
        public GrapeIfStatement IfStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
