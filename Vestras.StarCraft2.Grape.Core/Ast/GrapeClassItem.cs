using System;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeClassItem: GrapeDeclaration {
		private readonly GrapeModifier.GrapeModifierType modifiers;

		protected GrapeClassItem(GrapeList<GrapeModifier> modifiers) {
			this.modifiers = modifiers.Merge(GrapeModifier.GrapeModifierType.Public);
		}

		public GrapeModifier.GrapeModifierType Modifiers {
			get {
				return modifiers;
			}
		}
	}
}
