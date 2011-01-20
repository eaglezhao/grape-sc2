using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapePackageDeclaration : GrapeEntity {
        public List<GrapeEntity> Children { get; internal set; }
        public string PackageName { get; internal set; }

        public override string ToString() {
            return GetType().Name + " PackageName = " + PackageName;
        }

        public GrapePackageDeclaration() {
            Children = new List<GrapeEntity>();
        }
    }
}
