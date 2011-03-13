using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	internal class GrapeConstructor: GrapeMethod {
		[Rule("<Constructor Dec> ::= <Modifier List Opt> ~ctor Identifier ~'(' <Formal Param List Opt> ~')' ~':' <Stm Block>")]
		public GrapeConstructor(GrapeList<GrapeModifier> modifiers, GrapeIdentifier name, GrapeList<GrapeParameter> parameters, GrapeList<GrapeStatement> body): base(modifiers, name, parameters, body) {}

		public override GrapeMethodType Type {
			get {
				return GrapeMethodType.Constructor;
			}
		}
	}
}