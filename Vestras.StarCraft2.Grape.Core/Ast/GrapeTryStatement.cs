using System;
using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeTryStatement: GrapeStatementWithBlock {
		private readonly ReadOnlyCollection<GrapeCatchClause> catchClauses;
		private readonly GrapeFinallyClause finallyClause;

		[Rule("<Statement> ::= ~try ~':' <Stm Block> <Catch Clause List Opt> <Finally Clause Opt>")]
		public GrapeTryStatement(GrapeList<GrapeStatement> statements, GrapeList<GrapeCatchClause> catchClauses, GrapeFinallyClause finallyClause): base(statements) {
			this.catchClauses = catchClauses.ToList(this).AsReadOnly();
			if (finallyClause.Statements.Count > 0) {
				this.finallyClause = finallyClause;
			}
		}

		public ReadOnlyCollection<GrapeCatchClause> CatchClauses {
			get {
				return catchClauses;
			}
		}

		public GrapeFinallyClause FinallyClause {
			get {
				return finallyClause;
			}
		}
	}
}
