using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("this")]
	[Terminal("base")]
	public class GrapeObject: GrapeIdentifier {
		public GrapeObject(string name): base(name) {}

		public override bool IsObject {
			get {
				return true;
			}
		}
	}
}
