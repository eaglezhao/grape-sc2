using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeStatement: GrapeEntity {
		public virtual bool CanHaveBlock {
			get {
				return false;
			}
		}
	}
}
