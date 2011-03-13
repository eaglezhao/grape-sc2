using System;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core;

[assembly: RuleTrim("<Value> ::= '(' <Expression> ')'", "<Expression>", SemanticTokenType = typeof(GrapeEntity))]

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeExpression: GrapeEntity {}
}
