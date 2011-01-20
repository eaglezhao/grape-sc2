﻿using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeFinallyStatement : GrapeStatement {
        public GrapeTryStatement TryStatement { get; internal set; }
        public override bool CanHaveBlock {
            get { return true; }
        }
    }
}
