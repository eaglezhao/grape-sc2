using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public abstract class GrapeStatement : GrapeEntityWithBlock {
        public abstract bool CanHaveBlock { get; }
    }
}
