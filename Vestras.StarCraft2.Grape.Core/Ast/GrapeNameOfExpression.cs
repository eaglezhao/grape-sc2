using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeNameofExpression : GrapeExpression {
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
    }
}
