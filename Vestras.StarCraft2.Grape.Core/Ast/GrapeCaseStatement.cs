using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeCaseStatement : GrapeStatement {
        public GrapeExpression CaseValue { get; internal set; }
        public GrapeSwitchStatement SwitchStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
