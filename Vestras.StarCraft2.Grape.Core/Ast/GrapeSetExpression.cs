using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeSetExpression : GrapeMemberExpression {
        public GrapeExpression Value { get; internal set; }
    }
}
