using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeShiftExpression: GrapeBinaryExpression {
		public enum GrapeShiftExpressionType {
			ShiftLeft,
			ShiftRight
		}

		protected GrapeShiftExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public abstract GrapeShiftExpressionType Type {
			get;
		}
	}
}
