using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeNameofExpression: GrapeExpression {
		private readonly GrapeSimpleType value;

		[Rule("<Value> ::= ~nameof ~'(' <Simple Type> ~')'")]
		public GrapeNameofExpression(GrapeSimpleType value) {
			this.value = value;
		}

		public GrapeSimpleType Value {
			get {
				return value;
			}
		}
	}
}
