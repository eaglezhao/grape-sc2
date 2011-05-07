using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapePassStatement : GrapeStatement {
        [Rule("<Statement> ::= ~pass ~<NL>")]
        public GrapePassStatement() {
        }
    }
}
