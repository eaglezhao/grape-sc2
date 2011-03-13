using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("private")]
	internal class GrapePrivateModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Private;
			}
		}
	}
}