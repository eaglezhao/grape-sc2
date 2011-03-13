using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeDeleteStatement: GrapeStatement {
		private readonly GrapeExpression value;

		[Rule("<Statement> ::= ~delete <Expression> ~<NL>")]
		public GrapeDeleteStatement(GrapeExpression value) {
			this.value = value;
		}

		public GrapeExpression Value {
			get {
				return value;
			}
		}
	}
}
