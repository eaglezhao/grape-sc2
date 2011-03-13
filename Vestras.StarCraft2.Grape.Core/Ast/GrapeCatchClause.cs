using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeCatchClause: GrapeStatementWithBlock {
		private readonly GrapeType exceptionType;
		private readonly GrapeVariable exceptionVariable;

		[Rule("<Catch Clause> ::= ~catch ~':' <Stm Block>")]
		public GrapeCatchClause(GrapeList<GrapeStatement> statements): this(null, null, statements) {}

		[Rule("<Catch Clause> ::= ~catch <Simple Type> ~':' <Stm Block>")]
		public GrapeCatchClause(GrapeType exceptionType, GrapeList<GrapeStatement> statements): this(exceptionType, null, statements) {}

		[Rule("<Catch Clause> ::= ~catch <Simple Type> Identifier ~':' <Stm Block>")]
		public GrapeCatchClause(GrapeType exceptionType, GrapeIdentifier variableIdentifier, GrapeList<GrapeStatement> statements): base(statements) {
			this.exceptionType = exceptionType;
			exceptionVariable = GrapeVariable.Create(exceptionType, variableIdentifier);
		}

		public GrapeType ExceptionType {
			get {
				return exceptionType;
			}
		}

		public GrapeVariable ExceptionVariable {
			get {
				return exceptionVariable;
			}
		}

		public GrapeTryStatement TryStatement {
			get {
				return (GrapeTryStatement)Parent;
			}
		}
	}
}
