using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("protected")]
	internal class GrapeProtectedModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Protected;
			}
		}
	}
}