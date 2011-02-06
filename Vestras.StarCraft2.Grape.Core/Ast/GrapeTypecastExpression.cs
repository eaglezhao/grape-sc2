using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeTypecastExpression : GrapeExpression {
        private GrapeExpression type;
        private GrapeExpression value;

        public GrapeExpression Type {
            get {
                return type;
            }
            internal set {
                type = value;
                if (type != null) {
                    type.Parent = this;
                    type.FileName = FileName;
                }
            }
        }

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
    }
}
