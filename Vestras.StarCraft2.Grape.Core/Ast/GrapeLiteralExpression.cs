using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeLiteralExpression : GrapeExpression {
        public GrapeLiteralExpressionType Type { get; internal set; }
        public string Value { get; internal set; }

        public enum GrapeLiteralExpressionType {
            Hexadecimal,
            Int,
            Real,
            String,
            True,
            False,
            Null
        }
    }
}
