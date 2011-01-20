using System;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core {
    public class GrapeVariable : GrapeEntity {
        public bool IsParameter { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public GrapeExpression Value { get; internal set; }

        public override string ToString() {
            return GetType().Name + " Name = " + Name + ", Type = " + Type;
        }
    }
}
