using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeArrayAccessExpression : GrapeMemberExpression {
        private GrapeExpression array;

        public GrapeExpression Array {
            get {
                return array;
            }
            internal set {
                array = value;
                if (array != null) {
                    array.Parent = this;
                    array.FileName = FileName;
                }
            }
        }
    }
}
