using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeForEachStatement: GrapeStatementWithBlock {
		private readonly GrapeVariable iteratorVariable;
		private readonly GrapeExpression valueExpression;

		[Rule("<Statement> ::= ~foreach <Variable Declarator Uninitialized> ~in <Expression> ~':' <Stm Block>")]
		public GrapeForEachStatement(GrapeVariable iteratorVariable, GrapeExpression valueExpression, GrapeList<GrapeStatement> statements): base(statements) {
			this.iteratorVariable = iteratorVariable;
			this.valueExpression = valueExpression;
		}

		public GrapeVariable IteratorVariable {
			get {
				return iteratorVariable;
			}
		}

		public GrapeExpression ValueExpression {
			get {
				return valueExpression;
			}
		}
	}
}
