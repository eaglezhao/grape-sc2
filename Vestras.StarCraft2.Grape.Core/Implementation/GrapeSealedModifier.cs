using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("sealed")]
	internal class GrapeSealedModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Sealed;
			}
		}
	}
}