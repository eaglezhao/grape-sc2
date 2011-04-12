using System;
using bsn.GoldParser.Semantic;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeSimpleType : GrapeType {
        private readonly string packageName;
        private readonly string typeName;

        [Rule("<Simple Type> ::= <Qualified ID>")]
        public GrapeSimpleType(GrapeList<GrapeIdentifier> nameParts) {
            packageName = nameParts.GetPackageName();
            typeName = nameParts.GetSimpleName();
        }

        public string PackageName {
            get {
                return packageName;
            }
        }

        public string TypeName {
            get {
                return typeName;
            }
        }

        public override string ToString() {
            if (string.IsNullOrEmpty(packageName)) {
                return typeName;
            }

            return packageName + '.' + typeName;
        }
    }
}
