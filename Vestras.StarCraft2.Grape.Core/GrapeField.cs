using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	public sealed class GrapeField: GrapeClassItem {
		private readonly GrapeVariable field;

		[Rule("<Field Dec> ::= <Modifier List Opt> <Variable Declarator> ~<NL>")]
		public GrapeField(GrapeList<GrapeModifier> modifiers, GrapeVariable field): base(modifiers) {
			this.field = field;
		}

		public GrapeVariable Field {
			get {
				return field;
			}
		}
	}
}
