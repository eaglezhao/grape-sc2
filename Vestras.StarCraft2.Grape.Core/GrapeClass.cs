using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapeClass : GrapeEntityWithBlock {
        public static readonly int UseDefaultSize = -1;

        public bool IsNativeType { get; internal set; }
        public int Size { get; internal set; }
        public string Name { get; internal set; }
        public string Modifiers { get; internal set; }
        public GrapeExpression Inherits { get; internal set; }

        public override string ToString() {
            return GetType().Name + " Name = " + Name;
        }

        public GrapeClass() {
            Modifiers = "public";
            Size = UseDefaultSize;
        }
    }
}
