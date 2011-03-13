using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("HexLiteral")]
	internal class GrapeHexadecimalLiteralExpression: GrapeIntLiteralExpression {
		public GrapeHexadecimalLiteralExpression(string value): base(value) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return GrapeLiteralExpressionType.Hexadecimal;
			}
		}
	}
}
