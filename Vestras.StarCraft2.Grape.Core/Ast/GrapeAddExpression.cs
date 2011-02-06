using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeAddExpression : GrapeExpression {
        private GrapeExpression left;
        private GrapeExpression right;

        public GrapeAddExpressionType Type { get; internal set; }
        public GrapeExpression Left {
            get {
                return left;
            }
            internal set {
                left = value;
                if (left != null) {
                    left.Parent = this;
                    left.FileName = FileName;
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
                    right.FileName = FileName;
                }
            }
        }

        public enum GrapeAddExpressionType {
            Addition,
            Subtraction
        }
    }
}
