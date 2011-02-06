using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeElseIfStatement : GrapeElseStatement {
        public GrapeExpression Condition { get; internal set; }
    }
}
