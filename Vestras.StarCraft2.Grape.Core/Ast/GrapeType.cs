namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeType: GrapeEntity {
		protected GrapeType() {}

		public virtual bool IsArray {
			get {
				return false;
			}
		}
	}
}
