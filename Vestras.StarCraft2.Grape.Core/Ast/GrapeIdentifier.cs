using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("Identifier")]
	public class GrapeIdentifier: GrapeEntity {
		private readonly string name;

		public GrapeIdentifier(string name) {
			this.name = name;
		}

		public virtual bool IsObject {
			get {
				return false;
			}
		}

		public string Name {
			get {
				return name;
			}
		}

		public override string ToString() {
			return name;
		}
	}
}
