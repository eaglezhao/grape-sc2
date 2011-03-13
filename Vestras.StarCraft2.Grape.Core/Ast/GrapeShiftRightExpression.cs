using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeShiftRightExpression: GrapeShiftExpression {
		[Rule("<Shift Exp> ::= <Shift Exp> ~'>>' <Add Exp>")]
		public GrapeShiftRightExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeShiftExpressionType Type {
			get {
				return GrapeShiftExpressionType.ShiftRight;
			}
		}
	}
}
