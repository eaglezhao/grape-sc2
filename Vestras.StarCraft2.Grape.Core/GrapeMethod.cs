using System;
using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core {
	public class GrapeMethod: GrapeClassItem {
		public enum GrapeMethodType {
			Function,
			Procedure,
			Constructor,
			Destructor
		}

		private readonly ReadOnlyCollection<GrapeStatement> body;

		private readonly string name;
		private readonly ReadOnlyCollection<GrapeParameter> parameters;
		private readonly GrapeType returnType;

		[Rule("<Method Dec> ::= <Modifier List Opt> ~void Identifier ~'(' <Formal Param List Opt> ~')' ~':' <Stm Block>")]
		public GrapeMethod(GrapeList<GrapeModifier> modifiers, GrapeIdentifier name, GrapeList<GrapeParameter> parameters, GrapeList<GrapeStatement> body): this(modifiers, null, name, parameters, body) {}

		[Rule("<Method Dec> ::= <Modifier List Opt> <Type> Identifier ~'(' <Formal Param List Opt> ~')' ~':' <Stm Block>")]
		public GrapeMethod(GrapeList<GrapeModifier> modifiers, GrapeType returnType, GrapeIdentifier name, GrapeList<GrapeParameter> parameters, GrapeList<GrapeStatement> body): base(modifiers) {
			this.returnType = returnType;
			this.name = name.Name;
			this.parameters = parameters.ToList(this).AsReadOnly();
			this.body = body.ToList(this).AsReadOnly();
		}

		public ReadOnlyCollection<GrapeStatement> Body {
			get {
				return body;
			}
		}

		public string Name {
			get {
				return name;
			}
		}

		public ReadOnlyCollection<GrapeParameter> Parameters {
			get {
				return parameters;
			}
		}

		public GrapeType ReturnType {
			get {
				return Type == GrapeMethodType.Procedure ? new GrapeSimpleType(new GrapeList<GrapeIdentifier>(new GrapeIdentifier("void_base"))) : returnType; // left for compat
			}
		}

		public virtual GrapeMethodType Type {
			get {
				return returnType == null ? GrapeMethodType.Procedure : GrapeMethodType.Function;
			}
		}

		public override string ToString() {
			return GetType().Name+" Name = "+Name+", ReturnType = "+ReturnType;
		}
	}
}
