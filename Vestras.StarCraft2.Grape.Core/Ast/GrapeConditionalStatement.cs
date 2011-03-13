using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public sealed class GrapeConditionalStatement: GrapeStatementWithBlock {
		private readonly GrapeExpression condition;
		private readonly GrapeConditionalStatement elseStatement;

		[Rule("<Else Statement Opt> ::=")]
		public GrapeConditionalStatement(): this(null, null, null) {}

		[Rule("<Else Statement Opt> ::= ~else ~':' <Stm Block>")]
		public GrapeConditionalStatement(GrapeList<GrapeStatement> statements): this(null, statements, null) {}

		[Rule("<Statement> ::= ~if <Expression> ~':' <Stm Block> <Else Statement Opt>")]
		[Rule("<Else Statement Opt> ::= ~elseif <Expression> ~':' <Stm Block> <Else Statement Opt>")]
		public GrapeConditionalStatement(GrapeExpression condition, GrapeList<GrapeStatement> statements, GrapeConditionalStatement elseStatement): base(statements) {
			this.condition = condition;
			this.elseStatement = (elseStatement != null) && (elseStatement.Statements.Count > 0) ? elseStatement : null;
		}

		public GrapeExpression Condition {
			get {
				return condition;
			}
		}

		public GrapeConditionalStatement ElseStatement {
			get {
				return elseStatement;
			}
		}
	}
}
