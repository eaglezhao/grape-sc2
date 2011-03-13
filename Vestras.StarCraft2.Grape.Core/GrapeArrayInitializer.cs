using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	public class GrapeArrayInitializer: GrapeInitializer {
		private readonly ReadOnlyCollection<GrapeInitializer> initializers;

		[Rule("<Array Initializer> ::= ~'[' ~']'")]
		public GrapeArrayInitializer(): this(null) {}

		[Rule("<Array Initializer> ::= ~'[' <Variable Initializer List> ~']'")]
		public GrapeArrayInitializer(GrapeList<GrapeInitializer> initializers) {
			this.initializers = initializers.ToList(this).AsReadOnly();
		}
	}
}