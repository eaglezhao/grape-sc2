namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeArrayExpression: GrapeAccessExpression {
		public static GrapeArrayExpression Create(GrapeExpression arrayIndex, GrapeAccessExpression next) {
			GrapeArrayExpression result = new GrapeArrayExpression(arrayIndex, next);
			result.InitializeFromTemplate(arrayIndex);
			return result;
		}

		private readonly GrapeExpression arrayIndex;

		private GrapeArrayExpression(GrapeExpression arrayIndex, GrapeAccessExpression next): base(next) {
			this.arrayIndex = arrayIndex;
		}

		public GrapeExpression ArrayIndex {
			get {
				return arrayIndex;
			}
		}

		public override sealed GrapeAccessExpressionType Type {
			get {
				return GrapeAccessExpressionType.Array;
			}
		}
	}
}
