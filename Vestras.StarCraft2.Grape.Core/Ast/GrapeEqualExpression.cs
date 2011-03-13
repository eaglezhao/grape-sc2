using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeEqualExpression: GrapeConditionalExpression {
		[Rule("<Equality Exp> ::= <Equality Exp> ~'==' <Compare Exp>")]
		public GrapeEqualExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.Equal;
			}
		}
	}
}
