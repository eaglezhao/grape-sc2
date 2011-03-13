using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeVariableStatement: GrapeStatement {
		private readonly GrapeVariable variable;

		[Rule("<Statement> ::= <Variable Declarator> ~<NL>")]
		public GrapeVariableStatement(GrapeVariable variable) {
			this.variable = variable;
		}

		public GrapeVariable Variable {
			get {
				return variable;
			}
		}
	}
}
