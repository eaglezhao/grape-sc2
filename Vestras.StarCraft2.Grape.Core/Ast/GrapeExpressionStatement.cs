using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeExpressionStatement: GrapeStatement {
		private readonly GrapeExpression expression;

		[Rule("<Statement> ::= <Assignment Expression> ~<NL>")]
		public GrapeExpressionStatement(GrapeExpression expression) {
			this.expression = expression;
		}

		[Rule("<Statement> ::= <Method Call> ~<NL>")]
		public GrapeExpressionStatement(GrapeMember memberCall): this(memberCall.ToExpression()) {}

		public GrapeExpression Expression {
			get {
				return expression;
			}
		}
	}
}
