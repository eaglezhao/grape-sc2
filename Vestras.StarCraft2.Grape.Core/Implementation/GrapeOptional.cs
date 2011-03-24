using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	public class GrapeOptional<T>: GrapeEntity where T: GrapeEntity {
		public static implicit operator T(GrapeOptional<T> value) {
			if (value != null) {
				return value.item;
			}
			return null;
		}

		private readonly T item;

		[Rule("<Class Size Opt> ::=", typeof(GrapeLiteralExpression<int>))]
		[Rule("<Class Base Opt> ::=", typeof(GrapeSimpleType))]
		public GrapeOptional(): this(null) {}

		[Rule("<Class Size Opt> ::= ~'[' DecimalLiteral ~']'", typeof(GrapeLiteralExpression<int>))]
		[Rule("<Class Base Opt> ::= ~inherits <Simple Type>", typeof(GrapeSimpleType))]
		public GrapeOptional(T item) {
			this.item = item;
		}
	}
}
