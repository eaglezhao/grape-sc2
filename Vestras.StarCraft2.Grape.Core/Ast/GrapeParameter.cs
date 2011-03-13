using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeParameter: GrapeVariable {
		[Rule("<Formal Param> ::= <Type> Identifier")]
		public GrapeParameter(GrapeType type, GrapeIdentifier name): base(type, name, null) {}

		public override bool IsParameter {
			get {
				return true;
			}
		}
	}
}
