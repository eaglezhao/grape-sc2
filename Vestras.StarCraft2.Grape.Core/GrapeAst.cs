using System;
using System.Collections.Generic;
using System.Linq;

namespace Vestras.StarCraft2.Grape.Core {
	public sealed class GrapeAst {
		private List<GrapeEntity> children;

		public GrapeAst() {
			children = new List<GrapeEntity>();
		}

        private readonly List<GrapeEntity> childrenRecursive = new List<GrapeEntity>();
        public List<GrapeEntity> GetChildrenRecursive() {
            if (childrenRecursive.Count == 0) {
                childrenRecursive.AddRange(children.SelectMany(e => e.GetChildren()));
            }
            return childrenRecursive;
        }

		public List<GrapeEntity> Children {
			get {
				return children;
			}
		}

        public static GrapeAst Merge(GrapeAst left, GrapeAst right) {
            List<GrapeEntity> mergedChildren = new List<GrapeEntity>();
            mergedChildren.AddRange(left.Children);
            mergedChildren.AddRange(right.Children);
            GrapeAst ast = new GrapeAst();
            ast.children = mergedChildren;
            return ast;
        }

		public override string ToString() {
			return "Children.Count = "+Children.Count;
		}
	}
}
