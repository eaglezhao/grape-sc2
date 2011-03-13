using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	internal class GrapeDestructor: GrapeMethod {
		[Rule("<Destructor Dec> ::= <Modifier List Opt> ~dctor Identifier ~'(' ~')' ~':' <Stm Block>")]
		public GrapeDestructor(GrapeList<GrapeModifier> modifiers, GrapeIdentifier name, GrapeList<GrapeStatement> body): base(modifiers, name, null, body) {}

		public override GrapeMethodType Type {
			get {
				return GrapeMethodType.Destructor;
			}
		}
	}
}