using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeLogicalOrExpression: GrapeConditionalExpression {
		[Rule("<Logical Or Exp> ::= <Logical Or Exp> ~'|' <Logical Xor Exp>")]
		public GrapeLogicalOrExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.LogicalOr;
			}
		}
	}
}
