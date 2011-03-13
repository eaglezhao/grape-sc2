using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeShiftLeftExpression: GrapeShiftExpression {
		[Rule("<Shift Exp> ::= <Shift Exp> ~'<<' <Add Exp>")]
		public GrapeShiftLeftExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public override GrapeShiftExpressionType Type {
			get {
				return GrapeShiftExpressionType.ShiftLeft;
			}
		}
	}
}
