using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeFinallyClause: GrapeStatementWithBlock {
		[Rule("<Finally Clause Opt> ::=")]
		public GrapeFinallyClause(): this(null) {}

		[Rule("<Finally Clause Opt> ::= ~finally ~':' <Stm Block>")]
		public GrapeFinallyClause(GrapeList<GrapeStatement> statements): base(statements) {}
	}
}
