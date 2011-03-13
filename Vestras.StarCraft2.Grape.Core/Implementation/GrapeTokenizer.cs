using System;
using System.IO;

using bsn.GoldParser.Grammar;
using bsn.GoldParser.Parser;
using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	internal class GrapeTokenizer: Tokenizer<GrapeEntity> {
		private readonly SemanticActions<GrapeEntity> actions;
		private readonly GrapeParserConfiguration configuration;
		private readonly Symbol indentDecSymbol;
		private readonly Symbol indentIncSymbol;
		private int indent = 1;
		private int lastLine;
		private GrapeEntity pending;

		public GrapeTokenizer(TextReader textReader, GrapeParserConfiguration configuration, SemanticActions<GrapeEntity> actions): base(textReader, actions.Grammar) {
			this.configuration = configuration;
			this.actions = actions;
			indentIncSymbol = Grammar.GetSymbolByName("IndentInc");
			indentDecSymbol = Grammar.GetSymbolByName("IndentDec");
		}

		public override ParseMessage NextToken(out GrapeEntity token) {
			if (pending == null) {
				ParseMessage message = base.NextToken(out token);
				if ((message != ParseMessage.TokenRead) || (((IToken)token).Symbol.Kind == SymbolKind.WhiteSpace) || (((IToken)token).Position.Line == lastLine)) {
					return message;
				}
				pending = token;
				lastLine = ((IToken)token).Position.Line;
			}
			switch (Math.Sign(((IToken)pending).Position.Column-indent)) {
			case 1:
				token = CreateVirtualToken(indentIncSymbol, indent++);
				break;
			case -1:
				indent--;
				token = CreateVirtualToken(indentDecSymbol, ((IToken)pending).Position.Column);
				break;
			default:
				token = pending;
				pending = null;
				break;
			}
			return ParseMessage.TokenRead;
		}

		protected override GrapeEntity CreateToken(Symbol tokenSymbol, LineInfo tokenPosition, string text) {
			return CreateTokenInternal(tokenSymbol, tokenPosition, new LineInfo(InputIndex, LineNumber, LineColumn), text);
		}

		private GrapeEntity CreateTokenInternal(Symbol tokenSymbol, LineInfo startPosition, LineInfo endPosition, string text) {
			SemanticTerminalFactory<GrapeEntity> factory;
			if (!actions.TryGetTerminalFactory(tokenSymbol, out factory)) {
				throw new InvalidOperationException(string.Format("No factory found for terminal symbol {0}", tokenSymbol.Name));
			}
			GrapeEntity entity = factory.CreateAndInitialize(tokenSymbol, startPosition, text);
			entity.InitalizeFromPosition(configuration.FileName, endPosition);
			return entity;
		}

		private GrapeEntity CreateVirtualToken(Symbol symbol, int column) {
			LineInfo tokenPosition = new LineInfo(((IToken)pending).Position.Index, ((IToken)pending).Position.Line, column);
			return CreateTokenInternal(symbol, tokenPosition, tokenPosition, string.Empty);
		}
	}
}
