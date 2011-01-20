using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeMemberExpression : GrapeExpression {
        public GrapeExpression Member { get; internal set; }
        public GrapeMemberExpression Next { get; internal set; }
    }
}
