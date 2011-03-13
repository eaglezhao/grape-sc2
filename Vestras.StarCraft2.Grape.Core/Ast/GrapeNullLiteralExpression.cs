using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("null")]
	internal class GrapeNullLiteralExpression: GrapeLiteralExpression {
		public GrapeNullLiteralExpression(string value): base(value) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return GrapeLiteralExpressionType.Null;
			}
		}
	}
}
