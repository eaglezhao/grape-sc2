using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeNotEqualExpression: GrapeConditionalExpression {
		[Rule("<Equality Exp> ::= <Equality Exp> ~'!=' <Compare Exp>")]
		public GrapeNotEqualExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.NotEqual;
			}
		}
	}
}
