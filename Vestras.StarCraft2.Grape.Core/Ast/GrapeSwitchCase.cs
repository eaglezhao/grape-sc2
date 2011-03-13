using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeSwitchCase: GrapeStatementWithBlock {
		private readonly GrapeExpression caseValue;

		[Rule("<Switch Default> ::= ~default ~':' <Stm Block>")]
		public GrapeSwitchCase(GrapeList<GrapeStatement> children): this(null, children) {}

		[Rule("<Switch Label> ::= ~case <Expression> ~':' <Stm Block>")]
		public GrapeSwitchCase(GrapeExpression caseValue, GrapeList<GrapeStatement> children): base(children) {
			this.caseValue = caseValue;
		}

		public GrapeExpression CaseValue {
			get {
				return caseValue;
			}
		}

		public GrapeSwitchStatement SwitchStatement {
			get {
				return (GrapeSwitchStatement)Parent;
			}
		}
	}
}
