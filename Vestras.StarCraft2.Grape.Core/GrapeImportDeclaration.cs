using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	public sealed class GrapeImportDeclaration: GrapeDeclaration {
		private readonly string packageName;

		[Rule("<Import> ::= ~import <Qualified ID> ~<NL>")]
		public GrapeImportDeclaration(GrapeList<GrapeIdentifier> packageNameParts) {
			packageName = packageNameParts.GetFullName();
		}

		public string PackageName {
			get {
				return packageName;
			}
		}

		public override string ToString() {
			return GetType().Name+" ImportedPackage = "+PackageName;
		}
	}
}
