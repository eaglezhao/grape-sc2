using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeType: GrapeEntity {
		private readonly string packageName;
		private readonly string typeName;

		[Rule("<Simple Type> ::= <Qualified ID>")]
		public GrapeType(GrapeList<GrapeIdentifier> nameParts) {
			packageName = nameParts.GetPackageName();
			typeName = nameParts.GetSimpleName();
		}

		public virtual bool IsArray {
			get {
				return false;
			}
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
	}
}
