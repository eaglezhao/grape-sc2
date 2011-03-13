using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("false")]
	[Terminal("true")]
	internal class GrapeBoolLiteralExpression: GrapeLiteralExpression<bool> {
		public GrapeBoolLiteralExpression(string value): base(value) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return Value ? GrapeLiteralExpressionType.True : GrapeLiteralExpressionType.False;
			}
		}
	}
}
