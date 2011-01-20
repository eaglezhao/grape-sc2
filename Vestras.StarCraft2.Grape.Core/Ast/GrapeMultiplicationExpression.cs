using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeMultiplicationExpression : GrapeExpression {
        public GrapeMultiplicationExpressionType Type { get; internal set; }
        public GrapeExpression Left { get; internal set; }
        public GrapeExpression Right { get; internal set; }

        public enum GrapeMultiplicationExpressionType {
            Multiplication,
            Division,
            Mod
        }
    }
}
