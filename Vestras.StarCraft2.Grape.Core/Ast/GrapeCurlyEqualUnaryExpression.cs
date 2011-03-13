using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeCurlyEqualUnaryExpression: GrapeUnaryExpression {
		[Rule("<Unary Exp> ::= ~'~' <Value>")]
		public GrapeCurlyEqualUnaryExpression(GrapeExpression value): base(value) {}

		public override GrapeUnaryExpressionType Type {
			get {
				return GrapeUnaryExpressionType.CurlyEqual;
			}
		}
	}
}
