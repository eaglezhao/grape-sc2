using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core {
	public class GrapeValueInitializer: GrapeInitializer {
		private readonly GrapeExpression value;

		[Rule("<Variable Initializer> ::= <Expression>")]
		public GrapeValueInitializer(GrapeExpression value) {
			this.value = value;
		}

		public GrapeExpression Value {
			get {
				return value;
			}
		}
	}
}