using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core {
    [Terminal("(Whitespace)")]
    [Terminal("NewLine")]
    internal class GrapeWhitespaceToken : GrapeEntity {
        [Rule("<NL> ::= ~NewLine ~<NL>")]
        [Rule("<NL Opt> ::=")]
        public GrapeWhitespaceToken() { }
    }
}