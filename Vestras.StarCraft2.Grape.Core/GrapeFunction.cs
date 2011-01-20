using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapeFunction : GrapeEntityWithBlock {
        public GrapeFunctionType Type { get; internal set; }
        public string Modifiers { get; internal set; }
        public string Name { get; internal set; }
        public string ReturnType { get; internal set; }
        public List<GrapeVariable> Parameters { get; internal set; }
        public List<GrapeFunction> Overloads { get; internal set; }

        public override string ToString() {
            return GetType().Name + " Name = " + Name + ", ReturnType = " + ReturnType;
        }

        public GrapeFunction() {
            Modifiers = "public";
            Type = GrapeFunctionType.Function;
            Parameters = new List<GrapeVariable>();
            Overloads = new List<GrapeFunction>();
        }

        public enum GrapeFunctionType {
            Function,
            Constructor,
            Destructor
        }
    }
}
