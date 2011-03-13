using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("abstract")]
	internal class GrapeAbstractModifier: GrapeModifier {
		public override GrapeModifierType Type {
			get {
				return GrapeModifierType.Abstract;
			}
		}
	}
}