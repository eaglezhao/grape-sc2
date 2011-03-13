﻿using System;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core {
	[Terminal("(Whitespace)")]
	[Terminal("(EOF)")]
	[Terminal("-")]
	[Terminal("!")]
	[Terminal("!=")]
	[Terminal("%")]
	[Terminal("&")]
	[Terminal("&&")]
	[Terminal("(")]
	[Terminal(")")]
	[Terminal("*")]
	[Terminal(",")]
	[Terminal(".")]
	[Terminal("/")]
	[Terminal(":")]
	[Terminal("[")]
	[Terminal("]")]
	[Terminal("^")]
	[Terminal("|")]
	[Terminal("||")]
	[Terminal("~")]
	[Terminal("+")]
	[Terminal("<")]
	[Terminal("<<")]
	[Terminal("<=")]
	[Terminal("=")]
	[Terminal("==")]
	[Terminal(">")]
	[Terminal(">=")]
	[Terminal(">>")]

	[Terminal("as")]
	[Terminal("break")]
	[Terminal("case")]
	[Terminal("catch")]
	[Terminal("class")]
	[Terminal("continue")]
	[Terminal("ctor")]
	[Terminal("dctor")]
	[Terminal("default")]
	[Terminal("delete")]
	[Terminal("else")]
	[Terminal("elseif")]
	[Terminal("finally")]
	[Terminal("foreach")]
	[Terminal("if")]
	[Terminal("import")]
	[Terminal("in")]
	[Terminal("IndentDec")]
	[Terminal("IndentInc")]
	[Terminal("inherits")]
	[Terminal("init")]
	[Terminal("nameof")]
	[Terminal("new")]
	[Terminal("NewLine")]
	[Terminal("package")]
	[Terminal("return")]
	[Terminal("switch")]
	[Terminal("throw")]
	[Terminal("try")]
	[Terminal("void")]
	[Terminal("while")]
	public class GrapeParsingEntity: GrapeEntity {
		[Rule("<NL> ::= ~NewLine ~<NL>")]
		[Rule("<NL Opt> ::=")]
		public GrapeParsingEntity() {}
	}
}
