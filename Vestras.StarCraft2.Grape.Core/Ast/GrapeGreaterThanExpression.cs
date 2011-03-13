using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeGreaterThanExpression: GrapeConditionalExpression {
		[Rule("<Compare Exp> ::= <Compare Exp> ~'>' <Shift Exp>")]
		public GrapeGreaterThanExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.GreaterThan;
			}
		}
	}
}
