using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeConditionalExpression : GrapeExpression {
        private GrapeExpression left;
        private GrapeExpression right;

        public GrapeConditionalExpressionType Type { get; internal set; }
        public GrapeExpression Left {
            get {
                return left;
            }
            internal set {
                left = value;
                if (left != null) {
                    left.Parent = this;
                }
            }
        }

        public GrapeExpression Right {
            get {
                return right;
            }
            internal set {
                right = value;
                if (right != null) {
                    right.Parent = this;
                }
            }
        }

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
