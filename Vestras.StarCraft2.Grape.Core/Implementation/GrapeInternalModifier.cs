using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("internal")]
	internal class GrapeInternalModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Internal;
			}
		}
	}
}