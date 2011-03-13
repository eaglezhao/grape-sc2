using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeContinueStatement: GrapeStatement {
		[Rule("<Statement> ::= ~continue ~<NL>")]
		public GrapeContinueStatement() {}
	}
}
