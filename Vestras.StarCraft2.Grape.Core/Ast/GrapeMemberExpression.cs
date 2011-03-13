using System;
using System.Diagnostics;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeMemberExpression: GrapeAccessExpression {
		internal static GrapeMemberExpression Create(GrapeAccessExpressionType type, GrapeIdentifier identifier, GrapeAccessExpression next) {
			if (identifier == null) {
				throw new ArgumentNullException("identifier");
			}
			Debug.Assert((type == GrapeAccessExpressionType.Root) || (type == GrapeAccessExpressionType.Field));
			GrapeMemberExpression result = new GrapeMemberExpression(type, identifier, next);
			result.InitializeFromTemplate(identifier);
			return result;
		}

		private readonly GrapeIdentifier identifier;
		private readonly GrapeAccessExpressionType type;

		[Rule("<Value> ::= <Object>")]
		public GrapeMemberExpression(GrapeObject identifier): this(GrapeAccessExpressionType.Root, identifier, null) {}

		protected GrapeMemberExpression(GrapeAccessExpressionType type, GrapeIdentifier identifier, GrapeAccessExpression next): base(next) {
			Debug.Assert(type != GrapeAccessExpressionType.Array);
			this.type = type;
			this.identifier = identifier;
		}

		public GrapeIdentifier Identifier {
			get {
				return identifier;
			}
		}

		public override sealed GrapeAccessExpressionType Type {
			get {
				return type;
			}
		}
	}
}
