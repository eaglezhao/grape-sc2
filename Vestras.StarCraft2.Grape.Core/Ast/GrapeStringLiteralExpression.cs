using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("StringLiteral")]
	internal class GrapeStringLiteralExpression: GrapeLiteralExpression {
		public GrapeStringLiteralExpression(string value): base(value) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return GrapeLiteralExpressionType.String;
			}
		}
	}
}
