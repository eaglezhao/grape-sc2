using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeAccessExpression: GrapeExpression {
		public enum GrapeAccessExpressionType {
			Root,
			Field,
			Method,
			Array
		}

		private readonly GrapeAccessExpression next;

		protected GrapeAccessExpression(GrapeAccessExpression next) {
			this.next = next;
		}

		public GrapeAccessExpression Next {
			get {
				return next;
			}
		}

		public abstract GrapeAccessExpressionType Type {
			get;
		}
	}
}
