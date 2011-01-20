using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeNonArrayCreationExpression : GrapeExpression {
        public GrapeExpression Type { get; internal set; }
        public List<GrapeExpression> Parameters { get; internal set; }

        public GrapeNonArrayCreationExpression() {
            Parameters = new List<GrapeExpression>();
        }
    }
}
