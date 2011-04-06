using System;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeModifierExtensionMethods {
        public static bool Contains(this GrapeModifier.GrapeModifierType modifiers, GrapeModifier.GrapeModifierType value) {
            return (((uint)modifiers & (uint)value) != 0);
        }

        public static bool HasInvalidAccessModifiers(this GrapeModifier.GrapeModifierType modifiers) {
            return false; // TODO: implement this
        }
    }
}
