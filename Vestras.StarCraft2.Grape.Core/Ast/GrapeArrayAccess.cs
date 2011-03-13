using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeArrayAccess: GrapeMember {
		private readonly GrapeExpression indexExpression;

		[Rule("<Array Access> ::= <Qualified ID> ~'[' <Expression> ~']'")]
		public GrapeArrayAccess(GrapeList<GrapeIdentifier> identifiers, GrapeExpression indexExpression): this((GrapeMember)null, identifiers, indexExpression) {}

		[Rule("<Array Access> ::= <Array Access> ~'[' <Expression> ~']'")]
		[Rule("<Array Access> ::= <Method Call> ~'[' <Expression> ~']'")]
		public GrapeArrayAccess(GrapeMember ofMember, GrapeExpression indexExpression): this(ofMember, null, indexExpression) {}

		[Rule("<Array Access> ::= <Object> ~'.' <Qualified ID> ~'[' <Expression> ~']'")]
		public GrapeArrayAccess(GrapeObject grapeObject, GrapeList<GrapeIdentifier> identifiers, GrapeExpression indexExpression): this((GrapeMember)null, new GrapeList<GrapeIdentifier>(grapeObject, identifiers), indexExpression) {}

		[Rule("<Array Access> ::= <Array Access> ~'.' <Qualified ID> ~'[' <Expression> ~']'")]
		[Rule("<Array Access> ::= <Method Call> ~'.' <Qualified ID> ~'[' <Expression> ~']'")]
		public GrapeArrayAccess(GrapeMember ofMember, GrapeList<GrapeIdentifier> identifiers, GrapeExpression indexExpression): base(ofMember, identifiers) {
			this.indexExpression = indexExpression;
		}

		protected override GrapeAccessExpression GetAccessor(GrapeAccessExpression next) {
			return GrapeArrayExpression.Create(indexExpression, base.GetAccessor(next));
		}
	}
}
