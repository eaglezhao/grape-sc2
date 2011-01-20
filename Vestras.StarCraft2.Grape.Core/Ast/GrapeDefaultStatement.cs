using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeDefaultStatement : GrapeStatement {
        public GrapeSwitchStatement SwitchStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
