using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeNotUnaryExpression: GrapeUnaryExpression {
		[Rule("<Unary Exp> ::= ~'!' <Value>")]
		public GrapeNotUnaryExpression(GrapeExpression value): base(value) {}

		public override GrapeUnaryExpressionType Type {
			get {
				return GrapeUnaryExpressionType.Not;
			}
		}
	}
}
