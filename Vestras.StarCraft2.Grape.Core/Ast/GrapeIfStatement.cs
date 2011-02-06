using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public sealed class GrapeIfStatement : GrapeStatement {
        public List<GrapeElseStatement> ElseStatements { get; internal set; }
        public List<GrapeElseIfStatement> ElseIfStatements { get; internal set; }
        public GrapeExpression Condition { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }

        public GrapeIfStatement() {
            ElseStatements = new List<GrapeElseStatement>();
            ElseIfStatements = new List<GrapeElseIfStatement>();
        }
    }
}
