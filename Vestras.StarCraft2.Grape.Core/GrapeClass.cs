using System;
using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	public sealed class GrapeClass: GrapeClassItem {
        public static readonly int DefaultSize = 100;

		private readonly GrapeSimpleType inherits;
		private readonly ReadOnlyCollection<GrapeClassItem> classItems;
		private readonly string name;
		private readonly int? size;

		[Rule("<Class Decl> ::= <Modifier List Opt> ~class Identifier <Class Size Opt> <Class Base Opt> ~':' <Class Item Block>")]
		public GrapeClass(GrapeList<GrapeModifier> modifiers, GrapeIdentifier identifier, GrapeOptional<GrapeLiteralExpression<int>> size, GrapeOptional<GrapeSimpleType> inherits, GrapeList<GrapeClassItem> classItems): base(modifiers) {
			GrapeLiteralExpression<int> sizeLiteral = size;
			this.size = (sizeLiteral == null) ? default(int?) : sizeLiteral.Value;
			this.inherits = inherits;
			this.classItems = classItems.ToList(this).AsReadOnly();
			name = identifier.Name;
		}

		public GrapeSimpleType Inherits {
			get {
				return inherits;
			}
		}

		public bool IsNativeType {
			get;
			internal set;
		}

		public string Name {
			get {
				return name;
			}
		}

		public int? Size {
			get {
				return size;
			}
		}

		public ReadOnlyCollection<GrapeClassItem> ClassItems {
			get {
				return classItems;
			}
		}

		public override string ToString() {
			return GetType().Name+" Name = "+Name;
		}
	}
}
