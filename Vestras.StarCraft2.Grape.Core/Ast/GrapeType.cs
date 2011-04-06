namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeType: GrapeEntity {
		protected GrapeType() {}

        public abstract GrapeExpression ToExpression();

		public virtual bool IsArray {
			get {
				return false;
			}
		}
	}
}
