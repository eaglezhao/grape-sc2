using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("public")]
	internal class GrapePublicModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Public;
			}
		}
	}
}