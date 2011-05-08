using System.Collections.Generic;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

[assembly: RuleTrim("<Stm Block> ::= <NL> IndentInc <Stm List> IndentDec <NL Opt>", "<Stm List>", SemanticTokenType = typeof(GrapeEntity))]
[assembly: RuleTrim("<Switch Section Block> ::= <NL> IndentInc <Switch Section List> IndentDec <NL Opt>", "<Switch Section List>", SemanticTokenType = typeof(GrapeEntity))]
[assembly: RuleTrim("<Start> ::= <NL Opt> <Decl List>", "<Decl List>", SemanticTokenType = typeof(GrapeEntity))]
[assembly: RuleTrim("<Class Item Block> ::= <NL> IndentInc <Class Item List> IndentDec <NL Opt>", "<Class Item List>", SemanticTokenType = typeof(GrapeEntity))]

namespace Vestras.StarCraft2.Grape.Core.Implementation {
    public class GrapeList<T> : GrapeEntity where T : GrapeEntity {
        public static GrapeList<T> FromEnumerable(IEnumerable<T> enumerable) {
            GrapeList<T> list = new GrapeList<T>();
            IEnumerator<T> enumerator = enumerable.GetEnumerator();
            list.item = enumerator.Current;
            GrapeList<T> current = list;
            while (enumerator.MoveNext()) {
                current.next = new GrapeList<T>(enumerator.Current);
                current = current.next;
            }

            return list;
        }

        private T item;
        private GrapeList<T> next;

        [Rule("<Stm Block> ::= ~<NL>", typeof(GrapeStatement))]
        [Rule("<Switch Section Block> ::= ~<NL>", typeof(GrapeSwitchCase))]
        [Rule("<Catch Clause List Opt> ::=", typeof(GrapeCatchClause))]
        [Rule("<Decl List> ::=", typeof(GrapeDeclaration))]
        [Rule("<Arg List Opt> ::=", typeof(GrapeExpression))]
        [Rule("<Modifier List Opt> ::=", typeof(GrapeModifier))]
        [Rule("<Class Item Block> ::= ~<NL>", typeof(GrapeClassItem))]
        [Rule("<Formal Param List Opt> ::=", typeof(GrapeParameter))]
        public GrapeList() : this(null, null) { }

        [Rule("<Stm List> ::= <Statement>", typeof(GrapeStatement))]
        [Rule("<Qualified ID> ::= Identifier", typeof(GrapeIdentifier))]
        [Rule("<Switch Section List> ::= <Switch Label>", typeof(GrapeSwitchCase))]
        [Rule("<Switch Section List> ::= <Switch Default>", typeof(GrapeSwitchCase))]
        [Rule("<Arg List> ::= <Argument>", typeof(GrapeExpression))]
        [Rule("<Class Item List> ::= <Class Item>", typeof(GrapeClassItem))]
        [Rule("<Formal Param List> ::= <Formal Param>", typeof(GrapeParameter))]
        [Rule("<Variable Initializer List> ::= <Variable Initializer>", typeof(GrapeInitializer))]
        public GrapeList(T item) : this(item, null) { }

        [Rule("<Stm List> ::= <Statement> <Stm List>", typeof(GrapeStatement))]
        [Rule("<Qualified ID> ::= Identifier ~'.' <Qualified ID>", typeof(GrapeIdentifier))]
        [Rule("<Switch Section List> ::= <Switch Label> <Switch Section List>", typeof(GrapeSwitchCase))]
        [Rule("<Catch Clause List Opt> ::= <Catch Clause> <Catch Clause List Opt>", typeof(GrapeCatchClause))]
        [Rule("<Decl List> ::= <Package> <Decl List>", typeof(GrapeDeclaration))]
        [Rule("<Decl List> ::= <Import> <Decl List>", typeof(GrapeDeclaration))]
        [Rule("<Decl List> ::= <Type Decl> <Decl List>", typeof(GrapeDeclaration))]
        [Rule("<Arg List> ::= <Argument> ~',' <Arg List>", typeof(GrapeExpression))]
        [Rule("<Modifier List Opt> ::= <Modifier> <Modifier List Opt>", typeof(GrapeModifier))]
        [Rule("<Class Item List> ::= <Class Item> <Class Item List>", typeof(GrapeClassItem))]
        [Rule("<Formal Param List> ::= <Formal Param> ~',' <Formal Param List>", typeof(GrapeParameter))]
        [Rule("<Variable Initializer List> ::= <Variable Initializer> ~',' <Variable Initializer List>", typeof(GrapeInitializer))]
        public GrapeList(T item, GrapeList<T> next) {
            this.item = item;
            this.next = next;
        }

        public T Item {
            get {
                return item;
            }
        }

        public GrapeList<T> Next {
            get {
                return next;
            }
        }
    }
}