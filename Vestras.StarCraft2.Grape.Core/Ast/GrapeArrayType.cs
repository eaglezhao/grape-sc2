using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeArrayType: GrapeType {
		private readonly GrapeType elementType;

        public override GrapeExpression ToExpression() {
            // TODO: implement this. Return GrapeAccessExpression of some sort?
            throw new System.NotImplementedException();
        }

		[Rule("<Array Type> ::= <Qualified ID> ~'[' ~']'")]
		public GrapeArrayType(GrapeList<GrapeIdentifier> nameParts): this(new GrapeSimpleType(nameParts)) {}

		[Rule("<Array Type> ::= <Array Type> ~'[' ~']'")]
		public GrapeArrayType(GrapeType elementType): base() {
			this.elementType = elementType;
		}

		public GrapeType ElementType {
			get {
				return elementType;
			}
		}

		public override bool IsArray {
			get {
				return true;
			}
		}

		public override string ToString() {
			return elementType+"[]";
		}
	}
}
