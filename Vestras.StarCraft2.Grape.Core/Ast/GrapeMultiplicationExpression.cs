using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeMultiplicationExpression : GrapeExpression {
        private GrapeExpression left;
        private GrapeExpression right;

        public GrapeMultiplicationExpressionType Type { get; internal set; }
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

        public enum GrapeMultiplicationExpressionType {
            Multiplication,
            Division,
            Mod
        }
    }
}
