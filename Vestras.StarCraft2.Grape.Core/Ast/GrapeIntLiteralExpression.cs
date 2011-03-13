using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("DecimalLiteral")]
	internal class GrapeIntLiteralExpression: GrapeLiteralExpression<int> {
		public GrapeIntLiteralExpression(string value): base(value) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return GrapeLiteralExpressionType.Int;
			}
		}
	}
}
