using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeBinaryOrExpression: GrapeConditionalExpression {
		[Rule("<Or Exp> ::= <Or Exp> ~'||' <And Exp>")]
		public GrapeBinaryOrExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.BinaryOr;
			}
		}
	}
}
