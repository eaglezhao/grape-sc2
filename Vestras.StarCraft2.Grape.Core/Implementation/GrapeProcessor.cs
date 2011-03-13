using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using bsn.GoldParser.Grammar;
using bsn.GoldParser.Parser;
using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	internal class GrapeProcessor: SemanticProcessor<GrapeEntity> {
		private readonly List<GrapeEntity> allEntities = new List<GrapeEntity>();
		private readonly GrapeParserConfiguration configuration;

		public GrapeProcessor(TextReader reader, GrapeParserConfiguration configuration, SemanticActions<GrapeEntity> actions): base(new GrapeTokenizer(reader, configuration, actions), actions) {
			this.configuration = configuration;
		}

		public ICollection<GrapeEntity> AllEntities {
			get {
				return allEntities;
			}
		}

		public override ParseMessage Parse() {
			ParseMessage parseMessage = base.Parse();
			while (parseMessage == ParseMessage.LexicalError) {
				configuration.AddError(CurrentToken);
				if (!configuration.ContinueOnError) {
					break;
				}
				parseMessage = base.Parse();
			}
			if (CurrentToken != null) {
				switch (((IToken)CurrentToken).Symbol.Kind) {
				case SymbolKind.Nonterminal:
				case SymbolKind.Terminal:
					allEntities.Add(CurrentToken);
					break;
				}
			}
			return parseMessage;
		}

		protected override GrapeEntity CreateReduction(Rule rule, IList<GrapeEntity> children) {
			GrapeEntity entity = base.CreateReduction(rule, children);
			if (entity.Offset == 0) {
				GrapeEntity nonZeroOffset = children.FirstOrDefault(c => c.Offset > 0);
				if (nonZeroOffset != null) {
					entity.SetStartPosition(((IToken)nonZeroOffset).Position);
				}
			}
			entity.InitializeFromChildren(configuration.FileName, children);
			return entity;
		}

		protected override bool RetrySyntaxError(ref GrapeEntity currentToken) {
			configuration.AddError(currentToken);
			if (configuration.ContinueOnError && (((IToken)currentToken).Symbol.Kind != SymbolKind.End)) {
				currentToken = null;
				return true;
			}
			return false;
		}
	}
}
