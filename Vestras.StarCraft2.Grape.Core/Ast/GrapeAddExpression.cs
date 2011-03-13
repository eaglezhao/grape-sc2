using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeAddExpression: GrapeExpression {
		public enum GrapeAddExpressionType {
			Addition,
			Subtraction
		}

		private readonly GrapeExpression left;
		private readonly GrapeExpression right;

		[Rule("<Add Exp> ::= <Add Exp> ~'+' <Mult Exp>")]
		public GrapeAddExpression(GrapeExpression left, GrapeExpression right) {
			this.left = left;
			this.right = right;
		}

		public GrapeExpression Left {
			get {
				return left;
			}
		}

		public GrapeExpression Right {
			get {
				return right;
			}
		}

		public virtual GrapeAddExpressionType Type {
			get {
				return GrapeAddExpressionType.Addition;
			}
		}
	}
}
