using System;
using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeObjectCreationExpression: GrapeExpression {
		private readonly GrapeType className;
		private readonly ReadOnlyCollection<GrapeExpression> parameters;

		[Rule("<Value> ::= ~new <Simple Type> ~'(' <Arg List Opt> ~')'")]
		public GrapeObjectCreationExpression(GrapeType className, GrapeList<GrapeExpression> parameters) {
			this.className = className;
			this.parameters = parameters.ToList(this).AsReadOnly();
		}

		public GrapeType ClassName {
			get {
				return className;
			}
		}

		public ReadOnlyCollection<GrapeExpression> Parameters {
			get {
				return parameters;
			}
		}
	}
}
