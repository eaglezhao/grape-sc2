using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeBreakStatement: GrapeStatement {
		[Rule("<Statement> ::= ~break ~<NL>")]
		public GrapeBreakStatement() {}
	}
}
