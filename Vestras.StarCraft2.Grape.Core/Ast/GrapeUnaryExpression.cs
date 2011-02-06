using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeUnaryExpression : GrapeExpression {
        private GrapeExpression value;

        public GrapeExpression Value {
            get {
                return value;
            }
            internal set {
                this.value = value;
                if (this.value != null) {
                    this.value.Parent = this;
                    this.value.FileName = FileName;
                }
            }
        }

        public GrapeUnaryExpressionType Type { get; internal set; }

        public enum GrapeUnaryExpressionType {
            Negate,
            Not,
            CurlyEqual,
        }
    }
}
