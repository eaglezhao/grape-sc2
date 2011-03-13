namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeBinaryExpression: GrapeExpression {
		private readonly GrapeExpression left;
		private readonly GrapeExpression right;

		protected GrapeBinaryExpression(GrapeExpression left, GrapeExpression right) {
			this.left = left;
			this.right = right;
		}

		public GrapeExpression Left {
			get {
				return left;
			}
		}

		public GrapeExpression Right {
			get {
				return right;
			}
		}
	}
}
