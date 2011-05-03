using System;
using System.IO;

using bsn.GoldParser.Grammar;
using bsn.GoldParser.Parser;
using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
    internal class GrapeTokenizer : Tokenizer<GrapeEntity> {
        private readonly SemanticActions<GrapeEntity> actions;
        private readonly GrapeParserConfiguration configuration;
        private readonly Symbol indentDecSymbol;
        private readonly Symbol indentIncSymbol;
        private bool emittedVirtualNL = false;
        private int indent = 1;
        private int lastLine;
        private GrapeEntity pending;

        public GrapeTokenizer(TextReader textReader, GrapeParserConfiguration configuration, SemanticActions<GrapeEntity> actions)
            : base(textReader, actions.Grammar) {
            this.configuration = configuration;
            this.actions = actions;
            indentIncSymbol = Grammar.GetSymbolByName("IndentInc");
            indentDecSymbol = Grammar.GetSymbolByName("IndentDec");
        }

        public override ParseMessage NextToken(out GrapeEntity token) {
            if (pending == null) {
                ParseMessage message = base.NextToken(out token);
                if ((message != ParseMessage.TokenRead) || (token is GrapeWhitespaceToken) || (((IToken)token).Position.Line == lastLine)) {
                    return message;
                }
                pending = token;
                lastLine = ((IToken)token).Position.Line;
            }
            switch (((IToken)pending).Symbol.Kind) {
                case SymbolKind.Terminal:
                case SymbolKind.End:
                    switch (Math.Sign(((IToken)pending).Position.Column - indent)) {
                        case 1:
                            token = CreateVirtualToken(indentIncSymbol, indent++);
                            return ParseMessage.TokenRead;
                        case -1:
                            indent--;
                            token = CreateVirtualToken(indentDecSymbol, ((IToken)pending).Position.Column);
                            return ParseMessage.TokenRead;
                    }
                    break;
            }
            token = pending;
            pending = null;
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
            if ((tokenSymbol.Kind == SymbolKind.End) && (startPosition.Column > 1)) {
                if (!emittedVirtualNL) {
                    tokenSymbol = Grammar.GetSymbolByName("NewLine"); // simulate one newline before EOF
                    emittedVirtualNL = true;
                } else {
                    startPosition = new LineInfo(startPosition.Index, startPosition.Line + 1, 1);
                }
            }
            // Debug.WriteLine(startPosition.ToString(), string.Format("{0} [{1}]", tokenSymbol.Name, tokenSymbol.Kind));
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
