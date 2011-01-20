using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public abstract class GrapeExpression : GrapeEntity {
        // Hide Offset + Length because they are not set on expressions.
        private new int Offset { get; set; }
        private new int Length { get; set; }
        public int Line { get; internal set; }
    }
}
