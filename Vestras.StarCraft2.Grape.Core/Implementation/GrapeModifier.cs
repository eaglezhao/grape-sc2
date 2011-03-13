using System;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	public abstract class GrapeModifier: GrapeEntity {
		[Flags]
		public enum GrapeModifierType {
			Default = 0,
			Private = 1,
			Protected = 2,
			Internal = 4,
			Public = 8,
			Access = 15,
			Override = 16,
			Sealed = 32,
			Static = 64,
			Abstract = 128
		}

		public abstract GrapeModifierType Type {
			get;
		}
	}
}