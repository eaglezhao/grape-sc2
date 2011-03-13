using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeMultiplicationExpression: GrapeBinaryExpression {
		public enum GrapeMultiplicationExpressionType {
			Multiplication,
			Division,
			Mod
		}

		[Rule("<Mult Exp> ::= <Mult Exp> ~'*' <Typecast Exp>")]
		public GrapeMultiplicationExpression(GrapeExpression left, GrapeExpression right): base(left, right) {}

		public virtual GrapeMultiplicationExpressionType Type {
			get {
				return GrapeMultiplicationExpressionType.Multiplication;
			}
		}
	}
}
