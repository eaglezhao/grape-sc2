using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeBinaryAndExpression: GrapeConditionalExpression {
		[Rule("<And Exp> ::= <And Exp> ~'&&' <Logical Or Exp>")]
		public GrapeBinaryAndExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeConditionalExpressionType Type {
			get {
				return GrapeConditionalExpressionType.BinaryAnd;
			}
		}
	}
}
