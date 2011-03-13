using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeArrayType: GrapeType {
		[Rule("<Array Type> ::= <Qualified ID> ~'[' ~']'")]
		public GrapeArrayType(GrapeList<GrapeIdentifier> nameParts): base(nameParts) {}

		public override bool IsArray {
			get {
				return true;
			}
		}
	}
}
