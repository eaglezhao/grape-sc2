using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeInitStatement : GrapeStatement {
        public GrapeInitStatementType Type { get; internal set; }
        public List<GrapeExpression> Parameters { get; internal set; }
        public override bool CanHaveBlock {
            get { return false; }
        }

        public GrapeInitStatement() {
            Parameters = new List<GrapeExpression>();
        }

        public enum GrapeInitStatementType {
            This,
            Base,
            Unknown
        }
    }
}
