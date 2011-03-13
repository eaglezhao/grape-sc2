using System;
using System.Collections.Generic;
using System.Diagnostics;

using bsn.GoldParser.Parser;
using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core {
	public abstract class GrapeEntity: SemanticToken {
		protected GrapeEntity() {
			FileName = string.Empty;
		}

		public int EndColumn {
			get;
			private set;
		}

		public int EndLine {
			get;
			private set;
		}

		public string FileName {
			get;
			private set;
		}

		public int Length {
			get;
			private set;
		}

		public int Offset {
			get {
				return (int)((IToken)this).Position.Index;
			}
		}

		public GrapeEntity Parent {
			get;
			internal set;
		}

		public int StartColumn {
			get {
				return ((IToken)this).Position.Column;
			}
		}

		public int StartLine {
			get {
				return ((IToken)this).Position.Line;
			}
		}

		internal void InitalizeFromPosition(string fileName, LineInfo endPosition) {
			FileName = fileName;
			EndColumn = endPosition.Column;
			EndLine = endPosition.Line;
			Length = (int)(endPosition.Index-((IToken)this).Position.Index);
			Debug.Assert(Length >= 0);
		}

		internal void InitializeFromChildren(string fileName, IEnumerable<GrapeEntity> children) {
			FileName = fileName;
			GrapeEntity lastChild = null;
			if (children != null) {
				foreach (GrapeEntity child in children) {
					child.Parent = this;
					lastChild = child;
				}
			}
			if (lastChild != null) {
				EndColumn = lastChild.EndColumn;
				EndLine = lastChild.EndLine;
				Length = (lastChild.Offset+lastChild.Length)-Offset;
			} else if (Length == 0) {
				EndColumn = StartColumn;
				EndLine = StartLine;
			}
		}

		internal void InitializeFromTemplate(GrapeEntity template) {
			if (template == null) {
				throw new ArgumentNullException("template");
			}
			Initialize(((IToken)template).Symbol, ((IToken)template).Position);
			Parent = template.Parent;
			Length = template.Length;
			EndColumn = template.EndColumn;
			EndLine = template.EndLine;
			FileName = template.FileName;
		}

		internal void SetStartPosition(LineInfo lineInfo) {
			Initialize(((IToken)this).Symbol, lineInfo);
		}
	}
}
