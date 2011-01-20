using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeUnaryExpression : GrapeExpression {
        public GrapeExpression Value { get; internal set; }
        public GrapeUnaryExpressionType Type { get; internal set; }

        public enum GrapeUnaryExpressionType {
            Negate,
            Not,
            CurlyEqual,
        }
    }
}
