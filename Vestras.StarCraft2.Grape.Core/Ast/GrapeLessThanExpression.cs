using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeLessThanExpression: GrapeConditionalExpression {
		[Rule("<Compare Exp> ::= <Compare Exp> ~'<' <Shift Exp>")]
		public GrapeLessThanExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.LessThan;
			}
		}
	}
}
