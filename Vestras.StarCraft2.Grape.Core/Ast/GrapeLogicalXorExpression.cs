using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeLogicalXorExpression: GrapeConditionalExpression {
		[Rule("<Logical Xor Exp> ::= <Logical Xor Exp> ~'^' <Logical And Exp>")]
		public GrapeLogicalXorExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.LogicalXor;
			}
		}
	}
}
