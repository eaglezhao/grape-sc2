using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	internal class GrapeMethodCall: GrapeMember {
		private readonly GrapeList<GrapeExpression> parameters;

		[Rule("<Method Call> ::= <Field Access> ~'(' <Arg List Opt> ~')'")]
		public GrapeMethodCall(GrapeMember ofMember, GrapeList<GrapeExpression> parameters): base(ofMember, null) {
			this.parameters = parameters;
		}

		protected override GrapeAccessExpression ToExpression(GrapeIdentifier identifier, GrapeAccessExpression next) {
			return GrapeCallExpression.Create(identifier, parameters, next);
		}
	}
}
