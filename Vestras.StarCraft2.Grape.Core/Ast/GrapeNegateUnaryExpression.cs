using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeNegateUnaryExpression: GrapeUnaryExpression {
		[Rule("<Unary Exp> ::= ~'-' <Value>")]
		public GrapeNegateUnaryExpression(GrapeExpression value): base(value) {}

		public override GrapeUnaryExpressionType Type {
			get {
				return GrapeUnaryExpressionType.Negate;
			}
		}
	}
}
