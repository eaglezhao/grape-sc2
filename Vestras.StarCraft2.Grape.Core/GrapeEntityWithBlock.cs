using System;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core {
    public class GrapeEntityWithBlock : GrapeEntity {
        public GrapeBlock Block { get; internal set; }
    }
}
