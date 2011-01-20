using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeTryStatement : GrapeStatement {
        public List<GrapeCatchStatement> CatchStatements { get; internal set; }
        public GrapeFinallyStatement FinallyStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }

        public GrapeTryStatement() {
            CatchStatements = new List<GrapeCatchStatement>();
        }
    }
}
