using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeBlock : GrapeEntity {
        public List<GrapeEntity> Children { get; internal set; }

        public GrapeBlock() {
            Children = new List<GrapeEntity>();
        }
    }
}
