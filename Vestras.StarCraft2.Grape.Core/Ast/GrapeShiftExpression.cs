using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeShiftExpression : GrapeExpression {
        public GrapeShiftExpressionType Type { get; internal set; }
        public GrapeExpression Left { get; internal set; }
        public GrapeExpression Right { get; internal set; }

        public enum GrapeShiftExpressionType {
            ShiftLeft,
            ShiftRight
        }
    }
}
