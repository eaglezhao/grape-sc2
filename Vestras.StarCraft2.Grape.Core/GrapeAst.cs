using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapeAst {
        public List<GrapeEntity> Children { get; internal set; }

        public override string ToString() {
            return "Children.Count = " + Children.Count;
        }

        public static GrapeAst Merge(GrapeAst left, GrapeAst right) {
            List<GrapeEntity> mergedChildren = new List<GrapeEntity>();
            mergedChildren.AddRange(left.Children);
            mergedChildren.AddRange(right.Children);
            return new GrapeAst { Children = mergedChildren };
        }

        public GrapeAst() {
            Children = new List<GrapeEntity>();
        }
    }
}
