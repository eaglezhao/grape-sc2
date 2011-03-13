using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeGreaterThanOrEqualExpression: GrapeConditionalExpression {
		[Rule("<Compare Exp> ::= <Compare Exp> ~'>=' <Shift Exp>")]
		public GrapeGreaterThanOrEqualExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.GreaterThanOrEqual;
			}
		}
	}
}
