using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeReturnStatement: GrapeStatement {
		private readonly GrapeExpression returnValue;

		[Rule("<Statement> ::= ~return ~<NL>")]
		public GrapeReturnStatement(): this(null) {}

		[Rule("<Statement> ::= ~return <Expression> ~<NL>")]
		public GrapeReturnStatement(GrapeExpression returnValue) {
			this.returnValue = returnValue;
		}

		public GrapeExpression ReturnValue {
			get {
				return returnValue;
			}
		}
	}
}
