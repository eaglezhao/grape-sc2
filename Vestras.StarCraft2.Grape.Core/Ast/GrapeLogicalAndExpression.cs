using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeLogicalAndExpression: GrapeConditionalExpression {
		[Rule("<Logical And Exp> ::= <Logical And Exp> ~'&' <Equality Exp>")]
		public GrapeLogicalAndExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.LogicalAnd;
			}
		}
	}
}
