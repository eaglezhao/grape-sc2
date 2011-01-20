using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeAddExpression : GrapeExpression {
        public GrapeAddExpressionType Type { get; internal set; }
        public GrapeExpression Left { get; internal set; }
        public GrapeExpression Right { get; internal set; }

        public enum GrapeAddExpressionType {
            Addition,
            Subtraction
        }
    }
}
