using System.Collections.ObjectModel;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeStatementWithBlock: GrapeStatement {
		private readonly ReadOnlyCollection<GrapeStatement> statements;

		protected GrapeStatementWithBlock(GrapeList<GrapeStatement> statements) {
			this.statements = statements.ToList(this).AsReadOnly();
		}

		public override bool CanHaveBlock {
			get {
				return true;
			}
		}

		public ReadOnlyCollection<GrapeStatement> Statements {
			get {
				return statements;
			}
		}
	}
}
