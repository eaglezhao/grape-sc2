using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeConditionalExpression: GrapeBinaryExpression {
		public enum GrapeConditionalExpressionType {
			LessThan,
			GreaterThan,
			LessThanOrEqual,
			GreaterThanOrEqual,
			Equal,
			NotEqual,
			LogicalAnd,
			LogicalOr,
			LogicalXor,
			BinaryAnd,
			BinaryOr
		}

		protected GrapeConditionalExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public abstract GrapeConditionalExpressionType Type {
			get;
		}
	}
}
