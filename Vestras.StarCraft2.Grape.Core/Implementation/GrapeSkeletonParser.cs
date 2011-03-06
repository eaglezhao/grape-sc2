using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

using bsn.GoldParser.Grammar;
using bsn.GoldParser.Parser;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
    [Serializable()]
    internal class SymbolException : Exception {
        public SymbolException(string message)
            : base(message) {
        }

        public SymbolException(string message,
            Exception inner)
            : base(message, inner) {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context)
            : base(info, context) {
        }

    }

    [Serializable()]
    internal class RuleException : Exception {
        public RuleException(string message)
            : base(message) {
        }

        public RuleException(string message, Exception inner)
            : base(message, inner) {
        }

        protected RuleException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }

    enum SymbolConstants : int {
        SYMBOL_EOF = 0, // (EOF)
        SYMBOL_ERROR = 1, // (Error)
        SYMBOL_WHITESPACE = 2, // (Whitespace)
        SYMBOL_COMMENTEND = 3, // (Comment End)
        SYMBOL_COMMENTLINE = 4, // (Comment Line)
        SYMBOL_COMMENTSTART = 5, // (Comment Start)
        SYMBOL_MINUS = 6, // '-'
        SYMBOL_EXCLAM = 7, // '!'
        SYMBOL_EXCLAMEQ = 8, // '!='
        SYMBOL_PERCENT = 9, // '%'
        SYMBOL_AMP = 10, // '&'
        SYMBOL_AMPAMP = 11, // '&&'
        SYMBOL_LPARAN = 12, // '('
        SYMBOL_RPARAN = 13, // ')'
        SYMBOL_TIMES = 14, // '*'
        SYMBOL_COMMA = 15, // ','
        SYMBOL_DIV = 16, // '/'
        SYMBOL_LBRACKET = 17, // '['
        SYMBOL_RBRACKET = 18, // ']'
        SYMBOL_CARET = 19, // '^'
        SYMBOL_PIPE = 20, // '|'
        SYMBOL_PIPEPIPE = 21, // '||'
        SYMBOL_TILDE = 22, // '~'
        SYMBOL_PLUS = 23, // '+'
        SYMBOL_LT = 24, // '<'
        SYMBOL_LTLT = 25, // '<<'
        SYMBOL_LTEQ = 26, // '<='
        SYMBOL_EQ = 27, // '='
        SYMBOL_EQEQ = 28, // '=='
        SYMBOL_GT = 29, // '>'
        SYMBOL_GTEQ = 30, // '>='
        SYMBOL_GTGT = 31, // '>>'
        SYMBOL_ABSTRACT = 32, // abstract
        SYMBOL_AS = 33, // as
        SYMBOL_BASE = 34, // base
        SYMBOL_BREAK = 35, // break
        SYMBOL_CASE = 36, // case
        SYMBOL_CATCH = 37, // catch
        SYMBOL_CLASS = 38, // class
        SYMBOL_CONTINUE = 39, // continue
        SYMBOL_CTOR = 40, // ctor
        SYMBOL_DCTOR = 41, // dctor
        SYMBOL_DECIMALLITERAL = 42, // DecimalLiteral
        SYMBOL_DEFAULT = 43, // default
        SYMBOL_DELETE = 44, // delete
        SYMBOL_ELSE = 45, // else
        SYMBOL_ELSEIF = 46, // elseif
        SYMBOL_END = 47, // end
        SYMBOL_FALSE = 48, // false
        SYMBOL_FINALLY = 49, // finally
        SYMBOL_FOREACH = 50, // foreach
        SYMBOL_HEXLITERAL = 51, // HexLiteral
        SYMBOL_IDENTIFIER = 52, // Identifier
        SYMBOL_IF = 53, // if
        SYMBOL_IMPORT = 54, // import
        SYMBOL_IN = 55, // in
        SYMBOL_INHERITS = 56, // inherits
        SYMBOL_INIT = 57, // init
        SYMBOL_INTERNAL = 58, // internal
        SYMBOL_MEMBERNAME = 59, // MemberName
        SYMBOL_NAMEOF = 60, // nameof
        SYMBOL_NEW = 61, // new
        SYMBOL_NEWLINE = 62, // NewLine
        SYMBOL_NULL = 63, // null
        SYMBOL_OVERRIDE = 64, // override
        SYMBOL_PACKAGE = 65, // package
        SYMBOL_PRIVATE = 66, // private
        SYMBOL_PROTECTED = 67, // protected
        SYMBOL_PUBLIC = 68, // public
        SYMBOL_REALLITERAL = 69, // RealLiteral
        SYMBOL_RETURN = 70, // return
        SYMBOL_SEALED = 71, // sealed
        SYMBOL_STATIC = 72, // static
        SYMBOL_STRINGLITERAL = 73, // StringLiteral
        SYMBOL_SWITCH = 74, // switch
        SYMBOL_THIS = 75, // this
        SYMBOL_THROW = 76, // throw
        SYMBOL_TRUE = 77, // true
        SYMBOL_TRY = 78, // try
        SYMBOL_WHILE = 79, // while
        SYMBOL_ACCESS = 80, // <Access>
        SYMBOL_ADDEXP = 81, // <Add Exp>
        SYMBOL_ANDEXP = 82, // <And Exp>
        SYMBOL_ARGLIST = 83, // <Arg List>
        SYMBOL_ARGLISTOPT = 84, // <Arg List Opt>
        SYMBOL_ARGUMENT = 85, // <Argument>
        SYMBOL_ARRAYINITIALIZER = 86, // <Array Initializer>
        SYMBOL_ASSIGNTAIL = 87, // <Assign Tail>
        SYMBOL_CATCHCLAUSE = 88, // <Catch Clause>
        SYMBOL_CATCHCLAUSES = 89, // <Catch Clauses>
        SYMBOL_CLASSBASEOPT = 90, // <Class Base Opt>
        SYMBOL_CLASSDECL = 91, // <Class Decl>
        SYMBOL_CLASSITEM = 92, // <Class Item>
        SYMBOL_CLASSITEMDECSOPT = 93, // <Class Item Decs Opt>
        SYMBOL_CLASSSIZEOPT = 94, // <Class Size Opt>
        SYMBOL_COMPAREEXP = 95, // <Compare Exp>
        SYMBOL_CONSTRUCTORDEC = 96, // <Constructor Dec>
        SYMBOL_CONSTRUCTORINITSTMS = 97, // <Constructor Init Stms>
        SYMBOL_DESTRUCTORDEC = 98, // <Destructor Dec>
        SYMBOL_EQUALITYEXP = 99, // <Equality Exp>
        SYMBOL_EXPRESSION = 100, // <Expression>
        SYMBOL_EXPRESSIONOPT = 101, // <Expression Opt>
        SYMBOL_FIELDDEC = 102, // <Field Dec>
        SYMBOL_FINALLYCLAUSEOPT = 103, // <Finally Clause Opt>
        SYMBOL_FORMALPARAM = 104, // <Formal Param>
        SYMBOL_FORMALPARAMLIST = 105, // <Formal Param List>
        SYMBOL_FORMALPARAMLISTOPT = 106, // <Formal Param List Opt>
        SYMBOL_IMPORT2 = 107, // <Import>
        SYMBOL_LITERAL = 108, // <Literal>
        SYMBOL_LOCALVARDECL = 109, // <Local Var Decl>
        SYMBOL_LOGICALANDEXP = 110, // <Logical And Exp>
        SYMBOL_LOGICALOREXP = 111, // <Logical Or Exp>
        SYMBOL_LOGICALXOREXP = 112, // <Logical Xor Exp>
        SYMBOL_MEMBERLIST = 113, // <Member List>
        SYMBOL_METHODCALL = 114, // <Method Call>
        SYMBOL_METHODCALLS = 115, // <Method Calls>
        SYMBOL_METHODDEC = 116, // <Method Dec>
        SYMBOL_MODIFIER = 117, // <Modifier>
        SYMBOL_MODIFIERS = 118, // <Modifiers>
        SYMBOL_MULTEXP = 119, // <Mult Exp>
        SYMBOL_NL = 120, // <NL>
        SYMBOL_NLOREOF = 121, // <NL Or EOF>
        SYMBOL_NONARRAYTYPE = 122, // <Non Array Type>
        SYMBOL_NORMALSTM = 123, // <Normal Stm>
        SYMBOL_OBJECTS = 124, // <Objects>
        SYMBOL_OREXP = 125, // <Or Exp>
        SYMBOL_PACKAGE2 = 126, // <Package>
        SYMBOL_QUALIFIEDID = 127, // <Qualified ID>
        SYMBOL_RANKSPECIFIER = 128, // <Rank Specifier>
        SYMBOL_RANKSPECIFIERS = 129, // <Rank Specifiers>
        SYMBOL_SHIFTEXP = 130, // <Shift Exp>
        SYMBOL_START = 131, // <Start>
        SYMBOL_STATEMENT = 132, // <Statement>
        SYMBOL_STATEMENTEXP = 133, // <Statement Exp>
        SYMBOL_STMLIST = 134, // <Stm List>
        SYMBOL_SWITCHLABEL = 135, // <Switch Label>
        SYMBOL_SWITCHSECTIONSOPT = 136, // <Switch Sections Opt>
        SYMBOL_TYPE = 137, // <Type>
        SYMBOL_TYPEDECL = 138, // <Type Decl>
        SYMBOL_TYPEDECLOPT = 139, // <Type Decl Opt>
        SYMBOL_TYPECASTEXP = 140, // <Typecast Exp>
        SYMBOL_UNARYEXP = 141, // <Unary Exp>
        SYMBOL_VALIDID = 142, // <Valid ID>
        SYMBOL_VALUE = 143, // <Value>
        SYMBOL_VARIABLEDECLARATOR = 144, // <Variable Declarator>
        SYMBOL_VARIABLEDECLARATORBASE = 145, // <Variable Declarator Base>
        SYMBOL_VARIABLEINITIALIZER = 146, // <Variable Initializer>
        SYMBOL_VARIABLEINITIALIZERLIST = 147, // <Variable Initializer List>
        SYMBOL_VARIABLEINITIALIZERLISTOPT = 148  // <Variable Initializer List Opt>
    };

    enum RuleConstants : int {
        RULE_NL_NEWLINE = 0, // <NL> ::= NewLine <NL>
        RULE_NL_NEWLINE2 = 1, // <NL> ::= NewLine
        RULE_NLOREOF = 2, // <NL Or EOF> ::= <NL>
        RULE_NLOREOF2 = 3, // <NL Or EOF> ::= 
        RULE_START = 4, // <Start> ::= <Package> <Start>
        RULE_START2 = 5, // <Start> ::= <Import> <Start>
        RULE_START3 = 6, // <Start> ::= <Type Decl Opt> <Start>
        RULE_START4 = 7, // <Start> ::= 
        RULE_OBJECTS_THIS = 8, // <Objects> ::= this
        RULE_OBJECTS_BASE = 9, // <Objects> ::= base
        RULE_VALIDID_IDENTIFIER = 10, // <Valid ID> ::= Identifier
        RULE_VALIDID = 11, // <Valid ID> ::= <Objects>
        RULE_QUALIFIEDID = 12, // <Qualified ID> ::= <Valid ID> <Member List>
        RULE_MEMBERLIST_MEMBERNAME = 13, // <Member List> ::= <Member List> MemberName
        RULE_MEMBERLIST = 14, // <Member List> ::= 
        RULE_LITERAL_TRUE = 15, // <Literal> ::= true
        RULE_LITERAL_FALSE = 16, // <Literal> ::= false
        RULE_LITERAL_DECIMALLITERAL = 17, // <Literal> ::= DecimalLiteral
        RULE_LITERAL_HEXLITERAL = 18, // <Literal> ::= HexLiteral
        RULE_LITERAL_REALLITERAL = 19, // <Literal> ::= RealLiteral
        RULE_LITERAL_STRINGLITERAL = 20, // <Literal> ::= StringLiteral
        RULE_LITERAL_NULL = 21, // <Literal> ::= null
        RULE_PACKAGE_PACKAGE = 22, // <Package> ::= package <Qualified ID> <NL>
        RULE_IMPORT_IMPORT = 23, // <Import> ::= import <Qualified ID> <NL>
        RULE_TYPE = 24, // <Type> ::= <Non Array Type>
        RULE_TYPE2 = 25, // <Type> ::= <Non Array Type> <Rank Specifiers>
        RULE_NONARRAYTYPE = 26, // <Non Array Type> ::= <Qualified ID>
        RULE_RANKSPECIFIERS = 27, // <Rank Specifiers> ::= <Rank Specifier>
        RULE_RANKSPECIFIER_LBRACKET_RBRACKET = 28, // <Rank Specifier> ::= '[' <Expression> ']'
        RULE_EXPRESSIONOPT = 29, // <Expression Opt> ::= <Expression>
        RULE_EXPRESSIONOPT2 = 30, // <Expression Opt> ::= 
        RULE_EXPRESSION_EQ = 31, // <Expression> ::= <Or Exp> '=' <Expression>
        RULE_EXPRESSION = 32, // <Expression> ::= <Or Exp>
        RULE_OREXP_PIPEPIPE = 33, // <Or Exp> ::= <Or Exp> '||' <And Exp>
        RULE_OREXP = 34, // <Or Exp> ::= <And Exp>
        RULE_ANDEXP_AMPAMP = 35, // <And Exp> ::= <And Exp> '&&' <Logical Or Exp>
        RULE_ANDEXP = 36, // <And Exp> ::= <Logical Or Exp>
        RULE_LOGICALOREXP_PIPE = 37, // <Logical Or Exp> ::= <Logical Or Exp> '|' <Logical Xor Exp>
        RULE_LOGICALOREXP = 38, // <Logical Or Exp> ::= <Logical Xor Exp>
        RULE_LOGICALXOREXP_CARET = 39, // <Logical Xor Exp> ::= <Logical Xor Exp> '^' <Logical And Exp>
        RULE_LOGICALXOREXP = 40, // <Logical Xor Exp> ::= <Logical And Exp>
        RULE_LOGICALANDEXP_AMP = 41, // <Logical And Exp> ::= <Logical And Exp> '&' <Equality Exp>
        RULE_LOGICALANDEXP = 42, // <Logical And Exp> ::= <Equality Exp>
        RULE_EQUALITYEXP_EQEQ = 43, // <Equality Exp> ::= <Equality Exp> '==' <Compare Exp>
        RULE_EQUALITYEXP_EXCLAMEQ = 44, // <Equality Exp> ::= <Equality Exp> '!=' <Compare Exp>
        RULE_EQUALITYEXP = 45, // <Equality Exp> ::= <Compare Exp>
        RULE_COMPAREEXP_LT = 46, // <Compare Exp> ::= <Compare Exp> '<' <Shift Exp>
        RULE_COMPAREEXP_GT = 47, // <Compare Exp> ::= <Compare Exp> '>' <Shift Exp>
        RULE_COMPAREEXP_LTEQ = 48, // <Compare Exp> ::= <Compare Exp> '<=' <Shift Exp>
        RULE_COMPAREEXP_GTEQ = 49, // <Compare Exp> ::= <Compare Exp> '>=' <Shift Exp>
        RULE_COMPAREEXP = 50, // <Compare Exp> ::= <Shift Exp>
        RULE_SHIFTEXP_LTLT = 51, // <Shift Exp> ::= <Shift Exp> '<<' <Add Exp>
        RULE_SHIFTEXP_GTGT = 52, // <Shift Exp> ::= <Shift Exp> '>>' <Add Exp>
        RULE_SHIFTEXP = 53, // <Shift Exp> ::= <Add Exp>
        RULE_ADDEXP_PLUS = 54, // <Add Exp> ::= <Add Exp> '+' <Mult Exp>
        RULE_ADDEXP_MINUS = 55, // <Add Exp> ::= <Add Exp> '-' <Mult Exp>
        RULE_ADDEXP = 56, // <Add Exp> ::= <Mult Exp>
        RULE_MULTEXP_TIMES = 57, // <Mult Exp> ::= <Mult Exp> '*' <Typecast Exp>
        RULE_MULTEXP_DIV = 58, // <Mult Exp> ::= <Mult Exp> '/' <Typecast Exp>
        RULE_MULTEXP_PERCENT = 59, // <Mult Exp> ::= <Mult Exp> '%' <Typecast Exp>
        RULE_MULTEXP = 60, // <Mult Exp> ::= <Typecast Exp>
        RULE_TYPECASTEXP_AS = 61, // <Typecast Exp> ::= <Unary Exp> as <Qualified ID>
        RULE_TYPECASTEXP = 62, // <Typecast Exp> ::= <Unary Exp>
        RULE_UNARYEXP_MINUS = 63, // <Unary Exp> ::= '-' <Value>
        RULE_UNARYEXP_EXCLAM = 64, // <Unary Exp> ::= '!' <Value>
        RULE_UNARYEXP_TILDE = 65, // <Unary Exp> ::= '~' <Value>
        RULE_UNARYEXP = 66, // <Unary Exp> ::= <Value>
        RULE_LOCALVARDECL = 67, // <Local Var Decl> ::= <Variable Declarator>
        RULE_STATEMENTEXP = 68, // <Statement Exp> ::= <Qualified ID> <Method Calls>
        RULE_STATEMENTEXP_LBRACKET_RBRACKET = 69, // <Statement Exp> ::= <Qualified ID> '[' <Expression> ']' <Method Calls>
        RULE_STATEMENTEXP_LPARAN_RPARAN = 70, // <Statement Exp> ::= <Qualified ID> '(' <Arg List Opt> ')' <Method Calls>
        RULE_STATEMENTEXP2 = 71, // <Statement Exp> ::= <Qualified ID> <Method Calls> <Assign Tail>
        RULE_STATEMENTEXP_LPARAN_RPARAN2 = 72, // <Statement Exp> ::= <Qualified ID> '(' <Arg List Opt> ')' <Method Calls> <Assign Tail>
        RULE_STATEMENTEXP_LBRACKET_RBRACKET2 = 73, // <Statement Exp> ::= <Qualified ID> '[' <Expression> ']' <Method Calls> <Assign Tail>
        RULE_STATEMENTEXP3 = 74, // <Statement Exp> ::= <Variable Declarator Base>
        RULE_ASSIGNTAIL_EQ = 75, // <Assign Tail> ::= '=' <Expression>
        RULE_METHODCALLS = 76, // <Method Calls> ::= <Method Call> <Method Calls>
        RULE_METHODCALLS2 = 77, // <Method Calls> ::= 
        RULE_METHODCALL_MEMBERNAME = 78, // <Method Call> ::= MemberName
        RULE_METHODCALL_MEMBERNAME_LPARAN_RPARAN = 79, // <Method Call> ::= MemberName '(' <Arg List Opt> ')'
        RULE_METHODCALL_MEMBERNAME_LBRACKET_RBRACKET = 80, // <Method Call> ::= MemberName '[' <Expression> ']'
        RULE_VALUE_LPARAN_RPARAN = 81, // <Value> ::= '(' <Expression> ')'
        RULE_VALUE = 82, // <Value> ::= <Literal>
        RULE_VALUE2 = 83, // <Value> ::= <Statement Exp>
        RULE_VALUE_NAMEOF_LPARAN_RPARAN = 84, // <Value> ::= nameof '(' <Qualified ID> ')'
        RULE_VALUE_NEW_LPARAN_RPARAN = 85, // <Value> ::= new <Qualified ID> '(' <Arg List Opt> ')'
        RULE_ARGLISTOPT = 86, // <Arg List Opt> ::= <Arg List>
        RULE_ARGLISTOPT2 = 87, // <Arg List Opt> ::= 
        RULE_ARGLIST_COMMA = 88, // <Arg List> ::= <Arg List> ',' <Argument>
        RULE_ARGLIST = 89, // <Arg List> ::= <Argument>
        RULE_ARGUMENT = 90, // <Argument> ::= <Expression>
        RULE_STMLIST = 91, // <Stm List> ::= <Stm List> <Statement>
        RULE_STMLIST2 = 92, // <Stm List> ::= 
        RULE_STATEMENT = 93, // <Statement> ::= <Local Var Decl>
        RULE_STATEMENT_IF_END = 94, // <Statement> ::= if <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_ELSE_END = 95, // <Statement> ::= else <NL> <Stm List> end <NL>
        RULE_STATEMENT_ELSEIF_END = 96, // <Statement> ::= elseif <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END = 97, // <Statement> ::= foreach <Qualified ID> Identifier in <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END2 = 98, // <Statement> ::= foreach <Qualified ID> <Rank Specifiers> Identifier in <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_WHILE_END = 99, // <Statement> ::= while <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT2 = 100, // <Statement> ::= <Normal Stm>
        RULE_NORMALSTM_SWITCH_END = 101, // <Normal Stm> ::= switch <Expression> <NL> <Switch Sections Opt> end <NL>
        RULE_NORMALSTM_TRY_END = 102, // <Normal Stm> ::= try <NL> <Stm List> end <NL>
        RULE_NORMALSTM = 103, // <Normal Stm> ::= <Catch Clauses>
        RULE_NORMALSTM2 = 104, // <Normal Stm> ::= <Finally Clause Opt>
        RULE_NORMALSTM_BREAK = 105, // <Normal Stm> ::= break <NL>
        RULE_NORMALSTM_CONTINUE = 106, // <Normal Stm> ::= continue <NL>
        RULE_NORMALSTM_RETURN = 107, // <Normal Stm> ::= return <Expression Opt> <NL>
        RULE_NORMALSTM_THROW = 108, // <Normal Stm> ::= throw <Expression Opt> <NL>
        RULE_NORMALSTM_DELETE = 109, // <Normal Stm> ::= delete <Expression> <NL>
        RULE_NORMALSTM3 = 110, // <Normal Stm> ::= <Expression> <NL>
        RULE_NORMALSTM4 = 111, // <Normal Stm> ::= <Constructor Init Stms> <NL>
        RULE_CONSTRUCTORINITSTMS_INIT_BASE_LPARAN_RPARAN = 112, // <Constructor Init Stms> ::= init base '(' <Arg List Opt> ')'
        RULE_CONSTRUCTORINITSTMS_INIT_THIS_LPARAN_RPARAN = 113, // <Constructor Init Stms> ::= init this '(' <Arg List Opt> ')'
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER = 114, // <Variable Declarator Base> ::= <Qualified ID> Identifier
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER2 = 115, // <Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ = 116, // <Variable Declarator Base> ::= <Qualified ID> Identifier '=' <Variable Initializer>
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ2 = 117, // <Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier '=' <Variable Initializer>
        RULE_VARIABLEDECLARATOR = 118, // <Variable Declarator> ::= <Variable Declarator Base> <NL>
        RULE_VARIABLEINITIALIZER = 119, // <Variable Initializer> ::= <Expression>
        RULE_VARIABLEINITIALIZER2 = 120, // <Variable Initializer> ::= <Array Initializer>
        RULE_SWITCHSECTIONSOPT = 121, // <Switch Sections Opt> ::= <Switch Sections Opt> <Switch Label>
        RULE_SWITCHSECTIONSOPT2 = 122, // <Switch Sections Opt> ::= 
        RULE_SWITCHLABEL_CASE_END = 123, // <Switch Label> ::= case <Expression> <NL> <Stm List> end <NL>
        RULE_SWITCHLABEL_DEFAULT_END = 124, // <Switch Label> ::= default <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSES = 125, // <Catch Clauses> ::= <Catch Clause> <Catch Clauses>
        RULE_CATCHCLAUSES2 = 126, // <Catch Clauses> ::= 
        RULE_CATCHCLAUSE_CATCH_IDENTIFIER_END = 127, // <Catch Clause> ::= catch <Qualified ID> Identifier <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSE_CATCH_END = 128, // <Catch Clause> ::= catch <Qualified ID> <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSE_CATCH_END2 = 129, // <Catch Clause> ::= catch <NL> <Stm List> end <NL>
        RULE_FINALLYCLAUSEOPT_FINALLY_END = 130, // <Finally Clause Opt> ::= finally <NL> <Stm List> end <NL>
        RULE_FINALLYCLAUSEOPT = 131, // <Finally Clause Opt> ::= <NL>
        RULE_ACCESS_PRIVATE = 132, // <Access> ::= private
        RULE_ACCESS_PROTECTED = 133, // <Access> ::= protected
        RULE_ACCESS_PUBLIC = 134, // <Access> ::= public
        RULE_ACCESS_INTERNAL = 135, // <Access> ::= internal
        RULE_MODIFIER_ABSTRACT = 136, // <Modifier> ::= abstract
        RULE_MODIFIER_OVERRIDE = 137, // <Modifier> ::= override
        RULE_MODIFIER_SEALED = 138, // <Modifier> ::= sealed
        RULE_MODIFIER_STATIC = 139, // <Modifier> ::= static
        RULE_MODIFIER = 140, // <Modifier> ::= <Access>
        RULE_MODIFIERS = 141, // <Modifiers> ::= <Modifier> <Modifiers>
        RULE_MODIFIERS2 = 142, // <Modifiers> ::= 
        RULE_CLASSDECL_CLASS_IDENTIFIER_END = 143, // <Class Decl> ::= <Modifiers> class Identifier <Class Size Opt> <Class Base Opt> <NL> <Class Item Decs Opt> end <NL Or EOF>
        RULE_CLASSSIZEOPT_LBRACKET_DECIMALLITERAL_RBRACKET = 144, // <Class Size Opt> ::= '[' DecimalLiteral ']'
        RULE_CLASSSIZEOPT = 145, // <Class Size Opt> ::= 
        RULE_CLASSBASEOPT_INHERITS = 146, // <Class Base Opt> ::= inherits <Non Array Type>
        RULE_CLASSBASEOPT = 147, // <Class Base Opt> ::= 
        RULE_CLASSITEMDECSOPT = 148, // <Class Item Decs Opt> ::= <Class Item Decs Opt> <Class Item>
        RULE_CLASSITEMDECSOPT2 = 149, // <Class Item Decs Opt> ::= 
        RULE_CLASSITEM = 150, // <Class Item> ::= <Method Dec>
        RULE_CLASSITEM2 = 151, // <Class Item> ::= <Constructor Dec>
        RULE_CLASSITEM3 = 152, // <Class Item> ::= <Destructor Dec>
        RULE_CLASSITEM4 = 153, // <Class Item> ::= <Type Decl>
        RULE_CLASSITEM5 = 154, // <Class Item> ::= <Field Dec>
        RULE_FIELDDEC_IDENTIFIER = 155, // <Field Dec> ::= <Modifiers> <Type> Identifier <NL>
        RULE_FIELDDEC_IDENTIFIER_EQ = 156, // <Field Dec> ::= <Modifiers> <Type> Identifier '=' <Expression> <NL>
        RULE_METHODDEC_IDENTIFIER_LPARAN_RPARAN_END = 157, // <Method Dec> ::= <Modifiers> <Type> Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
        RULE_FORMALPARAMLISTOPT = 158, // <Formal Param List Opt> ::= <Formal Param List>
        RULE_FORMALPARAMLISTOPT2 = 159, // <Formal Param List Opt> ::= 
        RULE_FORMALPARAMLIST = 160, // <Formal Param List> ::= <Formal Param>
        RULE_FORMALPARAMLIST_COMMA = 161, // <Formal Param List> ::= <Formal Param List> ',' <Formal Param>
        RULE_FORMALPARAM_IDENTIFIER = 162, // <Formal Param> ::= <Type> Identifier
        RULE_TYPEDECL = 163, // <Type Decl> ::= <Class Decl>
        RULE_TYPEDECLOPT = 164, // <Type Decl Opt> ::= <Type Decl>
        RULE_TYPEDECLOPT2 = 165, // <Type Decl Opt> ::= <NL>
        RULE_CONSTRUCTORDEC_CTOR_IDENTIFIER_LPARAN_RPARAN_END = 166, // <Constructor Dec> ::= <Modifiers> ctor Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
        RULE_DESTRUCTORDEC_DCTOR_IDENTIFIER_LPARAN_RPARAN_END = 167, // <Destructor Dec> ::= <Modifiers> dctor Identifier '(' ')' <NL> <Stm List> end <NL>
        RULE_ARRAYINITIALIZER_LBRACKET_RBRACKET = 168, // <Array Initializer> ::= '[' <Variable Initializer List Opt> ']'
        RULE_ARRAYINITIALIZER_LBRACKET_COMMA_RBRACKET = 169, // <Array Initializer> ::= '[' <Variable Initializer List> ',' ']'
        RULE_VARIABLEINITIALIZERLISTOPT = 170, // <Variable Initializer List Opt> ::= <Variable Initializer List>
        RULE_VARIABLEINITIALIZERLISTOPT2 = 171, // <Variable Initializer List Opt> ::= 
        RULE_VARIABLEINITIALIZERLIST = 172, // <Variable Initializer List> ::= <Variable Initializer>
        RULE_VARIABLEINITIALIZERLIST_COMMA = 173  // <Variable Initializer List> ::= <Variable Initializer List> ',' <Variable Initializer>
    };

    internal class GrapeSkeletonParser {
        private LalrProcessor parser;
        private GrapeParserConfiguration config;
        private List<Reduction> processedExpressionTokens = new List<Reduction>();
        internal GrapeErrorSink errorSink;
        internal string currentFileName;
        internal static Dictionary<Type, List<GrapeEntity>> allEntities = new Dictionary<Type, List<GrapeEntity>>();
        internal static Dictionary<string, Dictionary<Type, List<GrapeEntity>>> allEntitiesWithFileFilter = new Dictionary<string, Dictionary<Type, List<GrapeEntity>>>();

        public GrapeSkeletonParser(string filename, GrapeParserConfiguration config) {
            this.config = config;
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
            	Init(stream);
            }
        }

        public GrapeSkeletonParser(string baseName, string resourceName, GrapeParserConfiguration config) {
            this.config = config;
					if (!string.IsNullOrEmpty(baseName)) {
						resourceName = baseName+Type.Delimiter+resourceName;
					}
					using (Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName)) {
						if (stream== null) {
							throw new FileNotFoundException(string.Format("The embedded resource file {0} does not exist in the assembly {1}", resourceName, GetType().Assembly.FullName));
						}
            Init(stream);
					}
        }

        public GrapeSkeletonParser(Stream stream, GrapeParserConfiguration config) {
            this.config = config;
            Init(stream);
        }

        private void Init(Stream stream) {
            grammar = CompiledGrammar.Load(stream);
        }

        public void Parse(string source) {
					using (StringReader reader = new StringReader(source)) {
						LalrProcessor processor = new LalrProcessor(new Tokenizer(reader, grammar), false);
						// TODO: implement the continue parsing on error (requires inheriting from the LalrProcessor)
						bool continueParsing;
						switch (processor.ParseAll()) {
						case ParseMessage.Accept:
							AcceptEvent((Reduction)processor.CurrentToken);
							break;
						case ParseMessage.LexicalError:
							TokenErrorEvent(processor, out continueParsing);
							break;
						case ParseMessage.SyntaxError:
							ParseErrorEvent(processor, out continueParsing);
							break;
						default:
							throw new InvalidOperationException("The parser returned an unexpected result");
						}
					}
        }

        private GrapePackageDeclaration currentPackageDeclaration;
        private GrapeEntityWithBlock currentEntityWithBlock;
        private GrapeEntityWithBlock lastEntityWithBlock;
        private GrapeEntity currentEntity;
        private GrapeBlock currentBlock;
        private void AddGrapeEntityToCurrentParent(GrapeEntity entity) {
            entity.FileName = currentFileName;
            while (currentBlock != null && entity.Offset > currentBlock.Offset + currentBlock.Length) {
                if (entity is GrapeStatement && !((GrapeStatement)entity).CanHaveBlock) {
                    break;
                }

                GrapeEntityWithBlock parentEntityWithBlock = currentBlock.Parent != null && currentBlock.Parent.Parent != null ? currentBlock.Parent.Parent as GrapeEntityWithBlock : null;
                if (parentEntityWithBlock != null) {
                    currentBlock = parentEntityWithBlock.Block;
                    currentEntity = parentEntityWithBlock;
                } else if (currentBlock.Parent.Parent is GrapeBlock) {
                    currentBlock = currentBlock.Parent.Parent as GrapeBlock;
                    currentEntity = currentBlock.Parent;
                } else {
                    currentBlock = null;
                    currentEntity = null;
                }
            }

            if (currentBlock != null) {
                entity.Parent = currentBlock;
                currentBlock.Children.Add(entity);
            } else if (currentPackageDeclaration != null) {
                entity.Parent = currentPackageDeclaration;
                currentPackageDeclaration.Children.Add(entity);
            } else {
                config.Ast.Children.Add(entity);
            }

            if (allEntities.ContainsKey(entity.GetType())) {
                allEntities[entity.GetType()].Add(entity);
            } else {
                List<GrapeEntity> list = new List<GrapeEntity>();
                list.Add(entity);
                allEntities[entity.GetType()] = list;
            }

            if (allEntitiesWithFileFilter.ContainsKey(entity.FileName)) {
                if (allEntitiesWithFileFilter[entity.FileName].ContainsKey(entity.GetType())) {
                    allEntitiesWithFileFilter[entity.FileName][entity.GetType()].Add(entity);
                } else {
                    List<GrapeEntity> list = new List<GrapeEntity>();
                    list.Add(entity);
                    allEntitiesWithFileFilter[entity.FileName].Add(entity.GetType(), list);
                }
            } else {
                Dictionary<Type, List<GrapeEntity>> dictionary = new Dictionary<Type, List<GrapeEntity>>();
                List<GrapeEntity> list = new List<GrapeEntity>();
                list.Add(entity);
                dictionary.Add(entity.GetType(), list);
                allEntitiesWithFileFilter.Add(entity.FileName, dictionary);
            }
        }

        private string GetQualifiedIdText(Reduction qualifiedIdToken, out TextToken lastToken) {
            TextToken dummyToken = null;
            return GetQualifiedIdText(qualifiedIdToken, ref dummyToken, out lastToken);
        }

        private string GetQualifiedIdText(Reduction qualifiedIdToken, ref TextToken firstToken, out TextToken lastToken) {
            string qualifiedIdText = "";
            return GetQualifiedIdText(qualifiedIdToken, ref firstToken, ref qualifiedIdText, out lastToken);
        }

        private string GetQualifiedIdText(Reduction qualifiedIdToken, ref TextToken firstToken, ref string qualifiedIdText, out TextToken lastToken) {
            lastToken = null;
            foreach (Token childNormalToken in qualifiedIdToken.Children) {
                Reduction childToken = childNormalToken as Reduction;
                TextToken childTerminalToken = childNormalToken as TextToken;
                if (childToken != null && childToken.Children.Count >= 1) {
                    foreach (Token terminalNormalToken in childToken.Children) {
                        TextToken terminalToken = terminalNormalToken as TextToken;
                        Reduction nonterminalToken = terminalNormalToken as Reduction;
                        if (terminalToken != null) {
                            if (firstToken == null) {
                                firstToken = terminalToken;
                            } else if (terminalToken.Position.Index < terminalToken.Position.Index) {
                                firstToken = terminalToken;
                            }

                            qualifiedIdText += terminalToken.Text;
                            lastToken = terminalToken;
                        } else if (nonterminalToken != null && nonterminalToken.Symbol.Index != (int)RuleConstants.RULE_MEMBERLIST) {
                            GetQualifiedIdText(nonterminalToken, ref firstToken, ref qualifiedIdText, out lastToken);
                        }
                    }
                } else if (childTerminalToken != null) {
                    if (firstToken == null) {
                        firstToken = childTerminalToken;
                    } else if (childTerminalToken.Position.Index < childTerminalToken.Position.Index) {
                        firstToken = childTerminalToken;
                    }

                    qualifiedIdText += childTerminalToken.Text;
                }
            }

            return qualifiedIdText;
        }

        private static readonly string[] AccessModifiers = new string[] {
            "public",
            "private",
            "protected",
            "internal"
        };

        private string GetModifiers(Reduction modifiers, out TextToken firstToken) {
            string foundModifiers = GetModifiers(modifiers, out firstToken, true);
            if (foundModifiers.Trim() == "") {
                return "public";
            }

            return foundModifiers;
        }

        private string GetModifiers(Reduction modifiers, out TextToken firstToken, bool dummyArgument) {
            string result = "";
            firstToken = null;
            if (modifiers.Children.Count >= 1) {
                int index = 0;
                foreach (Token token in modifiers.Children) {
                    Reduction nonterminalToken = token as Reduction;
                    if (nonterminalToken != null) {
                        int id = nonterminalToken.Symbol.Index;
                        if (id == (int)RuleConstants.RULE_MODIFIER || id == (int)RuleConstants.RULE_MODIFIER_ABSTRACT || id == (int)RuleConstants.RULE_MODIFIER_OVERRIDE || id == (int)RuleConstants.RULE_MODIFIER_SEALED || id == (int)RuleConstants.RULE_MODIFIER_STATIC) {
                            int childIndex = 0;
                            foreach (Token childToken in nonterminalToken.Children) {
                                Reduction childNonterminalToken = childToken as Reduction;
                                TextToken terminalToken = childToken as TextToken;
                                if (terminalToken != null) {
                                    if (firstToken == null) {
                                        firstToken = terminalToken;
                                    } else {
                                        if (terminalToken.Position.Index < firstToken.Position.Index) {
                                            firstToken = terminalToken;
                                        }
                                    }

                                    result += terminalToken.Text;
                                } else if (childNonterminalToken != null) {
                                    int childChildIndex = 0;
                                    foreach (Token childChildToken in childNonterminalToken.Children) {
                                        TextToken childTerminalToken = childChildToken as TextToken;
                                        if (childTerminalToken != null) {
                                            if (firstToken == null) {
                                                firstToken = childTerminalToken;
                                            } else {
                                                if (childTerminalToken.Position.Index < firstToken.Position.Index) {
                                                    firstToken = childTerminalToken;
                                                }
                                            }

                                            result += childTerminalToken.Text;
                                            if (childChildIndex < childNonterminalToken.Children.Count - 1) {
                                                result += " ";
                                            }

                                            childChildIndex++;
                                        }
                                    }
                                }

                                if (childIndex < nonterminalToken.Children.Count - 1) {
                                    result += " ";
                                }

                                childIndex++;
                            }

                            if (index < modifiers.Children.Count - 1) {
                                result += " ";
                            }
                        } else if (id == (int)RuleConstants.RULE_MODIFIERS || id == (int)RuleConstants.RULE_MODIFIERS2) {
                            result += GetModifiers(nonterminalToken, out firstToken, true);
                            if (index < modifiers.Children.Count - 1) {
                                result += " ";
                            }
                        }
                    }

                    index++;
                }
            }

            return result;
        }

        private string AddRankSpecifiersToQualifiedId(string qualifiedId, IList<Token> tokens, int currentIndex, ref int endTypeTokenIndex) {
            string newQualifiedId = qualifiedId;
            if (tokens.Count > currentIndex + 1) {
                Reduction suspectedRankSpecifierToken = tokens[currentIndex + 1] as Reduction;
                if (suspectedRankSpecifierToken != null) {
                    endTypeTokenIndex++;
                    foreach (Token token in suspectedRankSpecifierToken.Children) {
                        Reduction rankSpecifierToken = token as Reduction;
                        if (rankSpecifierToken != null) {
                            foreach (Token childToken in rankSpecifierToken.Children) {
                                TextToken terminalToken = childToken as TextToken;
                                if (terminalToken != null) {
                                    newQualifiedId += terminalToken.Text;
                                }
                            }
                        }
                    }
                }
            }

            return newQualifiedId;
        }

        private GrapeExpression GetTypeFromTypeToken(IList<Token> tokens, GrapeEntity parent) {
            GrapeExpression expression = null;
            int currentIndex = 0;
            foreach (Token token in tokens) {
                Reduction nonterminalToken = token as Reduction;
                if (nonterminalToken != null && ((nonterminalToken.Symbol.Index == (int)RuleConstants.RULE_TYPE || nonterminalToken.Symbol.Index == (int)RuleConstants.RULE_TYPE2) || nonterminalToken.Symbol.Index == (int)RuleConstants.RULE_QUALIFIEDID)) {
                    if (currentIndex + 1 < tokens.Count && tokens[currentIndex + 1] is Reduction && ((Reduction)tokens[currentIndex + 1]).Symbol.Index == (int)RuleConstants.RULE_RANKSPECIFIERS) {
                        expression = new GrapeArrayAccessExpression();
                        expression.FileName = currentFileName;
                        GrapeArrayAccessExpression arrayExpression = expression as GrapeArrayAccessExpression;
                        arrayExpression.Member = CreateExpression(tokens[currentIndex] as Reduction);
                        if (arrayExpression.Member != null) {
                            arrayExpression.Member.FileName = currentFileName;
                        }

                        Reduction rankSpecifierToken = (tokens[currentIndex + 1] as Reduction).Children[0] as Reduction;
                        Reduction arrayExpressionToken = rankSpecifierToken.Children[1] as Reduction;
                        arrayExpression.Array = CreateExpression(arrayExpressionToken);
                    } else if (nonterminalToken.Symbol.Index == (int)RuleConstants.RULE_TYPE || nonterminalToken.Symbol.Index == (int)RuleConstants.RULE_TYPE2) {
                        Reduction nonArrayToken = nonterminalToken.Children[0] as Reduction;
                        Reduction qualifiedIdToken = nonArrayToken.Children[0] as Reduction;
                        if (nonterminalToken.Children.Count > 1 && nonterminalToken.Children[1] is Reduction && ((Reduction)nonterminalToken.Children[1]).Symbol.Index == (int)RuleConstants.RULE_RANKSPECIFIERS) {
                            expression = new GrapeArrayAccessExpression();
                            expression.FileName = currentFileName;
                            GrapeArrayAccessExpression arrayExpression = expression as GrapeArrayAccessExpression;
                            arrayExpression.Member = CreateExpression(qualifiedIdToken);
                            if (arrayExpression.Member != null) {
                                arrayExpression.Member.FileName = currentFileName;
                            }

                            Reduction rankSpecifierToken = (nonterminalToken.Children[1] as Reduction).Children[0] as Reduction;
                            Reduction arrayExpressionToken = rankSpecifierToken.Children[1] as Reduction;
                            arrayExpression.Array = CreateExpression(arrayExpressionToken);
                        } else {
                            expression = CreateExpression(qualifiedIdToken);
                        }
                    } else {
                        expression = CreateExpression(nonterminalToken);
                    }
                }

                currentIndex++;
            }

            if (expression != null) {
                expression.FileName = currentFileName;
                expression.Parent = parent;
            }

            return expression;
        }

        private TextToken GetEndToken(IList<Token> tokens) {
            foreach (Token token in tokens) {
                if (token is TextToken && ((TextToken)token).Text == "end") {
                    return token as TextToken;
                }
            }

            return null;
        }

        private void AddMethodCallsToExpression(GrapeMemberExpression expression, Reduction token) {
            foreach (Token childToken in token.Children) {
                if (childToken is Reduction && (((Reduction)childToken).Symbol.Index == (int)RuleConstants.RULE_METHODCALLS || ((Reduction)childToken).Symbol.Index == (int)RuleConstants.RULE_METHODCALLS2)) {
                    Reduction nonterminalToken = childToken as Reduction;
                    GrapeMemberExpression nextExpression = new GrapeMemberExpression();
                    GrapeIdentifierExpression identifierExpression = new GrapeIdentifierExpression();
                    identifierExpression.FileName = currentFileName;
                    processedExpressionTokens.Add(nonterminalToken);
                    if (nonterminalToken.Children.Count > 0) {
                        Reduction methodCallToken = nonterminalToken.Children[0] as Reduction;
                        TextToken identifierToken = methodCallToken.Children[0] as TextToken;
                        identifierExpression.Identifier = identifierToken.Text.Trim('.');
                        if (methodCallToken.Children.Count > 1) {
                            TextToken bracketToken = methodCallToken.Children[1] as TextToken;
                            if (bracketToken != null) {
                                if (bracketToken.Text == "[") {
                                    GrapeArrayAccessExpression arrayExpression = new GrapeArrayAccessExpression();
                                    arrayExpression.FileName = currentFileName;
                                    Reduction expressionToken = methodCallToken.Children[2] as Reduction;
                                    arrayExpression.Array = CreateExpression(expressionToken);
                                    nextExpression = arrayExpression;
                                } else if (bracketToken.Text == "(") {
                                    GrapeCallExpression callExpression = new GrapeCallExpression();
                                    callExpression.FileName = currentFileName;
                                    Reduction argListOptToken = methodCallToken.Children[2] as Reduction;
                                    if (argListOptToken != null && argListOptToken.Children.Count > 0) {
                                        Reduction argListToken = argListOptToken.Children[0] as Reduction;
                                        foreach (Token argNormalToken in argListToken.Children) {
                                            Reduction argToken = argNormalToken as Reduction;
                                            if (argToken != null && argToken.Children.Count > 0) {
                                                Reduction argumentToken = argToken.Children[0] as Reduction;
                                                if (argumentToken != null && argumentToken.Children.Count > 0) {
                                                    Reduction argExpressionToken = argumentToken.Children[0] as Reduction;
                                                    callExpression.Parameters.Add(CreateExpression(argExpressionToken));
                                                }
                                            }
                                        }
                                    }

                                    nextExpression = callExpression;
                                }
                            }
                        }

                        nextExpression.Member = identifierExpression;
                        expression.Next = nextExpression;
                        AddMethodCallsToExpression(nextExpression, nonterminalToken);
                    }
                }
            }
        }

        private TextToken GetFirstTerminalToken(IList<Token> tokens) {
            foreach (Token token in tokens) {
                if (token is TextToken) {
                    return token as TextToken;
                } else if (token is Reduction) {
                    TextToken recursiveToken = GetFirstTerminalToken((token as Reduction).Children);
                    if (recursiveToken != null) {
                        return recursiveToken;
                    }
                }
            }

            return null;
        }

        private TextToken GetLastTerminalToken(IList<Token> tokens) {
            for (int index = tokens.Count - 1; index >= 0; index--) {
                Token token = tokens[index];
                if (token is TextToken) {
                    return token as TextToken;
                } else if (token is Reduction) {
                    TextToken recursiveToken = GetFirstTerminalToken((token as Reduction).Children);
                    if (recursiveToken != null) {
                        return recursiveToken;
                    }
                }
            }

            return null;
        }

        private GrapeExpression CreateExpression(Reduction token) {
            TextToken dummyToken = null;
            return CreateExpression(token, ref dummyToken);
        }

        private GrapeExpression CreateExpression(Reduction token, ref TextToken lastToken) {
            GrapeExpression expression = null;
            GrapeConditionalExpression conditionalExpression = null;
            GrapeUnaryExpression unaryExpression = null;
            GrapeAddExpression addExpression = null;
            GrapeMultiplicationExpression multiplicationExpression = null;
            GrapeShiftExpression shiftExpression = null;
            GrapeLiteralExpression literalExpression = null;
            GrapeCallExpression callExpression = null;
            GrapeArrayAccessExpression arrayExpression = null;
            GrapeTypecastExpression typecastExpression = null;
            GrapeMemberExpression memberExpression = null;
            GrapeStackExpression stackExpression = null;
            GrapeNameofExpression nameofExpression = null;
            if (token != null) {
                processedExpressionTokens.Add(token);
                if (token.Children.Count > 1 || (token.Children.Count == 1 && token.Children[0] is TextToken)) {
                    switch (token.Symbol.Index) {
                        // Expression in brackets: ((expression))
                        case (int)RuleConstants.RULE_VALUE_LPARAN_RPARAN:
                            expression = new GrapeStackExpression();
                            expression.FileName = currentFileName;
                            stackExpression = expression as GrapeStackExpression;
                            stackExpression.Child = CreateExpression(token.Children[1] as Reduction);
                            break;
                        // General statements (method calling, member accessing, variable accessing, etc.).
                        case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN2:
                        case (int)RuleConstants.RULE_VALUE_NEW_LPARAN_RPARAN:
                            int startIndex = 0;
                            if (token.Symbol.Index == (int)RuleConstants.RULE_VALUE_NEW_LPARAN_RPARAN) {
                                startIndex = 1;
                                callExpression = new GrapeObjectCreationExpression();
                            } else {
                                callExpression = new GrapeCallExpression();
                            }

                            callExpression.FileName = currentFileName;
                            memberExpression = CreateExpression(token.Children[startIndex] as Reduction, ref lastToken) as GrapeMemberExpression;
                            if (memberExpression != null) {
                                memberExpression.Parent = callExpression;
                                memberExpression.FileName = currentFileName;
                                GrapeMemberExpression actualMember = memberExpression;
                                if (actualMember.Member != null && actualMember.Member is GrapeMemberExpression) {
                                    actualMember = actualMember.Member as GrapeMemberExpression;
                                }

                                GrapeMemberExpression lastMember = null;
                                while (actualMember.Next != null) {
                                    lastMember = actualMember;
                                    actualMember = actualMember.Next;
                                    actualMember.FileName = currentFileName;
                                }

                                callExpression.Member = actualMember;
                                actualMember.Parent = callExpression;
                                int lastTokenIndex;
                                Reduction parametersToken = token.Children[startIndex + 2] as Reduction;
                                if (parametersToken != null) {
                                    AddParametersToCallExpression(callExpression, parametersToken, 0, out lastTokenIndex);
                                }

                                AddMethodCallsToExpression(callExpression, token);
                                if (lastMember != null) {
                                    expression = lastMember;
                                    lastMember.Next = callExpression;
                                } else {
                                    expression = callExpression;
                                }

                                if (token.Children.Count > startIndex + 5) {
                                    Reduction assignTailToken = token.Children[startIndex + 5] as Reduction;
                                    if (assignTailToken != null && assignTailToken.Symbol.Index == (int)RuleConstants.RULE_ASSIGNTAIL_EQ) {
                                        processedExpressionTokens.Add(assignTailToken);
                                        GrapeSetExpression setExpression = new GrapeSetExpression();
                                        setExpression.FileName = currentFileName;
                                        setExpression.Member = callExpression;
                                        setExpression.Value = CreateExpression(assignTailToken.Children[1] as Reduction, ref lastToken);
                                        expression = setExpression;
                                    }
                                }
                            }

                            break;
                        case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET2:
                            startIndex = 0;
                            arrayExpression = new GrapeArrayAccessExpression();
                            memberExpression = CreateExpression(token.Children[startIndex] as Reduction, ref lastToken) as GrapeMemberExpression;
                            arrayExpression.FileName = currentFileName;
                            if (memberExpression != null) {
                                memberExpression.FileName = currentFileName;
                                arrayExpression.Member = memberExpression.Member;
                                arrayExpression.Array = CreateExpression(token.Children[startIndex + 2] as Reduction, ref lastToken);
                                AddMethodCallsToExpression(arrayExpression, token);
                                expression = arrayExpression;
                                if (token.Children.Count > 5) {
                                    Reduction assignTailToken = token.Children[startIndex + 5] as Reduction;
                                    if (assignTailToken != null && assignTailToken.Symbol.Index == (int)RuleConstants.RULE_ASSIGNTAIL_EQ) {
                                        processedExpressionTokens.Add(assignTailToken);
                                        GrapeSetExpression setExpression = new GrapeSetExpression();
                                        setExpression.FileName = currentFileName;
                                        setExpression.Member = arrayExpression;
                                        setExpression.Value = CreateExpression(assignTailToken.Children[1] as Reduction, ref lastToken);
                                        expression = setExpression;
                                    }
                                }
                            }

                            break;
                        case (int)RuleConstants.RULE_STATEMENTEXP:
                        case (int)RuleConstants.RULE_STATEMENTEXP2:
                        case (int)RuleConstants.RULE_STATEMENTEXP3:
                            Reduction qualifiedIdToken = token.Children[0] as Reduction;
                            GrapeMemberExpression member = CreateExpression(qualifiedIdToken, ref lastToken) as GrapeMemberExpression;
                            expression = member;
                            expression.FileName = currentFileName;
                            if (expression != null) {
                                AddMethodCallsToExpression(expression as GrapeMemberExpression, token);
                                if (token.Children.Count > 2) {
                                    Reduction assignTailToken = token.Children[2] as Reduction;
                                    if (assignTailToken != null && assignTailToken.Symbol.Index == (int)RuleConstants.RULE_ASSIGNTAIL_EQ) {
                                        processedExpressionTokens.Add(assignTailToken);
                                        GrapeSetExpression setExpression = new GrapeSetExpression();
                                        setExpression.Member = member;
                                        setExpression.FileName = currentFileName;
                                        setExpression.Value = CreateExpression(assignTailToken.Children[1] as Reduction, ref lastToken);
                                        expression = setExpression;
                                    }
                                }
                            }

                            break;
                        // Represents a nameof expression. nameof((qualified ID))
                        case (int)RuleConstants.RULE_VALUE_NAMEOF_LPARAN_RPARAN:
                            expression = new GrapeNameofExpression();
                            expression.FileName = currentFileName;
                            nameofExpression = expression as GrapeNameofExpression;
                            Reduction valueToken = token.Children[2] as Reduction;
                            if (valueToken != null) {
                                nameofExpression.Value = CreateExpression(valueToken, ref lastToken);
                            }

                            break;
                        // Represents a member access expression. (identifier).(identifier)
                        case (int)RuleConstants.RULE_QUALIFIEDID:
                            expression = new GrapeMemberExpression();
                            expression.FileName = currentFileName;
                            memberExpression = expression as GrapeMemberExpression;
                            GrapeMemberExpression firstMember = new GrapeMemberExpression();
                            GrapeMemberExpression currentMember = firstMember;
                            string qualifiedIdText = GetQualifiedIdText(token, out lastToken);
                            string[] identifiers = qualifiedIdText.Split('.');
                            foreach (string identifier in identifiers) {
                                GrapeIdentifierExpression identifierExpression = new GrapeIdentifierExpression();
                                identifierExpression.Identifier = identifier;
                                identifierExpression.FileName = currentFileName;
                                currentMember.Member = identifierExpression;
                                GrapeMemberExpression oldCurrentMember = currentMember;
                                currentMember = new GrapeMemberExpression();
                                currentMember.FileName = currentFileName;
                                oldCurrentMember.Next = currentMember;
                            }

                            memberExpression.Member = firstMember;
                            currentMember = firstMember;
                            while (currentMember.Next.Member != null) {
                                currentMember = currentMember.Next;
                            }

                            currentMember.Next = null;
                            break;
                        // Typecasting: (expression) as (qualified ID).
                        case (int)RuleConstants.RULE_TYPECASTEXP_AS:
                            expression = new GrapeTypecastExpression();
                            expression.FileName = currentFileName;
                            typecastExpression = expression as GrapeTypecastExpression;
                            typecastExpression.Value = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            typecastExpression.Type = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            break;
                        // Literal expressions (hexadecimals, integers, reals, strings, true, false and null).
                        case (int)RuleConstants.RULE_LITERAL_DECIMALLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Int;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_HEXLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Hexadecimal;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_REALLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Real;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_STRINGLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.String;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_FALSE:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.False;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_TRUE:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.True;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_NULL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Children[0] as TextToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Null;
                            break;
                        // Conditional expressions, such as (expression) && (expression), (expression) ^ (expression), etc.
                        case (int)RuleConstants.RULE_ANDEXP:
                        case (int)RuleConstants.RULE_ANDEXP_AMPAMP:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LogicalAnd;
                            break;
                        case (int)RuleConstants.RULE_OREXP:
                        case (int)RuleConstants.RULE_OREXP_PIPEPIPE:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LogicalOr;
                            break;
                        case (int)RuleConstants.RULE_LOGICALANDEXP:
                        case (int)RuleConstants.RULE_LOGICALANDEXP_AMP:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryAnd;
                            break;
                        case (int)RuleConstants.RULE_LOGICALOREXP:
                        case (int)RuleConstants.RULE_LOGICALOREXP_PIPE:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryOr;
                            break;
                        case (int)RuleConstants.RULE_LOGICALXOREXP:
                        case (int)RuleConstants.RULE_LOGICALXOREXP_CARET:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LogicalXor;
                            break;
                        // Equal and not equal expressions: (expression) == (expression), (expression) != (expression).
                        case (int)RuleConstants.RULE_EQUALITYEXP_EQEQ:
                        case (int)RuleConstants.RULE_EQUALITYEXP_EXCLAMEQ:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            if (token.Symbol.Index == (int)RuleConstants.RULE_EQUALITYEXP_EQEQ) {
                                conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.Equal;
                            } else {
                                conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.NotEqual;
                            }

                            break;
                        // Compare expressions: (expression) < (expression), (expression) > (expression), (expression) <= (expression), (expression) >= (expression).
                        case (int)RuleConstants.RULE_COMPAREEXP_GT:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.GreaterThan;
                            break;
                        case (int)RuleConstants.RULE_COMPAREEXP_GTEQ:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.GreaterThanOrEqual;
                            break;
                        case (int)RuleConstants.RULE_COMPAREEXP_LT:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LessThan;
                            break;
                        case (int)RuleConstants.RULE_COMPAREEXP_LTEQ:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LessThanOrEqual;
                            break;
                        // Mathematical expressions, e.g (expression) + (expression), (expression) % (expression), ~(expression), etc.
                        // Addition and subtraction expressions: (expression) + (expression), (expression) - (expression).
                        case (int)RuleConstants.RULE_ADDEXP_PLUS:
                        case (int)RuleConstants.RULE_ADDEXP_MINUS:
                            expression = new GrapeAddExpression();
                            expression.FileName = currentFileName;
                            addExpression = expression as GrapeAddExpression;
                            addExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            addExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            addExpression.Type = token.Symbol.Index == (int)RuleConstants.RULE_ADDEXP_MINUS ? GrapeAddExpression.GrapeAddExpressionType.Subtraction : GrapeAddExpression.GrapeAddExpressionType.Addition;
                            break;
                        // Multiplication expressions: (expression) * (expression), (expression) / (expression), (expression) % (expression).
                        case (int)RuleConstants.RULE_MULTEXP_DIV:
                            expression = new GrapeMultiplicationExpression();
                            expression.FileName = currentFileName;
                            multiplicationExpression = expression as GrapeMultiplicationExpression;
                            multiplicationExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            multiplicationExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            multiplicationExpression.Type = GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Division;
                            break;
                        case (int)RuleConstants.RULE_MULTEXP_PERCENT:
                            expression = new GrapeMultiplicationExpression();
                            expression.FileName = currentFileName;
                            multiplicationExpression = expression as GrapeMultiplicationExpression;
                            multiplicationExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            multiplicationExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            multiplicationExpression.Type = GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Mod;
                            break;
                        case (int)RuleConstants.RULE_MULTEXP_TIMES:
                            expression = new GrapeMultiplicationExpression();
                            expression.FileName = currentFileName;
                            multiplicationExpression = expression as GrapeMultiplicationExpression;
                            multiplicationExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            multiplicationExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            multiplicationExpression.Type = GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Multiplication;
                            break;
                        // Shift expressions: (expression) << (expression), (expression) >> (expression).
                        case (int)RuleConstants.RULE_SHIFTEXP_GTGT:
                            expression = new GrapeShiftExpression();
                            expression.FileName = currentFileName;
                            shiftExpression = expression as GrapeShiftExpression;
                            shiftExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            shiftExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            shiftExpression.Type = GrapeShiftExpression.GrapeShiftExpressionType.ShiftRight;
                            break;
                        case (int)RuleConstants.RULE_SHIFTEXP_LTLT:
                            expression = new GrapeShiftExpression();
                            expression.FileName = currentFileName;
                            shiftExpression = expression as GrapeShiftExpression;
                            shiftExpression.Left = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                            shiftExpression.Right = CreateExpression(token.Children[2] as Reduction, ref lastToken);
                            shiftExpression.Type = GrapeShiftExpression.GrapeShiftExpressionType.ShiftLeft;
                            break;
                        // Unary expressions: !(expression), -(expression), ~(expression).
                        case (int)RuleConstants.RULE_UNARYEXP_EXCLAM:
                            expression = new GrapeUnaryExpression();
                            expression.FileName = currentFileName;
                            unaryExpression = expression as GrapeUnaryExpression;
                            unaryExpression.Type = GrapeUnaryExpression.GrapeUnaryExpressionType.Not;
                            unaryExpression.Value = CreateExpression(token.Children[1] as Reduction, ref lastToken);
                            break;
                        case (int)RuleConstants.RULE_UNARYEXP_MINUS:
                            expression = new GrapeUnaryExpression();
                            expression.FileName = currentFileName;
                            unaryExpression = expression as GrapeUnaryExpression;
                            unaryExpression.Type = GrapeUnaryExpression.GrapeUnaryExpressionType.Negate;
                            unaryExpression.Value = CreateExpression(token.Children[1] as Reduction, ref lastToken);
                            break;
                        case (int)RuleConstants.RULE_UNARYEXP_TILDE:
                            expression = new GrapeUnaryExpression();
                            expression.FileName = currentFileName;
                            unaryExpression = expression as GrapeUnaryExpression;
                            unaryExpression.Type = GrapeUnaryExpression.GrapeUnaryExpressionType.CurlyEqual;
                            unaryExpression.Value = CreateExpression(token.Children[1] as Reduction, ref lastToken);
                            break;
                    }
                } else {
                    expression = CreateExpression(token.Children[0] as Reduction, ref lastToken);
                }
            }

            if (conditionalExpression != null) {
                conditionalExpression.Left.Parent = conditionalExpression;
                conditionalExpression.Right.Parent = conditionalExpression;
            } else if (addExpression != null) {
                addExpression.Left.Parent = addExpression;
                addExpression.Right.Parent = addExpression;
            } else if (multiplicationExpression != null) {
                multiplicationExpression.Left.Parent = multiplicationExpression;
                multiplicationExpression.Right.Parent = multiplicationExpression;
            } else if (shiftExpression != null) {
                shiftExpression.Left.Parent = shiftExpression;
                shiftExpression.Right.Parent = shiftExpression;
            } else if (unaryExpression != null) {
                unaryExpression.Value.Parent = unaryExpression;
            }

            if (expression != null) {
                expression.FileName = currentFileName;
                TextToken firstTerminalToken = GetFirstTerminalToken(token.Children);
                TextToken lastTerminalToken = GetLastTerminalToken(token.Children);
                if (firstTerminalToken != null && lastTerminalToken != null) {
                    expression.StartLine = firstTerminalToken.Position.Line;
                    expression.StartColumn = firstTerminalToken.Position.Column;
                    expression.EndLine = lastTerminalToken.Position.Line;
                    expression.EndColumn = lastTerminalToken.Position.Column;
                    expression.Offset = firstTerminalToken.Position.Index;
                    expression.Length = lastTerminalToken.Position.Index - firstTerminalToken.Position.Index;
                }
            }

            return expression;
        }

        public enum GrapeStatementType {
            GrapeForEachStatement,
            GrapeIfStatement,
            GrapeElseStatement,
            GrapeElseIfStatement,
            GrapeTryStatement,
            GrapeCatchStatement,
            GrapeFinallyStatement,
            GrapeSwitchStatement,
            GrapeCaseStatement,
            GrapeDefaultStatement,
            GrapeWhileStatement,
            GrapeBreakStatement,
            GrapeContinueStatement,
            GrapeReturnStatement,
            GrapeThrowStatement,
            GrapeInitStatement,
            GrapeDeleteStatement
        }

        private GrapeStatement CreateStatement(Reduction token, GrapeStatementType statementType) {
            TextToken dummyToken = null;
            return CreateStatement(token, statementType, ref dummyToken);
        }

        private GrapeIfStatement currentIfStatement;
        private GrapeSwitchStatement currentSwitchStatement;
        private GrapeTryStatement currentTryStatement;
    	private CompiledGrammar grammar;

    	private GrapeStatement CreateStatement(Reduction token, GrapeStatementType statementType, ref TextToken lastToken) {
            GrapeStatement statement = null;
            GrapeForEachStatement forEachStatement = null;
            GrapeIfStatement ifStatement = null;
            GrapeElseStatement elseStatement = null;
            GrapeElseIfStatement elseIfStatement = null;
            GrapeWhileStatement whileStatement = null;
            GrapeSwitchStatement switchStatement = null;
            GrapeCaseStatement caseStatement = null;
            GrapeDefaultStatement defaultStatement = null;
            GrapeTryStatement tryStatement = null;
            GrapeCatchStatement catchStatement = null;
            GrapeFinallyStatement finallyStatement = null;
            GrapeReturnStatement returnStatement = null;
            GrapeThrowStatement throwStatement = null;
            GrapeInitStatement initStatement = null;
            GrapeDeleteStatement deleteStatement = null;
            TextToken blockFirstToken = null;
            TextToken blockLastToken = null;
            Reduction conditionToken = null;
            Reduction expressionToken = null;
            int currentIndex = 0;
            int startIndex = 0;
            int index = 1;
            switch (statementType) {
                case GrapeStatementType.GrapeForEachStatement:
                    statement = new GrapeForEachStatement();
                    forEachStatement = statement as GrapeForEachStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = GetEndToken(token.Children);
                    index = 1;
                    while (token.Children[index] is TextToken) {
                        index++;
                    }

                    startIndex = index - 1;
                    GrapeVariable v = new GrapeVariable();
                    GrapeExpression type = GetTypeFromTypeToken(token.Children, v);
                    v.Type = type;
                    TextToken nameToken = token.Children[startIndex + 1] as TextToken;
                    if (nameToken != null) {
                        v.Name = nameToken.Text;
                    }

                    forEachStatement.Variable = v;
                    Reduction containerExpressionToken = token.Children[startIndex + 3] as Reduction;
                    if (containerExpressionToken != null) {
                        forEachStatement.ContainerExpression = CreateExpression(containerExpressionToken);
                        forEachStatement.ContainerExpression.Parent = forEachStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    currentEntityWithBlock = forEachStatement;
                    AddGrapeEntityToCurrentParent(statement);
                    CreateBlock(token);
                    break;
                case GrapeStatementType.GrapeIfStatement:
                    statement = new GrapeIfStatement();
                    ifStatement = statement as GrapeIfStatement;
                    currentIfStatement = ifStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = GetEndToken(token.Children);
                    conditionToken = token.Children[1] as Reduction;
                    if (conditionToken == null && token.Children[1] is TextToken && ((TextToken)token.Children[1]).Text == "(") {
                        conditionToken = token.Children[2] as Reduction;
                    }

                    if (conditionToken != null) {
                        ifStatement.Condition = CreateExpression(conditionToken);
                        ifStatement.Condition.Parent = ifStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    currentEntityWithBlock = ifStatement;
                    AddGrapeEntityToCurrentParent(statement);
                    CreateBlock(token);
                    break;
                case GrapeStatementType.GrapeElseIfStatement:
                    if (currentIfStatement != null) {
                        statement = new GrapeElseIfStatement();
                        elseIfStatement = statement as GrapeElseIfStatement;
                        elseIfStatement.IfStatement = currentIfStatement;
                        currentIfStatement.ElseIfStatements.Add(elseIfStatement);
                        blockFirstToken = token.Children[0] as TextToken;
                        blockLastToken = GetEndToken(token.Children);
                        conditionToken = token.Children[1] as Reduction;
                        if (conditionToken == null && token.Children[1] is TextToken && ((TextToken)token.Children[1]).Text == "(") {
                            conditionToken = token.Children[2] as Reduction;
                        }

                        if (conditionToken != null) {
                            elseIfStatement.Condition = CreateExpression(conditionToken);
                            elseIfStatement.Condition.Parent = elseIfStatement;
                        }

                        if (blockFirstToken != null && blockLastToken != null) {
                            statement.Offset = blockFirstToken.Position.Index;
                            statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                            statement.StartLine = blockFirstToken.Position.Line;
                            statement.StartColumn = blockFirstToken.Position.Column;
                            statement.EndLine = blockLastToken.Position.Line;
                            statement.EndColumn = blockLastToken.Position.Column;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = elseIfStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "An elseif statement must have an if statement as a parent.", FileName = currentFileName, Entity = statement });
                    }

                    break;
                case GrapeStatementType.GrapeElseStatement:
                    if (currentIfStatement != null) {
                        statement = new GrapeElseStatement();
                        elseStatement = statement as GrapeElseStatement;
                        elseStatement.IfStatement = currentIfStatement;
                        currentIfStatement.ElseStatements.Add(elseStatement);
                        blockFirstToken = token.Children[0] as TextToken;
                        blockLastToken = GetEndToken(token.Children);
                        if (blockFirstToken != null && blockLastToken != null) {
                            statement.Offset = blockFirstToken.Position.Index;
                            statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                            statement.StartLine = blockFirstToken.Position.Line;
                            statement.StartColumn = blockFirstToken.Position.Column;
                            statement.EndLine = blockLastToken.Position.Line;
                            statement.EndColumn = blockLastToken.Position.Column;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = elseStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                        if (currentIfStatement.ElseStatements.Count > 1) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "An if statement can only have one else statement.", FileName = currentFileName, Entity = statement });
                        }
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "An else statement must have an if statement as a parent.", FileName = currentFileName, Entity = statement });
                    }

                    break;
                case GrapeStatementType.GrapeSwitchStatement:
                    statement = new GrapeSwitchStatement();
                    switchStatement = statement as GrapeSwitchStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = GetEndToken(token.Children);
                    currentSwitchStatement = switchStatement;
                    if (token.Children.Count >= 1) {
                        currentIndex = 0;
                        expressionToken = null;
                        while (currentIndex < token.Children.Count) {
                            if (token.Children[currentIndex] is Reduction && (((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                expressionToken = token.Children[currentIndex] as Reduction;
                                break;
                            }

                            currentIndex++;
                        }

                        if (expressionToken != null) {
                            switchStatement.SwitchTarget = CreateExpression(expressionToken);
                            switchStatement.SwitchTarget.Parent = switchStatement;
                        }
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    currentEntityWithBlock = switchStatement;
                    AddGrapeEntityToCurrentParent(statement);
                    CreateBlock(token);
                    break;
                case GrapeStatementType.GrapeCaseStatement:
                    if (currentSwitchStatement != null) {
                        statement = new GrapeCaseStatement();
                        caseStatement = statement as GrapeCaseStatement;
                        caseStatement.SwitchStatement = currentSwitchStatement;
                        blockFirstToken = token.Children[0] as TextToken;
                        blockLastToken = GetEndToken(token.Children);
                        if (token.Children.Count >= 1) {
                            currentIndex = 0;
                            expressionToken = null;
                            while (currentIndex < token.Children.Count) {
                                if (token.Children[currentIndex] is Reduction && (((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                    expressionToken = token.Children[currentIndex] as Reduction;
                                    break;
                                }

                                currentIndex++;
                            }

                            if (expressionToken != null) {
                                caseStatement.CaseValue = CreateExpression(expressionToken);
                                caseStatement.CaseValue.Parent = caseStatement;
                            }
                        }

                        if (blockFirstToken != null && blockLastToken != null) {
                            statement.Offset = blockFirstToken.Position.Index;
                            statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                            statement.StartLine = blockFirstToken.Position.Line;
                            statement.StartColumn = blockFirstToken.Position.Column;
                            statement.EndLine = blockLastToken.Position.Line;
                            statement.EndColumn = blockLastToken.Position.Column;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = caseStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A case statement must have a switch statement as a parent.", FileName = currentFileName, Entity = statement });
                    }

                    break;
                case GrapeStatementType.GrapeDefaultStatement:
                    if (currentSwitchStatement != null) {
                        statement = new GrapeDefaultStatement();
                        defaultStatement = statement as GrapeDefaultStatement;
                        defaultStatement.SwitchStatement = currentSwitchStatement;
                        currentSwitchStatement.DefaultStatement = defaultStatement;
                        blockFirstToken = token.Children[0] as TextToken;
                        blockLastToken = GetEndToken(token.Children);
                        if (blockFirstToken != null && blockLastToken != null) {
                            statement.Offset = blockFirstToken.Position.Index;
                            statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                            statement.StartLine = blockFirstToken.Position.Line;
                            statement.StartColumn = blockFirstToken.Position.Column;
                            statement.EndLine = blockLastToken.Position.Line;
                            statement.EndColumn = blockLastToken.Position.Column;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = defaultStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A default statement must have a switch statement as a parent.", FileName = currentFileName, Entity = statement });
                    }

                    break;
                case GrapeStatementType.GrapeTryStatement:
                    statement = new GrapeTryStatement();
                    tryStatement = statement as GrapeTryStatement;
                    currentTryStatement = tryStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = GetEndToken(token.Children);
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    currentEntityWithBlock = tryStatement;
                    AddGrapeEntityToCurrentParent(statement);
                    CreateBlock(token);
                    break;
                case GrapeStatementType.GrapeCatchStatement:
                    if (currentTryStatement != null) {
                        statement = new GrapeCatchStatement();
                        catchStatement = statement as GrapeCatchStatement;
                        catchStatement.TryStatement = currentTryStatement;
                        currentTryStatement.CatchStatements.Add(catchStatement);
                        blockFirstToken = token.Children[0] as TextToken;
                        blockLastToken = GetEndToken(token.Children);
                        index = 1;
                        while (token.Children[index] is TextToken) {
                            index++;
                        }

                        startIndex = index - 1;
                        if (token.Children.Count > 1) {
                            GrapeVariable catchVariable = new GrapeVariable();
                            GrapeExpression catchVariableType = GetTypeFromTypeToken(token.Children, catchVariable);
                            catchVariable.Type = catchVariableType;
                            if (token.Children.Count > startIndex + 1) {
                                TextToken catchVariableName = token.Children[startIndex + 1] as TextToken;
                                if (catchVariableName != null) {
                                    catchVariable.Name = catchVariableName.Text;
                                }
                            }

                            catchStatement.Variable = catchVariable;
                        }

                        if (blockFirstToken != null && blockLastToken != null) {
                            statement.Offset = blockFirstToken.Position.Index;
                            statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                            statement.StartLine = blockFirstToken.Position.Line;
                            statement.StartColumn = blockFirstToken.Position.Column;
                            statement.EndLine = blockLastToken.Position.Line;
                            statement.EndColumn = blockLastToken.Position.Column;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = catchStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A catch statement must have a try statement as a parent.", FileName = currentFileName, Entity = statement });
                    }

                    break;
                case GrapeStatementType.GrapeFinallyStatement:
                    if (currentTryStatement != null) {
                        statement = new GrapeFinallyStatement();
                        finallyStatement = statement as GrapeFinallyStatement;
                        if (currentTryStatement.FinallyStatement == null) {
                            finallyStatement.TryStatement = currentTryStatement;
                            currentTryStatement.FinallyStatement = finallyStatement;
                        }

                        blockFirstToken = token.Children[0] as TextToken;
                        blockLastToken = GetEndToken(token.Children);
                        if (blockFirstToken != null && blockLastToken != null) {
                            statement.Offset = blockFirstToken.Position.Index;
                            statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                            statement.StartLine = blockFirstToken.Position.Line;
                            statement.StartColumn = blockFirstToken.Position.Column;
                            statement.EndLine = blockLastToken.Position.Line;
                            statement.EndColumn = blockLastToken.Position.Column;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = finallyStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A finally statement must have a try statement as a parent.", FileName = currentFileName, Entity = statement });
                    }

                    break;
                case GrapeStatementType.GrapeWhileStatement:
                    statement = new GrapeWhileStatement();
                    whileStatement = statement as GrapeWhileStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = GetEndToken(token.Children);
                    currentIndex = 0;
                    expressionToken = null;
                    while (currentIndex < token.Children.Count) {
                        if (token.Children[currentIndex] is Reduction && (((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                            expressionToken = token.Children[currentIndex] as Reduction;
                            break;
                        }

                        currentIndex++;
                    }

                    if (expressionToken != null) {
                        whileStatement.Condition = CreateExpression(expressionToken);
                        whileStatement.Condition.Parent = whileStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    currentEntityWithBlock = whileStatement;
                    AddGrapeEntityToCurrentParent(statement);
                    CreateBlock(token);
                    break;
                case GrapeStatementType.GrapeBreakStatement:
                    statement = new GrapeBreakStatement();
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeContinueStatement:
                    statement = new GrapeContinueStatement();
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeReturnStatement:
                    statement = new GrapeReturnStatement();
                    returnStatement = statement as GrapeReturnStatement;
                    if (token.Children.Count > 1) {
                        currentIndex = 0;
                        expressionToken = null;
                        while (currentIndex < token.Children.Count) {
                            if (token.Children[currentIndex] is Reduction && (((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                expressionToken = token.Children[currentIndex] as Reduction;
                                break;
                            }

                            currentIndex++;
                        }

                        if (expressionToken != null) {
                            returnStatement.ReturnValue = CreateExpression(expressionToken);
                            returnStatement.ReturnValue.Parent = returnStatement;
                        }
                    }

                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeThrowStatement:
                    statement = new GrapeThrowStatement();
                    throwStatement = statement as GrapeThrowStatement;
                    if (token.Children.Count > 1) {
                        currentIndex = 0;
                        expressionToken = null;
                        while (currentIndex < token.Children.Count) {
                            if (token.Children[currentIndex] is Reduction && (((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((Reduction)token.Children[currentIndex]).Symbol.Index == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                expressionToken = token.Children[currentIndex] as Reduction;
                                break;
                            }

                            currentIndex++;
                        }

                        if (expressionToken != null) {
                            throwStatement.ThrowExpression = CreateExpression(expressionToken);
                            throwStatement.ThrowExpression.Parent = throwStatement;
                        }
                    }

                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeInitStatement:
                    statement = new GrapeInitStatement();
                    initStatement = statement as GrapeInitStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = blockFirstToken;
                    TextToken initToken = token.Children[1] as TextToken;
                    if (initToken != null) {
                        initStatement.Type = GrapeInitStatement.GrapeInitStatementType.Unknown;
                        if (initToken.Text == "base") {
                            initStatement.Type = GrapeInitStatement.GrapeInitStatementType.Base;
                        } else if (initToken.Text == "this") {
                            initStatement.Type = GrapeInitStatement.GrapeInitStatementType.This;
                        }
                    }

                    int lastTokenIndex;
                    Reduction parametersToken = token.Children[3] as Reduction;
                    if (parametersToken != null) {
                        AddParametersToFunction(initStatement, parametersToken, 0, out lastTokenIndex);
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeDeleteStatement:
                    statement = new GrapeDeleteStatement();
                    deleteStatement = statement as GrapeDeleteStatement;
                    blockFirstToken = token.Children[0] as TextToken;
                    blockLastToken = GetEndToken(token.Children);
                    Reduction valueToken = token.Children[1] as Reduction;
                    if (valueToken != null) {
                        deleteStatement.Value = CreateExpression(valueToken, ref blockLastToken);
                        deleteStatement.Value.Parent = deleteStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Position.Index;
                        statement.Length = blockLastToken.Position.Index - blockFirstToken.Position.Index;
                        statement.StartLine = blockFirstToken.Position.Line;
                        statement.StartColumn = blockFirstToken.Position.Column;
                        statement.EndLine = blockLastToken.Position.Line;
                        statement.EndColumn = blockLastToken.Position.Column;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
            }

            return statement;
        }

        private void CreateVariable(Reduction token) {
            if (token.Children.Count >= 4) {
                GrapeVariable v;
                GrapeField f = null;
                TextToken firstToken = null;
                TextToken lastToken = null;
                Reduction modifiers = token.Children[0] as Reduction;
                if ((modifiers != null && modifiers.Symbol.Index != (int)RuleConstants.RULE_QUALIFIEDID && modifiers.Children.Count > 0) || currentBlock.Parent is GrapeClass) {
                    string modifiersText = GetModifiers(modifiers, out firstToken);
                    if (!string.IsNullOrEmpty(modifiersText) || currentBlock.Parent is GrapeClass) {
                        v = new GrapeField();
                        f = v as GrapeField;
                        f.Modifiers = modifiersText;
                    } else {
                        v = new GrapeVariable();
                    }
                } else {
                    v = new GrapeVariable();
                }

                int nameTokenIndex = 2;
                GrapeExpression type = GetTypeFromTypeToken(token.Children, v);
                v.Type = type;
                if (nameTokenIndex >= token.Children.Count) {
                    nameTokenIndex = 1;
                }

                TextToken nameToken = token.Children[nameTokenIndex] as TextToken;
                if (nameToken.Text == "=") {
                    nameTokenIndex--;
                    nameToken = token.Children[nameTokenIndex] as TextToken;
                }

                if (nameToken != null) {
                    v.Name = nameToken.Text;
                    firstToken = nameToken;
                }

                TextToken endToken = nameToken;
                if (token.Children.Count > nameTokenIndex + 1) {
                    TextToken initializerToken = token.Children[nameTokenIndex + 1] as TextToken;
                    if (initializerToken != null && initializerToken.Text == "=") {
                        Reduction valueExpressionToken = token.Children[nameTokenIndex + 2] as Reduction;
                        if (valueExpressionToken != null) {
                            v.Value = CreateExpression(valueExpressionToken, ref endToken);
                            v.Value.Parent = v;
                            processedExpressionTokens.Add(valueExpressionToken);
                        }
                    }
                }

                lastToken = endToken;
                if (firstToken != null && lastToken != null) {
                    v.Offset = firstToken.Position.Index;
                    v.Length = lastToken.Position.Index - firstToken.Position.Index;
                    v.StartLine = firstToken.Position.Line;
                    v.StartColumn = firstToken.Position.Column;
                    v.EndLine = lastToken.Position.Line;
                    v.EndColumn = lastToken.Position.Column;
                }

                AddGrapeEntityToCurrentParent(v);
            }
        }

        private void AddParametersToCallExpression(GrapeCallExpression callExpression, Reduction token, int startIndex, out int lastTokenIndex) {
            lastTokenIndex = startIndex;
            if (token.Children.Count > 0) {
                Reduction formalParamListToken = token.Children[0] as Reduction;
                foreach (Token formalParamToken in formalParamListToken.Children) {
                    Reduction formalParamNonterminalToken = formalParamToken as Reduction;
                    if (formalParamNonterminalToken != null) {
                        if (formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLIST || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLIST_COMMA || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLISTOPT || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLISTOPT2) {
                            AddParametersToCallExpression(callExpression, formalParamListToken, startIndex, out lastTokenIndex);
                        } else {
                            GrapeExpression param = CreateExpression(formalParamNonterminalToken);
                            callExpression.Parameters.Add(param);
                            lastTokenIndex++;
                        }
                    }
                }
            } else {
                lastTokenIndex++;
            }
        }

        private void AddParametersToFunction(GrapeInitStatement i, Reduction token, int startIndex, out int lastTokenIndex) {
            lastTokenIndex = startIndex;
            if (token.Children.Count > 0) {
                Reduction formalParamListToken = token.Children[0] as Reduction;
                foreach (Token formalParamToken in formalParamListToken.Children) {
                    Reduction formalParamNonterminalToken = formalParamToken as Reduction;
                    if (formalParamNonterminalToken != null) {
                        if (formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLIST || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLIST_COMMA || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLISTOPT || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_ARGLISTOPT2) {
                            AddParametersToFunction(i, formalParamListToken, startIndex, out lastTokenIndex);
                        } else {
                            GrapeExpression param = CreateExpression(formalParamNonterminalToken);
                            param.Parent = i;
                            i.Parameters.Add(param);
                            lastTokenIndex++;
                        }
                    }
                }
            } else {
                lastTokenIndex++;
            }
        }

        private void AddParametersToFunction(GrapeFunction f, Reduction token, int startIndex, out int lastTokenIndex) {
            lastTokenIndex = startIndex;
            if (token.Children.Count > 0) {
                Reduction formalParamListToken = token.Children[0] as Reduction;
                foreach (Token formalParamToken in formalParamListToken.Children) {
                    Reduction formalParamNonterminalToken = formalParamToken as Reduction;
                    if (formalParamNonterminalToken != null) {
                        if (formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_FORMALPARAMLIST || formalParamNonterminalToken.Symbol.Index == (int)RuleConstants.RULE_FORMALPARAMLIST_COMMA) {
                            AddParametersToFunction(f, formalParamListToken, startIndex, out lastTokenIndex);
                        } else {
                            GrapeVariable param = new GrapeVariable();
                            param.IsParameter = true;
                            param.FileName = currentFileName;
                            param.Parent = f;
                            foreach (Token childToken in formalParamNonterminalToken.Children) {
                                Reduction childNonterminalToken = childToken as Reduction;
                                TextToken childTerminalToken = childToken as TextToken;
                                if (childNonterminalToken != null) {
                                    GrapeExpression paramType = GetTypeFromTypeToken(formalParamNonterminalToken.Children, param);
                                    param.Type = paramType;
                                } else if (childTerminalToken != null && childTerminalToken.Text != ",") {
                                    string paramName = childTerminalToken.Text;
                                    param.Name = paramName;
                                    param.Offset = childTerminalToken.Position.Index;
                                    param.Length = childTerminalToken.Text.Length;
                                    param.StartLine = childTerminalToken.Position.Line;
                                    param.StartColumn = childTerminalToken.Position.Column;
                                    param.EndLine = childTerminalToken.Position.Line;
                                    param.EndColumn = childTerminalToken.Position.Column + childTerminalToken.Text.Length;
                                }
                            }

                            f.Parameters.Add(param);
                            if (allEntities.ContainsKey(param.GetType())) {
                                allEntities[param.GetType()].Add(param);
                            } else {
                                List<GrapeEntity> list = new List<GrapeEntity>();
                                list.Add(param);
                                allEntities[param.GetType()] = list;
                            }

                            if (allEntitiesWithFileFilter.ContainsKey(param.FileName)) {
                                if (allEntitiesWithFileFilter[param.FileName].ContainsKey(param.GetType())) {
                                    allEntitiesWithFileFilter[param.FileName][param.GetType()].Add(param);
                                } else {
                                    List<GrapeEntity> list = new List<GrapeEntity>();
                                    list.Add(param);
                                    allEntitiesWithFileFilter[param.FileName].Add(param.GetType(), list);
                                }
                            } else {
                                Dictionary<Type, List<GrapeEntity>> dictionary = new Dictionary<Type, List<GrapeEntity>>();
                                List<GrapeEntity> list = new List<GrapeEntity>();
                                list.Add(param);
                                dictionary.Add(param.GetType(), list);
                                allEntitiesWithFileFilter.Add(param.FileName, dictionary);
                            }

                            lastTokenIndex++;
                        }
                    }
                }
            } else {
                lastTokenIndex++;
            }
        }

        private void CreateFunction(Reduction token) {
            if (token.Children.Count >= 4) {
                GrapeFunction f = new GrapeFunction();
                TextToken firstToken = null;
                TextToken lastToken = null;
                Reduction modifiers = token.Children[0] as Reduction;
                if (modifiers != null && modifiers.Children.Count > 0) {
                    f.Modifiers = GetModifiers(modifiers, out firstToken);
                }

                int nameTokenIndex = 2;
                GrapeExpression type = GetTypeFromTypeToken(token.Children, f);
                TextToken ctorToken = token.Children[1] as TextToken;
                bool isCtor = false;
                if (ctorToken != null && (ctorToken.Text == "ctor" || ctorToken.Text == "dctor")) {
                    type = new GrapeIdentifierExpression { Identifier = ctorToken.Text };
                    isCtor = true;
                }

                f.ReturnType = type;
                if (isCtor) {
                    string typeText = (type as GrapeIdentifierExpression).Identifier;
                    if (typeText == "ctor") {
                        f.Type = GrapeFunction.GrapeFunctionType.Constructor;
                    } else if (typeText == "dctor") {
                        f.Type = GrapeFunction.GrapeFunctionType.Destructor;
                    }
                }

                TextToken nameToken = token.Children[nameTokenIndex] as TextToken;
                if (nameToken != null) {
                    firstToken = nameToken;
                    f.Name = nameToken.Text;
                }

                if (f.Type == GrapeFunction.GrapeFunctionType.Constructor) {
                    if (currentBlock != null) {
                        GrapeClass c = currentBlock.Parent as GrapeClass;
                        if (c != null) {
                            f.ReturnType = new GrapeIdentifierExpression { Identifier = c.Name };
                        } else {
                            f.ReturnType = new GrapeIdentifierExpression { Identifier = f.Name };
                        }
                    } else {
                        f.ReturnType = new GrapeIdentifierExpression { Identifier = f.Name };
                    }
                } else if (f.Type == GrapeFunction.GrapeFunctionType.Destructor) {
                    f.ReturnType = new GrapeIdentifierExpression { Identifier = "void_base" };
                }

                int lastTokenIndex;
                if (f.Type != GrapeFunction.GrapeFunctionType.Destructor) {
                    Reduction parametersToken = token.Children[nameTokenIndex + 2] as Reduction;
                    AddParametersToFunction(f, parametersToken, nameTokenIndex + 2, out lastTokenIndex);
                }

                TextToken endToken = GetEndToken(token.Children);
                if (endToken != null) {
                    lastToken = endToken;
                }

                if (firstToken != null && lastToken != null) {
                    f.Offset = firstToken.Position.Index;
                    f.Length = lastToken.Position.Index - firstToken.Position.Index;
                    f.StartLine = firstToken.Position.Line;
                    f.StartColumn = firstToken.Position.Column;
                    f.EndLine = lastToken.Position.Line;
                    f.EndColumn = lastToken.Position.Column;
                }

                currentEntityWithBlock = f;
                AddGrapeEntityToCurrentParent(f);
                CreateBlock(token);
            }
        }

        private void CreateBlock(Reduction token) {
            if (currentEntityWithBlock != null && currentEntityWithBlock is GrapeEntity && currentEntityWithBlock != lastEntityWithBlock) {
                GrapeBlock block = new GrapeBlock();
                GrapeEntity currentEntity = currentEntityWithBlock as GrapeEntity;
                block.Offset = currentEntity.Offset;
                block.Length = currentEntity.Length;
                block.StartLine = currentEntity.StartLine;
                block.StartColumn = currentEntity.StartColumn;
                block.EndLine = currentEntity.EndLine;
                block.EndColumn = currentEntity.EndColumn;
                block.Parent = currentEntity;
                currentBlock = block;
                this.currentEntity = currentEntity;
                currentEntityWithBlock.Block = block;
                lastEntityWithBlock = currentEntityWithBlock;
            }
        }

        private void CreateClass(Reduction token) {
            if (token.Children.Count >= 3) {
                GrapeClass c = new GrapeClass();
                TextToken firstToken = null;
                TextToken lastToken = null;
                Reduction modifiers = token.Children[0] as Reduction;
                if (modifiers != null && modifiers.Children.Count > 0) {
                    c.Modifiers = GetModifiers(modifiers, out firstToken);
                }

                TextToken nameToken = token.Children[2] as TextToken;
                if (nameToken != null) {
                    c.Name = nameToken.Text;
                    firstToken = nameToken;
                    lastToken = nameToken;
                }

                if (token.Children.Count >= 4) {
                    Reduction sizeToken = token.Children[3] as Reduction;
                    if (sizeToken != null && sizeToken.Children.Count > 0) {
                        TextToken terminalToken = sizeToken.Children[1] as TextToken;
                        if (terminalToken != null) {
                            c.Size = Convert.ToInt32(terminalToken.Text);
                        }
                    }
                }

                if (token.Children.Count >= 5) {
                    Reduction inheritanceToken = token.Children[4] as Reduction;
                    if (inheritanceToken != null && inheritanceToken.Children.Count >= 2) {
                        Reduction qualifiedSuperIdToken = inheritanceToken.Children[1] as Reduction;
                        if (qualifiedSuperIdToken != null && qualifiedSuperIdToken.Children.Count >= 1) {
                            Reduction qualifiedIdToken = qualifiedSuperIdToken.Children[0] as Reduction;
                            c.Inherits = CreateExpression(qualifiedIdToken);
                            c.Inherits.Parent = c;
                        }
                    } else if (c.Name != "void" && c.Name != "void_base" && c.Name != "object") {
                        c.Inherits = new GrapeIdentifierExpression { Identifier = "object" };
                        c.Inherits.Parent = c;
                    }
                }

                TextToken endToken = GetEndToken(token.Children);
                if (endToken != null) {
                    lastToken = endToken;
                }

                if (firstToken != null && lastToken != null) {
                    c.Offset = firstToken.Position.Index;
                    c.Length = lastToken.Position.Index - firstToken.Position.Index;
                    c.StartLine = firstToken.Position.Line;
                    c.StartColumn = firstToken.Position.Column;
                    c.EndLine = lastToken.Position.Line;
                    c.EndColumn = lastToken.Position.Column;
                }

                currentEntityWithBlock = c;
                AddGrapeEntityToCurrentParent(c);
                CreateBlock(token);
            }
        }

        private void CreatePackageDeclaration(Reduction token) {
            if (token.Children.Count >= 2) {
                GrapePackageDeclaration packageDeclaration = new GrapePackageDeclaration();
                TextToken firstToken = token.Children[0] as TextToken;
                Reduction qualifiedIdToken = token.Children[1] as Reduction;
                if (qualifiedIdToken != null) {
                    TextToken lastToken;
                    string packageName = GetQualifiedIdText(qualifiedIdToken, out lastToken);
                    packageDeclaration.PackageName = packageName;
                    packageDeclaration.FileName = currentFileName;
                    if (firstToken != null && lastToken != null) {
                        packageDeclaration.Offset = firstToken.Position.Index;
                        packageDeclaration.Length = lastToken.Position.Index - firstToken.Position.Index;
                        packageDeclaration.StartLine = firstToken.Position.Line;
                        packageDeclaration.StartColumn = firstToken.Position.Column;
                        packageDeclaration.EndLine = lastToken.Position.Line;
                        packageDeclaration.EndColumn = lastToken.Position.Column;
                    }

                    config.Ast.Children.Add(packageDeclaration);
                    if (allEntities.ContainsKey(packageDeclaration.GetType())) {
                        allEntities[packageDeclaration.GetType()].Add(packageDeclaration);
                    } else {
                        List<GrapeEntity> list = new List<GrapeEntity>();
                        list.Add(packageDeclaration);
                        allEntities[packageDeclaration.GetType()] = list;
                    }

                    if (allEntitiesWithFileFilter.ContainsKey(packageDeclaration.FileName)) {
                        if (allEntitiesWithFileFilter[packageDeclaration.FileName].ContainsKey(packageDeclaration.GetType())) {
                            allEntitiesWithFileFilter[packageDeclaration.FileName][packageDeclaration.GetType()].Add(packageDeclaration);
                        } else {
                            List<GrapeEntity> list = new List<GrapeEntity>();
                            list.Add(packageDeclaration);
                            allEntitiesWithFileFilter[packageDeclaration.FileName].Add(packageDeclaration.GetType(), list);
                        }
                    } else {
                        Dictionary<Type, List<GrapeEntity>> dictionary = new Dictionary<Type, List<GrapeEntity>>();
                        List<GrapeEntity> list = new List<GrapeEntity>();
                        list.Add(packageDeclaration);
                        dictionary.Add(packageDeclaration.GetType(), list);
                        allEntitiesWithFileFilter.Add(packageDeclaration.FileName, dictionary);
                    }

                    currentPackageDeclaration = packageDeclaration;
                }
            }
        }

        private void CreateImportDeclaration(Reduction token) {
            if (token.Children.Count >= 2) {
                GrapeImportDeclaration importDeclaration = new GrapeImportDeclaration();
                TextToken firstToken = token.Children[0] as TextToken;
                Reduction qualifiedIdToken = token.Children[1] as Reduction;
                if (qualifiedIdToken != null) {
                    TextToken lastToken;
                    string importedPackageName = GetQualifiedIdText(qualifiedIdToken, out lastToken);
                    importDeclaration.ImportedPackage = importedPackageName;
                    importDeclaration.FileName = currentFileName;
                    if (firstToken != null && lastToken != null) {
                        importDeclaration.Offset = firstToken.Position.Index;
                        importDeclaration.Length = lastToken.Position.Index - firstToken.Position.Index;
                        importDeclaration.StartLine = firstToken.Position.Line;
                        importDeclaration.StartColumn = firstToken.Position.Column;
                        importDeclaration.EndLine = lastToken.Position.Line;
                        importDeclaration.EndColumn = lastToken.Position.Column;
                    }

                    config.Ast.Children.Add(importDeclaration);
                    if (allEntities.ContainsKey(importDeclaration.GetType())) {
                        allEntities[importDeclaration.GetType()].Add(importDeclaration);
                    } else {
                        List<GrapeEntity> list = new List<GrapeEntity>();
                        list.Add(importDeclaration);
                        allEntities[importDeclaration.GetType()] = list;
                    }

                    if (allEntitiesWithFileFilter.ContainsKey(importDeclaration.FileName)) {
                        if (allEntitiesWithFileFilter[importDeclaration.FileName].ContainsKey(importDeclaration.GetType())) {
                            allEntitiesWithFileFilter[importDeclaration.FileName][importDeclaration.GetType()].Add(importDeclaration);
                        } else {
                            List<GrapeEntity> list = new List<GrapeEntity>();
                            list.Add(importDeclaration);
                            allEntitiesWithFileFilter[importDeclaration.FileName].Add(importDeclaration.GetType(), list);
                        }
                    } else {
                        Dictionary<Type, List<GrapeEntity>> dictionary = new Dictionary<Type, List<GrapeEntity>>();
                        List<GrapeEntity> list = new List<GrapeEntity>();
                        list.Add(importDeclaration);
                        dictionary.Add(importDeclaration.GetType(), list);
                        allEntitiesWithFileFilter.Add(importDeclaration.FileName, dictionary);
                    }
                }
            }
        }

        private void ProcessNonterminalTokensOfToken(Reduction token) {
            foreach (Token childNormalToken in token.Children) {
                Reduction childToken = childNormalToken as Reduction;
                if (childToken != null) {
                    switch (childToken.Symbol.Index) {
                        // Generic top-level clauses (packages, imports, etc.)
                        case (int)RuleConstants.RULE_PACKAGE_PACKAGE:
                            CreatePackageDeclaration(childToken);
                            break;
                        case (int)RuleConstants.RULE_IMPORT_IMPORT:
                            CreateImportDeclaration(childToken);
                            break;
                        // Objects
                        case (int)RuleConstants.RULE_CLASSDECL_CLASS_IDENTIFIER_END:
                            CreateClass(childToken);
                            break;
                        case (int)RuleConstants.RULE_FIELDDEC_IDENTIFIER:
                        case (int)RuleConstants.RULE_FIELDDEC_IDENTIFIER_EQ:
                        case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER:
                        case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER2:
                        case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ:
                        case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ2:
                            CreateVariable(childToken);
                            break;
                        case (int)RuleConstants.RULE_CONSTRUCTORDEC_CTOR_IDENTIFIER_LPARAN_RPARAN_END:
                        case (int)RuleConstants.RULE_DESTRUCTORDEC_DCTOR_IDENTIFIER_LPARAN_RPARAN_END:
                        case (int)RuleConstants.RULE_METHODDEC_IDENTIFIER_LPARAN_RPARAN_END:
                            CreateFunction(childToken);
                            break;
                        // Statements
                        case (int)RuleConstants.RULE_NORMALSTM_RETURN:
                            CreateStatement(childToken, GrapeStatementType.GrapeReturnStatement);
                            break;
                        case (int)RuleConstants.RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END:
                        case (int)RuleConstants.RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END2:
                            CreateStatement(childToken, GrapeStatementType.GrapeForEachStatement);
                            break;
                        case (int)RuleConstants.RULE_STATEMENT_IF_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeIfStatement);
                            break;
                        case (int)RuleConstants.RULE_STATEMENT_ELSE_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeElseStatement);
                            break;
                        case (int)RuleConstants.RULE_STATEMENT_ELSEIF_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeElseIfStatement);
                            break;
                        case (int)RuleConstants.RULE_STATEMENT_WHILE_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeWhileStatement);
                            break;
                        case (int)RuleConstants.RULE_CONSTRUCTORINITSTMS_INIT_THIS_LPARAN_RPARAN:
                        case (int)RuleConstants.RULE_CONSTRUCTORINITSTMS_INIT_BASE_LPARAN_RPARAN:
                            CreateStatement(childToken, GrapeStatementType.GrapeInitStatement);
                            break;
                        case (int)RuleConstants.RULE_NORMALSTM_SWITCH_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeSwitchStatement);
                            break;
                        case (int)RuleConstants.RULE_SWITCHLABEL_CASE_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeCaseStatement);
                            break;
                        case (int)RuleConstants.RULE_SWITCHLABEL_DEFAULT_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeDefaultStatement);
                            break;
                        case (int)RuleConstants.RULE_NORMALSTM_TRY_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeTryStatement);
                            break;
                        case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_END:
                        case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_END2:
                        case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_IDENTIFIER_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeCatchStatement);
                            break;
                        case (int)RuleConstants.RULE_FINALLYCLAUSEOPT_FINALLY_END:
                            CreateStatement(childToken, GrapeStatementType.GrapeFinallyStatement);
                            break;
                        case (int)RuleConstants.RULE_NORMALSTM_DELETE:
                            CreateStatement(childToken, GrapeStatementType.GrapeDeleteStatement);
                            break;
                        // Expressions. The reason we only handle StatementExp here is because it is the only expression type that can be in a statement list. All other expressions are either children of a StatementExp or a Statement.
                        case (int)RuleConstants.RULE_STATEMENTEXP:
                        case (int)RuleConstants.RULE_STATEMENTEXP2:
                        case (int)RuleConstants.RULE_STATEMENTEXP3:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET2:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN2:
                        case (int)RuleConstants.RULE_VALUE_NEW_LPARAN_RPARAN:
                            if (!processedExpressionTokens.Contains(childToken)) {
                                GrapeExpression expression = CreateExpression(childToken);
                                expression.Parent = currentBlock;
                                if (currentBlock != null) {
                                    currentBlock.Children.Add(expression);
                                }
                            }

                            break;
                    }

                    ProcessNonterminalTokensOfToken(childToken);
                }
            }
        }

        private void AcceptEvent(Reduction acceptToken) {
            processedExpressionTokens.Clear();
            ProcessNonterminalTokensOfToken(acceptToken);
        }

        private void TokenErrorEvent(LalrProcessor parser, out bool continueParsing) {
            string message = "Token error with input: '" + parser.CurrentToken + "'";
            	continueParsing = config.ContinueOnError;
            if (config.OutputErrors) {
                errorSink.AddError(new GrapeErrorSink.Error { Description = message, FileName = currentFileName, Offset = parser.CurrentToken.Position.Index, Length = 1, StartLine = parser.CurrentToken.Position.Line, EndLine = parser.CurrentToken.Position.Line, StartColumn = parser.CurrentToken.Position.Column, EndColumn = parser.CurrentToken.Position.Column + 1 });
            }
        }

        private void ParseErrorEvent(LalrProcessor parser, out bool continueParsing) {
            if (parser.CurrentToken.Symbol.Kind != SymbolKind.End) {
                string message = "Unexpected token: '" + parser.CurrentToken + "'";
            	continueParsing = config.ContinueOnError;
                if (config.OutputErrors) {
                    errorSink.AddError(new GrapeErrorSink.Error { Description = message, FileName = currentFileName, Offset = parser.CurrentToken.Position.Index, Length = 1, StartLine = parser.CurrentToken.Position.Line, EndLine = parser.CurrentToken.Position.Line, StartColumn = parser.CurrentToken.Position.Column, EndColumn = parser.CurrentToken.Position.Column + 1 });
                }
            } else {
            	continueParsing = false;
            }
        }
    }
}
