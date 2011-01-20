using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeTypecastExpression : GrapeExpression {
        public GrapeExpression Type { get; internal set; }
        public GrapeExpression Value { get; internal set; }
    }
}
