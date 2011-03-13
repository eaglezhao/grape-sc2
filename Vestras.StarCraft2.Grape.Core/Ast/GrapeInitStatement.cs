using System;
using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeInitStatement: GrapeStatement {
		public enum GrapeInitStatementType {
			This,
			Base,
			Unknown
		}

		private readonly ReadOnlyCollection<GrapeExpression> parameters;
		private readonly GrapeInitStatementType type;

		[Rule("<Statement> ::= ~init <Object> ~'(' <Arg List Opt> ~')' ~<NL>")]
		public GrapeInitStatement(GrapeObject objectIdentifier, GrapeList<GrapeExpression> parameters) {
			type = (GrapeInitStatementType)Enum.Parse(typeof(GrapeInitStatementType), objectIdentifier.Name, true);
			this.parameters = parameters.ToList(this).AsReadOnly();
		}

		public ReadOnlyCollection<GrapeExpression> Parameters {
			get {
				return parameters;
			}
		}

		public GrapeInitStatementType Type {
			get {
				return type;
			}
		}
	}
}
