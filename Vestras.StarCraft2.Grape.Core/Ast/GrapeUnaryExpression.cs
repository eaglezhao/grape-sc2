using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeUnaryExpression: GrapeExpression {
		public enum GrapeUnaryExpressionType {
			Negate,
			Not,
			CurlyEqual,
		}

		private readonly GrapeExpression value;

		protected GrapeUnaryExpression(GrapeExpression value) {
			this.value = value;
		}

		public abstract GrapeUnaryExpressionType Type {
			get;
		}

		public GrapeExpression Value {
			get {
				return value;
			}
		}
	}
}
