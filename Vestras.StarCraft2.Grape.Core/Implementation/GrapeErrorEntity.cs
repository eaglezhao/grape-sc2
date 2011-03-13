using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("(Error)")]
	internal sealed class GrapeErrorEntity: GrapeEntity {
		private readonly string error;

		public GrapeErrorEntity(string error) {
			this.error = error;
		}

		public string Error {
			get {
				return error;
			}
		}
	}
}