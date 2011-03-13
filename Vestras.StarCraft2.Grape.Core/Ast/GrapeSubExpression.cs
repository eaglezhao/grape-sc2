using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeSubExpression: GrapeAddExpression {
		[Rule("<Add Exp> ::= <Add Exp> ~'-' <Mult Exp>")]
		public GrapeSubExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeAddExpressionType Type {
			get {
				return GrapeAddExpressionType.Subtraction;
			}
		}
	}
}
