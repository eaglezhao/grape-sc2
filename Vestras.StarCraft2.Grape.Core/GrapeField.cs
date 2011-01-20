using System;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapeField : GrapeVariable {
        public string Modifiers { get; internal set; }

        public GrapeField() {
            Modifiers = "public";
        }
    }
}
