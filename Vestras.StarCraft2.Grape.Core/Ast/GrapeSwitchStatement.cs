using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeSwitchStatement : GrapeStatement {
        public GrapeExpression SwitchTarget { get; internal set; }
        public GrapeDefaultStatement DefaultStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
