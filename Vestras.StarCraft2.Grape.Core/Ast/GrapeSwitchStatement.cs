using System;
using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeSwitchStatement: GrapeStatement {
		private readonly ReadOnlyCollection<GrapeSwitchCase> cases;
		private readonly GrapeExpression expression;

		[Rule("<Statement> ::= ~switch <Expression> ~':' <Switch Section Block>")]
		public GrapeSwitchStatement(GrapeExpression expression, GrapeList<GrapeSwitchCase> cases): base() {
			this.expression = expression;
			this.cases = cases.ToList(this).AsReadOnly();
		}

		public ReadOnlyCollection<GrapeSwitchCase> Cases {
			get {
				return cases;
			}
		}

		public GrapeExpression Expression {
			get {
				return expression;
			}
		}
	}
}
