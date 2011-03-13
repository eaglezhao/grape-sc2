using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeDivisionExpression: GrapeMultiplicationExpression {
		[Rule("<Mult Exp> ::= <Mult Exp> ~'/' <Typecast Exp>")]
		public GrapeDivisionExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeMultiplicationExpressionType Type {
			get {
				return GrapeMultiplicationExpressionType.Division;
			}
		}
	}
}
