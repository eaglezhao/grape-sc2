using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeArrayAccessExpression : GrapeMemberExpression {
        public GrapeExpression Array { get; internal set; }
    }
}
