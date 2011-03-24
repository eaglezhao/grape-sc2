using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core {
	public class GrapeVariable: GrapeEntity {
		internal static GrapeVariable Create(GrapeSimpleType type, GrapeIdentifier name) {
			GrapeVariable variable = new GrapeVariable(type, name);
			variable.InitializeFromTemplate(type);
			variable.InitializeFromChildren(type.FileName, new GrapeEntity[] {type, name});
			return variable;
		}

		private readonly GrapeInitializer initializer;
		private readonly string name;
		private readonly GrapeType type;

		[Rule("<Variable Declarator Uninitialized> ::= <Type> Identifier")]
		public GrapeVariable(GrapeType type, GrapeIdentifier name): this(type, name, null) {}

		[Rule("<Variable Declarator Initialized> ::= <Type> Identifier ~'=' <Variable Initializer>")]
		public GrapeVariable(GrapeType type, GrapeIdentifier name, GrapeInitializer initializer) {
			this.name = name.Name;
			this.type = type;
			this.initializer = initializer;
		}

		public GrapeInitializer Initializer {
			get {
				return initializer;
			}
		}

		public virtual bool IsParameter {
			get {
				return false;
			}
		}

		public string Name {
			get {
				return name;
			}
		}

		public GrapeType Type {
			get {
				return type;
			}
		}

		public override string ToString() {
			return GetType().Name+" Name = "+Name+", Type = "+Type;
		}
	}
}
