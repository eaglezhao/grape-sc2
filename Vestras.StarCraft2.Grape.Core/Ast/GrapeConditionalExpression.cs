using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeConditionalExpression : GrapeExpression {
        public GrapeConditionalExpressionType Type { get; internal set; }
        public GrapeExpression Left { get; internal set; }
        public GrapeExpression Right { get; internal set; }

        public enum GrapeConditionalExpressionType {
            LessThan,
            GreaterThan,
            LessThanOrEqual,
            GreaterThanOrEqual,
            Equal,
            NotEqual,
            LogicalAnd,
            LogicalOr,
            LogicalXor,
            BinaryAnd,
            BinaryOr,
        }
    }
}
