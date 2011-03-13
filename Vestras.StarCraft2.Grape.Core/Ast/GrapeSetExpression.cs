using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeSetExpression: GrapeExpression {
		private readonly GrapeAccessExpression memberAccess;
		private readonly GrapeExpression value;

		[Rule("<Assignment Expression> ::= <Member Access> ~'=' <Expression>")]
		public GrapeSetExpression(GrapeAccessExpression memberAccess, GrapeExpression value) {
			this.memberAccess = memberAccess;
			this.value = value;
		}

		public GrapeAccessExpression MemberAccess {
			get {
				return memberAccess;
			}
		}

		public GrapeExpression Value {
			get {
				return value;
			}
		}
	}
}
