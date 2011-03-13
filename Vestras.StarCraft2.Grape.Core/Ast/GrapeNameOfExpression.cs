using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeNameofExpression: GrapeExpression {
		private readonly GrapeType value;

		[Rule("<Value> ::= ~nameof ~'(' <Simple Type> ~')'")]
		public GrapeNameofExpression(GrapeType value) {
			this.value = value;
		}

		public GrapeType Value {
			get {
				return value;
			}
		}
	}
}
