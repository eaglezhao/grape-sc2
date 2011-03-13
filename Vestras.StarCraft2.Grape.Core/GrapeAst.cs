using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core {
	public sealed class GrapeAst {
		private readonly List<GrapeEntity> children;

		public GrapeAst() {
			children = new List<GrapeEntity>();
		}

		public List<GrapeEntity> Children {
			get {
				return children;
			}
		}

		public override string ToString() {
			return "Children.Count = "+Children.Count;
		}
	}
}
