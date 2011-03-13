using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("override")]
	internal class GrapeOverrideModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Override;
			}
		}
	}
}