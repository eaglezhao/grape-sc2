using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeTypecastExpression: GrapeExpression {
		private readonly GrapeType typeName;
		private readonly GrapeExpression value;

		[Rule("<Typecast Exp> ::= <Typecast Exp> ~as <Type>")]
		public GrapeTypecastExpression(GrapeExpression value, GrapeType typeName) {
			this.value = value;
			this.typeName = typeName;
		}

		public GrapeType TypeName {
			get {
				return typeName;
			}
		}

		public GrapeExpression Value {
			get {
				return value;
			}
		}
	}
}
