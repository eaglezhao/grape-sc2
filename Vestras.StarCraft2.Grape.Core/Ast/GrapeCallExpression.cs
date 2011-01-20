using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeCallExpression : GrapeMemberExpression {
        public List<GrapeExpression> Parameters { get; internal set; }

        public GrapeCallExpression() {
            Parameters = new List<GrapeExpression>();
        }
    }
}
