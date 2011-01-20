using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapeAst {
        public List<GrapeEntity> Children { get; internal set; }

        public override string ToString() {
            return "Children.Count = " + Children.Count;
        }

        public GrapeAst() {
            Children = new List<GrapeEntity>();
        }
    }
}
