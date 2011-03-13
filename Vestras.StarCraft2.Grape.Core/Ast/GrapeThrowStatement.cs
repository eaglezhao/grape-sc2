using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeThrowStatement: GrapeStatement {
		private readonly GrapeExpression throwExpression;

		[Rule("<Statement> ::= ~throw ~<NL>")]
		public GrapeThrowStatement(): this(null) {}

		[Rule("<Statement> ::= ~throw <Expression> ~<NL>")]
		public GrapeThrowStatement(GrapeExpression throwExpression) {
			this.throwExpression = throwExpression;
		}

		public GrapeExpression ThrowExpression {
			get {
				return throwExpression;
			}
		}
	}
}
