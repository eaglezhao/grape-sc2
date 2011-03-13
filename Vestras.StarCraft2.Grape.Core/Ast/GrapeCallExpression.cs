using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public class GrapeCallExpression: GrapeMemberExpression {
		internal static GrapeCallExpression Create(GrapeIdentifier identifier, GrapeList<GrapeExpression> parameters, GrapeAccessExpression next) {
			List<GrapeEntity> children = new List<GrapeEntity>();
			children.Add(identifier);
			children.AddRange(parameters.Enumerate());
			GrapeCallExpression result = new GrapeCallExpression(identifier, parameters, next);
			result.InitializeFromTemplate(identifier);
			result.InitializeFromChildren(identifier.FileName, children);
			return result;
		}

		private readonly ReadOnlyCollection<GrapeExpression> parameters;

		private GrapeCallExpression(GrapeIdentifier identifier, GrapeList<GrapeExpression> parameters, GrapeAccessExpression next): base(GrapeAccessExpressionType.Method, identifier, next) {
			this.parameters = parameters.ToList(this).AsReadOnly();
		}

		public ReadOnlyCollection<GrapeExpression> Parameters {
			get {
				return parameters;
			}
		}
	}
}
