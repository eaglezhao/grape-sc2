using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeWhileStatement: GrapeStatementWithBlock {
		private readonly GrapeExpression condition;

		[Rule("<Statement> ::= ~while <Expression> ~':' <Stm Block>")]
		public GrapeWhileStatement(GrapeExpression condition, GrapeList<GrapeStatement> statements): base(statements) {
			this.condition = condition;
		}

		public GrapeExpression Condition {
			get {
				return condition;
			}
		}
	}
}
