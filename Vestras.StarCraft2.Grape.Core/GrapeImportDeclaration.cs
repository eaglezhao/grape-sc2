using System;

namespace Vestras.StarCraft2.Grape.Core {
    public sealed class GrapeImportDeclaration : GrapeEntity {
        public string ImportedPackage { get; internal set; }

        public override string ToString() {
            return GetType().Name + " ImportedPackage = " + ImportedPackage;
        }
    }
}
