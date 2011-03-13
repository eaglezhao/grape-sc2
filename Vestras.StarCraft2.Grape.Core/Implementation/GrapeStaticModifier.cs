using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("static")]
	internal class GrapeStaticModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Static;
			}
		}
	}
}