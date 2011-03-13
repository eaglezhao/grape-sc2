using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("RealLiteral")]
	internal class GrapeRealLiteralExpression: GrapeLiteralExpression<double> {
		public GrapeRealLiteralExpression(string value): base(value) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return GrapeLiteralExpressionType.Real;
			}
		}
	}
}
