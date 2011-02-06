using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeStackExpression : GrapeExpression {
        private GrapeExpression child;

        public GrapeExpression Child {
            get {
                return child;
            }
            internal set {
                child = value;
                if (child != null) {
                    child.Parent = this;
                    child.FileName = FileName;
                }
            }
        }
    }
}
