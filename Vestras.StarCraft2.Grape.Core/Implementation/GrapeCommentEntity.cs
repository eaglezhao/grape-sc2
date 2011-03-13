using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Terminal("(Comment)")]
	[Terminal("(Comment End)")]
	[Terminal("(Comment Line)")]
	[Terminal("(Comment Start)")]
	public class GrapeCommentEntity: GrapeEntity {
		private readonly string comment;

		public GrapeCommentEntity(string comment) {
			this.comment = comment;
		}

		public string Comment {
			get {
				return comment;
			}
		}
	}
}
