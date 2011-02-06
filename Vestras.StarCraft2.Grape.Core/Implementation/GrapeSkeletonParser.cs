using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using com.calitha.goldparser;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
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
        SYMBOL_ELSE = 44, // else
        SYMBOL_ELSEIF = 45, // elseif
        SYMBOL_END = 46, // end
        SYMBOL_FALSE = 47, // false
        SYMBOL_FINALLY = 48, // finally
        SYMBOL_FOREACH = 49, // foreach
        SYMBOL_HEXLITERAL = 50, // HexLiteral
        SYMBOL_IDENTIFIER = 51, // Identifier
        SYMBOL_IF = 52, // if
        SYMBOL_IMPORT = 53, // import
        SYMBOL_IN = 54, // in
        SYMBOL_INHERITS = 55, // inherits
        SYMBOL_INIT = 56, // init
        SYMBOL_INTERNAL = 57, // internal
        SYMBOL_MEMBERNAME = 58, // MemberName
        SYMBOL_NEW = 59, // new
        SYMBOL_NEWLINE = 60, // NewLine
        SYMBOL_NULL = 61, // null
        SYMBOL_OVERRIDE = 62, // override
        SYMBOL_PACKAGE = 63, // package
        SYMBOL_PRIVATE = 64, // private
        SYMBOL_PROTECTED = 65, // protected
        SYMBOL_PUBLIC = 66, // public
        SYMBOL_REALLITERAL = 67, // RealLiteral
        SYMBOL_RETURN = 68, // return
        SYMBOL_SEALED = 69, // sealed
        SYMBOL_STATIC = 70, // static
        SYMBOL_STRINGLITERAL = 71, // StringLiteral
        SYMBOL_SWITCH = 72, // switch
        SYMBOL_THIS = 73, // this
        SYMBOL_THROW = 74, // throw
        SYMBOL_TRUE = 75, // true
        SYMBOL_TRY = 76, // try
        SYMBOL_WHILE = 77, // while
        SYMBOL_ACCESS = 78, // <Access>
        SYMBOL_ADDEXP = 79, // <Add Exp>
        SYMBOL_ANDEXP = 80, // <And Exp>
        SYMBOL_ARGLIST = 81, // <Arg List>
        SYMBOL_ARGLISTOPT = 82, // <Arg List Opt>
        SYMBOL_ARGUMENT = 83, // <Argument>
        SYMBOL_ARRAYINITIALIZER = 84, // <Array Initializer>
        SYMBOL_ASSIGNTAIL = 85, // <Assign Tail>
        SYMBOL_CATCHCLAUSE = 86, // <Catch Clause>
        SYMBOL_CATCHCLAUSES = 87, // <Catch Clauses>
        SYMBOL_CLASSBASEOPT = 88, // <Class Base Opt>
        SYMBOL_CLASSDECL = 89, // <Class Decl>
        SYMBOL_CLASSITEM = 90, // <Class Item>
        SYMBOL_CLASSITEMDECSOPT = 91, // <Class Item Decs Opt>
        SYMBOL_COMPAREEXP = 92, // <Compare Exp>
        SYMBOL_CONSTRUCTORDEC = 93, // <Constructor Dec>
        SYMBOL_CONSTRUCTORINITSTMS = 94, // <Constructor Init Stms>
        SYMBOL_DESTRUCTORDEC = 95, // <Destructor Dec>
        SYMBOL_EQUALITYEXP = 96, // <Equality Exp>
        SYMBOL_EXPRESSION = 97, // <Expression>
        SYMBOL_EXPRESSIONOPT = 98, // <Expression Opt>
        SYMBOL_FIELDDEC = 99, // <Field Dec>
        SYMBOL_FINALLYCLAUSEOPT = 100, // <Finally Clause Opt>
        SYMBOL_FORMALPARAM = 101, // <Formal Param>
        SYMBOL_FORMALPARAMLIST = 102, // <Formal Param List>
        SYMBOL_FORMALPARAMLISTOPT = 103, // <Formal Param List Opt>
        SYMBOL_IMPORT2 = 104, // <Import>
        SYMBOL_LITERAL = 105, // <Literal>
        SYMBOL_LOCALVARDECL = 106, // <Local Var Decl>
        SYMBOL_LOGICALANDEXP = 107, // <Logical And Exp>
        SYMBOL_LOGICALOREXP = 108, // <Logical Or Exp>
        SYMBOL_LOGICALXOREXP = 109, // <Logical Xor Exp>
        SYMBOL_MEMBERLIST = 110, // <Member List>
        SYMBOL_METHODCALL = 111, // <Method Call>
        SYMBOL_METHODCALLS = 112, // <Method Calls>
        SYMBOL_METHODDEC = 113, // <Method Dec>
        SYMBOL_MODIFIER = 114, // <Modifier>
        SYMBOL_MODIFIERS = 115, // <Modifiers>
        SYMBOL_MULTEXP = 116, // <Mult Exp>
        SYMBOL_NL = 117, // <NL>
        SYMBOL_NLOREOF = 118, // <NL Or EOF>
        SYMBOL_NONARRAYTYPE = 119, // <Non Array Type>
        SYMBOL_NORMALSTM = 120, // <Normal Stm>
        SYMBOL_OBJECTS = 121, // <Objects>
        SYMBOL_OREXP = 122, // <Or Exp>
        SYMBOL_PACKAGE2 = 123, // <Package>
        SYMBOL_QUALIFIEDID = 124, // <Qualified ID>
        SYMBOL_RANKSPECIFIER = 125, // <Rank Specifier>
        SYMBOL_RANKSPECIFIERS = 126, // <Rank Specifiers>
        SYMBOL_SHIFTEXP = 127, // <Shift Exp>
        SYMBOL_START = 128, // <Start>
        SYMBOL_STATEMENT = 129, // <Statement>
        SYMBOL_STATEMENTEXP = 130, // <Statement Exp>
        SYMBOL_STMLIST = 131, // <Stm List>
        SYMBOL_SWITCHLABEL = 132, // <Switch Label>
        SYMBOL_SWITCHSECTIONSOPT = 133, // <Switch Sections Opt>
        SYMBOL_TYPE = 134, // <Type>
        SYMBOL_TYPEDECL = 135, // <Type Decl>
        SYMBOL_TYPEDECLOPT = 136, // <Type Decl Opt>
        SYMBOL_TYPECASTEXP = 137, // <Typecast Exp>
        SYMBOL_UNARYEXP = 138, // <Unary Exp>
        SYMBOL_VALIDID = 139, // <Valid ID>
        SYMBOL_VALUE = 140, // <Value>
        SYMBOL_VARIABLEDECLARATOR = 141, // <Variable Declarator>
        SYMBOL_VARIABLEDECLARATORBASE = 142, // <Variable Declarator Base>
        SYMBOL_VARIABLEINITIALIZER = 143, // <Variable Initializer>
        SYMBOL_VARIABLEINITIALIZERLIST = 144, // <Variable Initializer List>
        SYMBOL_VARIABLEINITIALIZERLISTOPT = 145  // <Variable Initializer List Opt>
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
        RULE_VALUE_NEW_LPARAN_RPARAN = 84, // <Value> ::= new <Qualified ID> '(' <Arg List Opt> ')'
        RULE_ARGLISTOPT = 85, // <Arg List Opt> ::= <Arg List>
        RULE_ARGLISTOPT2 = 86, // <Arg List Opt> ::= 
        RULE_ARGLIST_COMMA = 87, // <Arg List> ::= <Arg List> ',' <Argument>
        RULE_ARGLIST = 88, // <Arg List> ::= <Argument>
        RULE_ARGUMENT = 89, // <Argument> ::= <Expression>
        RULE_STMLIST = 90, // <Stm List> ::= <Stm List> <Statement>
        RULE_STMLIST2 = 91, // <Stm List> ::= 
        RULE_STATEMENT = 92, // <Statement> ::= <Local Var Decl>
        RULE_STATEMENT_IF_END = 93, // <Statement> ::= if <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_ELSE_END = 94, // <Statement> ::= else <NL> <Stm List> end <NL>
        RULE_STATEMENT_ELSEIF_END = 95, // <Statement> ::= elseif <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END = 96, // <Statement> ::= foreach <Qualified ID> Identifier in <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END2 = 97, // <Statement> ::= foreach <Qualified ID> <Rank Specifiers> Identifier in <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_WHILE_END = 98, // <Statement> ::= while <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT2 = 99, // <Statement> ::= <Normal Stm>
        RULE_NORMALSTM_SWITCH_END = 100, // <Normal Stm> ::= switch <Expression> <NL> <Switch Sections Opt> end <NL>
        RULE_NORMALSTM_TRY_END = 101, // <Normal Stm> ::= try <NL> <Stm List> end <NL>
        RULE_NORMALSTM = 102, // <Normal Stm> ::= <Catch Clauses>
        RULE_NORMALSTM2 = 103, // <Normal Stm> ::= <Finally Clause Opt>
        RULE_NORMALSTM_BREAK = 104, // <Normal Stm> ::= break <NL>
        RULE_NORMALSTM_CONTINUE = 105, // <Normal Stm> ::= continue <NL>
        RULE_NORMALSTM_RETURN = 106, // <Normal Stm> ::= return <Expression Opt> <NL>
        RULE_NORMALSTM_THROW = 107, // <Normal Stm> ::= throw <Expression Opt> <NL>
        RULE_NORMALSTM3 = 108, // <Normal Stm> ::= <Expression> <NL>
        RULE_NORMALSTM4 = 109, // <Normal Stm> ::= <Constructor Init Stms> <NL>
        RULE_CONSTRUCTORINITSTMS_INIT_BASE_LPARAN_RPARAN = 110, // <Constructor Init Stms> ::= init base '(' <Arg List Opt> ')'
        RULE_CONSTRUCTORINITSTMS_INIT_THIS_LPARAN_RPARAN = 111, // <Constructor Init Stms> ::= init this '(' <Arg List Opt> ')'
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER = 112, // <Variable Declarator Base> ::= <Qualified ID> Identifier
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER2 = 113, // <Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ = 114, // <Variable Declarator Base> ::= <Qualified ID> Identifier '=' <Variable Initializer>
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ2 = 115, // <Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier '=' <Variable Initializer>
        RULE_VARIABLEDECLARATOR = 116, // <Variable Declarator> ::= <Variable Declarator Base> <NL>
        RULE_VARIABLEINITIALIZER = 117, // <Variable Initializer> ::= <Expression>
        RULE_VARIABLEINITIALIZER2 = 118, // <Variable Initializer> ::= <Array Initializer>
        RULE_SWITCHSECTIONSOPT = 119, // <Switch Sections Opt> ::= <Switch Sections Opt> <Switch Label>
        RULE_SWITCHSECTIONSOPT2 = 120, // <Switch Sections Opt> ::= 
        RULE_SWITCHLABEL_CASE_END = 121, // <Switch Label> ::= case <Expression> <NL> <Stm List> end <NL>
        RULE_SWITCHLABEL_DEFAULT_END = 122, // <Switch Label> ::= default <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSES = 123, // <Catch Clauses> ::= <Catch Clause> <Catch Clauses>
        RULE_CATCHCLAUSES2 = 124, // <Catch Clauses> ::= 
        RULE_CATCHCLAUSE_CATCH_IDENTIFIER_END = 125, // <Catch Clause> ::= catch <Qualified ID> Identifier <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSE_CATCH_END = 126, // <Catch Clause> ::= catch <Qualified ID> <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSE_CATCH_END2 = 127, // <Catch Clause> ::= catch <NL> <Stm List> end <NL>
        RULE_FINALLYCLAUSEOPT_FINALLY_END = 128, // <Finally Clause Opt> ::= finally <NL> <Stm List> end <NL>
        RULE_FINALLYCLAUSEOPT = 129, // <Finally Clause Opt> ::= <NL>
        RULE_ACCESS_PRIVATE = 130, // <Access> ::= private
        RULE_ACCESS_PROTECTED = 131, // <Access> ::= protected
        RULE_ACCESS_PUBLIC = 132, // <Access> ::= public
        RULE_ACCESS_INTERNAL = 133, // <Access> ::= internal
        RULE_MODIFIER_ABSTRACT = 134, // <Modifier> ::= abstract
        RULE_MODIFIER_OVERRIDE = 135, // <Modifier> ::= override
        RULE_MODIFIER_SEALED = 136, // <Modifier> ::= sealed
        RULE_MODIFIER_STATIC = 137, // <Modifier> ::= static
        RULE_MODIFIER = 138, // <Modifier> ::= <Access>
        RULE_MODIFIERS = 139, // <Modifiers> ::= <Modifier> <Modifiers>
        RULE_MODIFIERS2 = 140, // <Modifiers> ::= 
        RULE_CLASSDECL_CLASS_IDENTIFIER_END = 141, // <Class Decl> ::= <Modifiers> class Identifier <Class Base Opt> <NL> <Class Item Decs Opt> end <NL Or EOF>
        RULE_CLASSBASEOPT_INHERITS = 142, // <Class Base Opt> ::= inherits <Non Array Type>
        RULE_CLASSBASEOPT = 143, // <Class Base Opt> ::= 
        RULE_CLASSITEMDECSOPT = 144, // <Class Item Decs Opt> ::= <Class Item Decs Opt> <Class Item>
        RULE_CLASSITEMDECSOPT2 = 145, // <Class Item Decs Opt> ::= 
        RULE_CLASSITEM = 146, // <Class Item> ::= <Method Dec>
        RULE_CLASSITEM2 = 147, // <Class Item> ::= <Constructor Dec>
        RULE_CLASSITEM3 = 148, // <Class Item> ::= <Destructor Dec>
        RULE_CLASSITEM4 = 149, // <Class Item> ::= <Type Decl>
        RULE_CLASSITEM5 = 150, // <Class Item> ::= <Field Dec>
        RULE_FIELDDEC_IDENTIFIER = 151, // <Field Dec> ::= <Modifiers> <Type> Identifier <NL>
        RULE_FIELDDEC_IDENTIFIER_EQ = 152, // <Field Dec> ::= <Modifiers> <Type> Identifier '=' <Expression> <NL>
        RULE_METHODDEC_IDENTIFIER_LPARAN_RPARAN_END = 153, // <Method Dec> ::= <Modifiers> <Type> Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
        RULE_FORMALPARAMLISTOPT = 154, // <Formal Param List Opt> ::= <Formal Param List>
        RULE_FORMALPARAMLISTOPT2 = 155, // <Formal Param List Opt> ::= 
        RULE_FORMALPARAMLIST = 156, // <Formal Param List> ::= <Formal Param>
        RULE_FORMALPARAMLIST_COMMA = 157, // <Formal Param List> ::= <Formal Param List> ',' <Formal Param>
        RULE_FORMALPARAM_IDENTIFIER = 158, // <Formal Param> ::= <Type> Identifier
        RULE_TYPEDECL = 159, // <Type Decl> ::= <Class Decl>
        RULE_TYPEDECLOPT = 160, // <Type Decl Opt> ::= <Type Decl>
        RULE_TYPEDECLOPT2 = 161, // <Type Decl Opt> ::= <NL>
        RULE_CONSTRUCTORDEC_CTOR_IDENTIFIER_LPARAN_RPARAN_END = 162, // <Constructor Dec> ::= <Modifiers> ctor Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
        RULE_DESTRUCTORDEC_DCTOR_IDENTIFIER_LPARAN_RPARAN_END = 163, // <Destructor Dec> ::= <Modifiers> dctor Identifier '(' ')' <NL> <Stm List> end <NL>
        RULE_ARRAYINITIALIZER_LBRACKET_RBRACKET = 164, // <Array Initializer> ::= '[' <Variable Initializer List Opt> ']'
        RULE_ARRAYINITIALIZER_LBRACKET_COMMA_RBRACKET = 165, // <Array Initializer> ::= '[' <Variable Initializer List> ',' ']'
        RULE_VARIABLEINITIALIZERLISTOPT = 166, // <Variable Initializer List Opt> ::= <Variable Initializer List>
        RULE_VARIABLEINITIALIZERLISTOPT2 = 167, // <Variable Initializer List Opt> ::= 
        RULE_VARIABLEINITIALIZERLIST = 168, // <Variable Initializer List> ::= <Variable Initializer>
        RULE_VARIABLEINITIALIZERLIST_COMMA = 169  // <Variable Initializer List> ::= <Variable Initializer List> ',' <Variable Initializer>
    };

    internal class GrapeSkeletonParser {
        private LALRParser parser;
        private GrapeParserConfiguration config;
        private List<NonterminalToken> processedExpressionTokens = new List<NonterminalToken>();
        internal GrapeErrorSink errorSink;
        internal string currentFileName;
        internal static Dictionary<Type, List<GrapeEntity>> allEntities = new Dictionary<Type, List<GrapeEntity>>();
        internal static Dictionary<string, Dictionary<Type, List<GrapeEntity>>> allEntitiesWithFileFilter = new Dictionary<string, Dictionary<Type, List<GrapeEntity>>>();

        public GrapeSkeletonParser(string filename, GrapeParserConfiguration config) {
            this.config = config;
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public GrapeSkeletonParser(string baseName, string resourceName, GrapeParserConfiguration config) {
            this.config = config;
            byte[] buffer = ResourceUtil.GetByteArrayResource(GetType().Assembly, baseName, resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public GrapeSkeletonParser(Stream stream, GrapeParserConfiguration config) {
            this.config = config;
            Init(stream);
        }

        private void Init(Stream stream) {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;
            parser.OnAccept += new LALRParser.AcceptHandler(AcceptEvent);
            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public void Parse(string source) {
            parser.Parse(source);
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

        private string GetQualifiedIdText(NonterminalToken qualifiedIdToken, out TerminalToken lastToken) {
            TerminalToken dummyToken = null;
            return GetQualifiedIdText(qualifiedIdToken, ref dummyToken, out lastToken);
        }

        private string GetQualifiedIdText(NonterminalToken qualifiedIdToken, ref TerminalToken firstToken, out TerminalToken lastToken) {
            string qualifiedIdText = "";
            return GetQualifiedIdText(qualifiedIdToken, ref firstToken, ref qualifiedIdText, out lastToken);
        }

        private string GetQualifiedIdText(NonterminalToken qualifiedIdToken, ref TerminalToken firstToken, ref string qualifiedIdText, out TerminalToken lastToken) {
            lastToken = null;
            foreach (Token childNormalToken in qualifiedIdToken.Tokens) {
                NonterminalToken childToken = childNormalToken as NonterminalToken;
                TerminalToken childTerminalToken = childNormalToken as TerminalToken;
                if (childToken != null && childToken.Tokens.Length >= 1) {
                    foreach (Token terminalNormalToken in childToken.Tokens) {
                        TerminalToken terminalToken = terminalNormalToken as TerminalToken;
                        NonterminalToken nonterminalToken = terminalNormalToken as NonterminalToken;
                        if (terminalToken != null) {
                            if (firstToken == null) {
                                firstToken = terminalToken;
                            } else if (terminalToken.Location.Position < terminalToken.Location.Position) {
                                firstToken = terminalToken;
                            }

                            qualifiedIdText += terminalToken.Text;
                            lastToken = terminalToken;
                        } else if (nonterminalToken != null && nonterminalToken.Rule.Id != (int)RuleConstants.RULE_MEMBERLIST) {
                            GetQualifiedIdText(nonterminalToken, ref firstToken, ref qualifiedIdText, out lastToken);
                        }
                    }
                } else if (childTerminalToken != null) {
                    if (firstToken == null) {
                        firstToken = childTerminalToken;
                    } else if (childTerminalToken.Location.Position < childTerminalToken.Location.Position) {
                        firstToken = childTerminalToken;
                    }

                    qualifiedIdText += childTerminalToken.Text;
                }
            }

            return qualifiedIdText;
        }

        private static readonly string DefaultAccessModifier = "public";
        private static readonly string[] AccessModifiers = new string[] {
            "public",
            "private",
            "protected",
            "internal"
        };

        private string GetModifiers(NonterminalToken modifiers, out TerminalToken firstToken) {
            string result = "";
            firstToken = null;
            if (modifiers.Tokens.Length >= 1) {
                int index = 0;
                foreach (Token token in modifiers.Tokens) {
                    NonterminalToken nonterminalToken = token as NonterminalToken;
                    if (nonterminalToken != null) {
                        int id = nonterminalToken.Rule.Id;
                        if (id == (int)RuleConstants.RULE_MODIFIER || id == (int)RuleConstants.RULE_MODIFIER_ABSTRACT || id == (int)RuleConstants.RULE_MODIFIER_OVERRIDE || id == (int)RuleConstants.RULE_MODIFIER_SEALED || id == (int)RuleConstants.RULE_MODIFIER_STATIC) {
                            int childIndex = 0;
                            foreach (Token childToken in nonterminalToken.Tokens) {
                                TerminalToken terminalToken = childToken as TerminalToken;
                                if (terminalToken != null) {
                                    if (firstToken == null) {
                                        firstToken = terminalToken;
                                    } else {
                                        if (terminalToken.Location.Position < firstToken.Location.Position) {
                                            firstToken = terminalToken;
                                        }
                                    }

                                    result += terminalToken.Text;
                                }

                                if (childIndex < nonterminalToken.Tokens.Length - 1) {
                                    result += " ";
                                }

                                childIndex++;
                            }

                            if (index < modifiers.Tokens.Length - 1) {
                                result += " ";
                            }
                        } else if (id == (int)RuleConstants.RULE_MODIFIERS || id == (int)RuleConstants.RULE_MODIFIERS2) {
                            result += GetModifiers(nonterminalToken, out firstToken);
                            if (index < modifiers.Tokens.Length - 1) {
                                result += " ";
                            }
                        }
                    }

                    index++;
                }
            }

            bool containsAccessModifier = false;
            foreach (string modifier in result.Split(' ')) {
                foreach (string accessModifier in AccessModifiers) {
                    if (modifier == accessModifier) {
                        containsAccessModifier = true;
                        break;
                    }
                }
            }

            if (!containsAccessModifier) {
                result = DefaultAccessModifier + " " + result;
            }

            return result;
        }

        private string AddRankSpecifiersToQualifiedId(string qualifiedId, Token[] tokens, int currentIndex, ref int endTypeTokenIndex) {
            string newQualifiedId = qualifiedId;
            if (tokens.Length > currentIndex + 1) {
                NonterminalToken suspectedRankSpecifierToken = tokens[currentIndex + 1] as NonterminalToken;
                if (suspectedRankSpecifierToken != null) {
                    endTypeTokenIndex++;
                    foreach (Token token in suspectedRankSpecifierToken.Tokens) {
                        NonterminalToken rankSpecifierToken = token as NonterminalToken;
                        if (rankSpecifierToken != null) {
                            foreach (Token childToken in rankSpecifierToken.Tokens) {
                                TerminalToken terminalToken = childToken as TerminalToken;
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

        private GrapeExpression GetTypeFromTypeToken(Token[] tokens, GrapeEntity parent) {
            GrapeExpression expression = null;
            int currentIndex = 0;
            foreach (Token token in tokens) {
                NonterminalToken nonterminalToken = token as NonterminalToken;
                if (nonterminalToken != null && ((nonterminalToken.Rule.Id == (int)RuleConstants.RULE_TYPE || nonterminalToken.Rule.Id == (int)RuleConstants.RULE_TYPE2) || nonterminalToken.Rule.Id == (int)RuleConstants.RULE_QUALIFIEDID)) {
                    if (currentIndex + 1 < tokens.Length && tokens[currentIndex + 1] is NonterminalToken && ((NonterminalToken)tokens[currentIndex + 1]).Rule.Id == (int)RuleConstants.RULE_RANKSPECIFIERS) {
                        expression = new GrapeArrayAccessExpression();
                        expression.FileName = currentFileName;
                        GrapeArrayAccessExpression arrayExpression = expression as GrapeArrayAccessExpression;
                        arrayExpression.Member = CreateExpression(tokens[currentIndex] as NonterminalToken);
                        if (arrayExpression.Member != null) {
                            arrayExpression.Member.FileName = currentFileName;
                        }

                        NonterminalToken rankSpecifierToken = (tokens[currentIndex + 1] as NonterminalToken).Tokens[0] as NonterminalToken;
                        NonterminalToken arrayExpressionToken = rankSpecifierToken.Tokens[1] as NonterminalToken;
                        arrayExpression.Array = CreateExpression(arrayExpressionToken);
                    } else if (nonterminalToken.Rule.Id == (int)RuleConstants.RULE_TYPE || nonterminalToken.Rule.Id == (int)RuleConstants.RULE_TYPE2) {
                        NonterminalToken nonArrayToken = nonterminalToken.Tokens[0] as NonterminalToken;
                        NonterminalToken qualifiedIdToken = nonArrayToken.Tokens[0] as NonterminalToken;
                        if (nonterminalToken.Tokens.Length > 1 && nonterminalToken.Tokens[1] is NonterminalToken && ((NonterminalToken)nonterminalToken.Tokens[1]).Rule.Id == (int)RuleConstants.RULE_RANKSPECIFIERS) {
                            expression = new GrapeArrayAccessExpression();
                            expression.FileName = currentFileName;
                            GrapeArrayAccessExpression arrayExpression = expression as GrapeArrayAccessExpression;
                            arrayExpression.Member = CreateExpression(qualifiedIdToken);
                            if (arrayExpression.Member != null) {
                                arrayExpression.Member.FileName = currentFileName;
                            }

                            NonterminalToken rankSpecifierToken = (nonterminalToken.Tokens[1] as NonterminalToken).Tokens[0] as NonterminalToken;
                            NonterminalToken arrayExpressionToken = rankSpecifierToken.Tokens[1] as NonterminalToken;
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

        private TerminalToken GetEndToken(Token[] tokens) {
            foreach (Token token in tokens) {
                if (token is TerminalToken && ((TerminalToken)token).Text == "end") {
                    return token as TerminalToken;
                }
            }

            return null;
        }

        private void AddMethodCallsToExpression(GrapeMemberExpression expression, NonterminalToken token) {
            foreach (Token childToken in token.Tokens) {
                if (childToken is NonterminalToken && (((NonterminalToken)childToken).Rule.Id == (int)RuleConstants.RULE_METHODCALLS || ((NonterminalToken)childToken).Rule.Id == (int)RuleConstants.RULE_METHODCALLS2)) {
                    NonterminalToken nonterminalToken = childToken as NonterminalToken;
                    GrapeMemberExpression nextExpression = new GrapeMemberExpression();
                    GrapeIdentifierExpression identifierExpression = new GrapeIdentifierExpression();
                    identifierExpression.FileName = currentFileName;
                    processedExpressionTokens.Add(nonterminalToken);
                    if (nonterminalToken.Tokens.Length > 0) {
                        NonterminalToken methodCallToken = nonterminalToken.Tokens[0] as NonterminalToken;
                        TerminalToken identifierToken = methodCallToken.Tokens[0] as TerminalToken;
                        identifierExpression.Identifier = identifierToken.Text.Trim('.');
                        if (methodCallToken.Tokens.Length > 1) {
                            TerminalToken bracketToken = methodCallToken.Tokens[1] as TerminalToken;
                            if (bracketToken != null) {
                                if (bracketToken.Text == "[") {
                                    GrapeArrayAccessExpression arrayExpression = new GrapeArrayAccessExpression();
                                    arrayExpression.FileName = currentFileName;
                                    NonterminalToken expressionToken = methodCallToken.Tokens[2] as NonterminalToken;
                                    arrayExpression.Array = CreateExpression(expressionToken);
                                    nextExpression = arrayExpression;
                                } else if (bracketToken.Text == "(") {
                                    GrapeCallExpression callExpression = new GrapeCallExpression();
                                    callExpression.FileName = currentFileName;
                                    NonterminalToken argListOptToken = methodCallToken.Tokens[2] as NonterminalToken;
                                    if (argListOptToken != null && argListOptToken.Tokens.Length > 0) {
                                        NonterminalToken argListToken = argListOptToken.Tokens[0] as NonterminalToken;
                                        foreach (Token argNormalToken in argListToken.Tokens) {
                                            NonterminalToken argToken = argNormalToken as NonterminalToken;
                                            if (argToken != null && argToken.Tokens.Length > 0) {
                                                NonterminalToken argumentToken = argToken.Tokens[0] as NonterminalToken;
                                                if (argumentToken != null && argumentToken.Tokens.Length > 0) {
                                                    NonterminalToken argExpressionToken = argumentToken.Tokens[0] as NonterminalToken;
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

        private TerminalToken GetFirstTerminalToken(Token[] tokens) {
            foreach (Token token in tokens) {
                if (token is TerminalToken) {
                    return token as TerminalToken;
                } else if (token is NonterminalToken) {
                    TerminalToken recursiveToken = GetFirstTerminalToken((token as NonterminalToken).Tokens);
                    if (recursiveToken != null) {
                        return recursiveToken;
                    }
                }
            }

            return null;
        }

        private GrapeExpression CreateExpression(NonterminalToken token) {
            TerminalToken dummyToken = null;
            return CreateExpression(token, ref dummyToken);
        }

        private GrapeExpression CreateExpression(NonterminalToken token, ref TerminalToken lastToken) {
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
            if (token != null) {
                processedExpressionTokens.Add(token);
                if (token.Tokens.Length > 1 || (token.Tokens.Length == 1 && token.Tokens[0] is TerminalToken)) {
                    switch (token.Rule.Id) {
                        // Expression in brackets: ((expression))
                        case (int)RuleConstants.RULE_VALUE_LPARAN_RPARAN:
                            expression = new GrapeStackExpression();
                            expression.FileName = currentFileName;
                            stackExpression = expression as GrapeStackExpression;
                            stackExpression.Child = CreateExpression(token.Tokens[1] as NonterminalToken);
                            break;
                        // General statements (method calling, member accessing, variable accessing, etc.).
                        case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN2:
                        case (int)RuleConstants.RULE_VALUE_NEW_LPARAN_RPARAN:
                            int startIndex = 0;
                            if (token.Rule.Id == (int)RuleConstants.RULE_VALUE_NEW_LPARAN_RPARAN) {
                                startIndex = 1;
                                callExpression = new GrapeObjectCreationExpression();
                            } else {
                                callExpression = new GrapeCallExpression();
                            }

                            callExpression.FileName = currentFileName;
                            memberExpression = CreateExpression(token.Tokens[startIndex] as NonterminalToken, ref lastToken) as GrapeMemberExpression;
                            if (memberExpression != null) {
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
                                int lastTokenIndex;
                                NonterminalToken parametersToken = token.Tokens[startIndex + 2] as NonterminalToken;
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

                                if (token.Tokens.Length > startIndex + 5) {
                                    NonterminalToken assignTailToken = token.Tokens[startIndex + 5] as NonterminalToken;
                                    if (assignTailToken != null && assignTailToken.Rule.Id == (int)RuleConstants.RULE_ASSIGNTAIL_EQ) {
                                        processedExpressionTokens.Add(assignTailToken);
                                        GrapeSetExpression setExpression = new GrapeSetExpression();
                                        setExpression.FileName = currentFileName;
                                        setExpression.Member = callExpression;
                                        setExpression.Value = CreateExpression(assignTailToken.Tokens[1] as NonterminalToken, ref lastToken);
                                        expression = setExpression;
                                    }
                                }
                            }

                            break;
                        case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET:
                        case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET2:
                            startIndex = 0;
                            arrayExpression = new GrapeArrayAccessExpression();
                            memberExpression = CreateExpression(token.Tokens[startIndex] as NonterminalToken, ref lastToken) as GrapeMemberExpression;
                            arrayExpression.FileName = currentFileName;
                            if (memberExpression != null) {
                                memberExpression.FileName = currentFileName;
                                arrayExpression.Member = memberExpression.Member;
                                arrayExpression.Array = CreateExpression(token.Tokens[startIndex + 2] as NonterminalToken, ref lastToken);
                                AddMethodCallsToExpression(arrayExpression, token);
                                expression = arrayExpression;
                                if (token.Tokens.Length > 5) {
                                    NonterminalToken assignTailToken = token.Tokens[startIndex + 5] as NonterminalToken;
                                    if (assignTailToken != null && assignTailToken.Rule.Id == (int)RuleConstants.RULE_ASSIGNTAIL_EQ) {
                                        processedExpressionTokens.Add(assignTailToken);
                                        GrapeSetExpression setExpression = new GrapeSetExpression();
                                        setExpression.FileName = currentFileName;
                                        setExpression.Member = arrayExpression;
                                        setExpression.Value = CreateExpression(assignTailToken.Tokens[1] as NonterminalToken, ref lastToken);
                                        expression = setExpression;
                                    }
                                }
                            }

                            break;
                        case (int)RuleConstants.RULE_STATEMENTEXP:
                        case (int)RuleConstants.RULE_STATEMENTEXP2:
                        case (int)RuleConstants.RULE_STATEMENTEXP3:
                            NonterminalToken qualifiedIdToken = token.Tokens[0] as NonterminalToken;
                            GrapeMemberExpression member = CreateExpression(qualifiedIdToken, ref lastToken) as GrapeMemberExpression;
                            expression = member;
                            expression.FileName = currentFileName;
                            if (expression != null) {
                                AddMethodCallsToExpression(expression as GrapeMemberExpression, token);
                                if (token.Tokens.Length > 2) {
                                    NonterminalToken assignTailToken = token.Tokens[2] as NonterminalToken;
                                    if (assignTailToken != null && assignTailToken.Rule.Id == (int)RuleConstants.RULE_ASSIGNTAIL_EQ) {
                                        processedExpressionTokens.Add(assignTailToken);
                                        GrapeSetExpression setExpression = new GrapeSetExpression();
                                        setExpression.Member = member;
                                        setExpression.FileName = currentFileName;
                                        setExpression.Value = CreateExpression(assignTailToken.Tokens[1] as NonterminalToken, ref lastToken);
                                        expression = setExpression;
                                    }
                                }
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
                            typecastExpression.Value = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            typecastExpression.Type = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            break;
                        // Literal expressions (hexadecimals, integers, reals, strings, true, false and null).
                        case (int)RuleConstants.RULE_LITERAL_DECIMALLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Int;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_HEXLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Hexadecimal;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_REALLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Real;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_STRINGLITERAL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.String;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_FALSE:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.False;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_TRUE:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.True;
                            break;
                        case (int)RuleConstants.RULE_LITERAL_NULL:
                            expression = new GrapeLiteralExpression();
                            expression.FileName = currentFileName;
                            literalExpression = expression as GrapeLiteralExpression;
                            literalExpression.Value = (token.Tokens[0] as TerminalToken).Text;
                            literalExpression.Type = GrapeLiteralExpression.GrapeLiteralExpressionType.Null;
                            break;
                        // Conditional expressions, such as (expression) && (expression), (expression) ^ (expression), etc.
                        case (int)RuleConstants.RULE_ANDEXP:
                        case (int)RuleConstants.RULE_ANDEXP_AMPAMP:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LogicalAnd;
                            break;
                        case (int)RuleConstants.RULE_OREXP:
                        case (int)RuleConstants.RULE_OREXP_PIPEPIPE:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LogicalOr;
                            break;
                        case (int)RuleConstants.RULE_LOGICALANDEXP:
                        case (int)RuleConstants.RULE_LOGICALANDEXP_AMP:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryAnd;
                            break;
                        case (int)RuleConstants.RULE_LOGICALOREXP:
                        case (int)RuleConstants.RULE_LOGICALOREXP_PIPE:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryOr;
                            break;
                        case (int)RuleConstants.RULE_LOGICALXOREXP:
                        case (int)RuleConstants.RULE_LOGICALXOREXP_CARET:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LogicalXor;
                            break;
                        // Equal and not equal expressions: (expression) == (expression), (expression) != (expression).
                        case (int)RuleConstants.RULE_EQUALITYEXP_EQEQ:
                        case (int)RuleConstants.RULE_EQUALITYEXP_EXCLAMEQ:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            if (token.Rule.Id == (int)RuleConstants.RULE_EQUALITYEXP_EQEQ) {
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
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.GreaterThan;
                            break;
                        case (int)RuleConstants.RULE_COMPAREEXP_GTEQ:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.GreaterThanOrEqual;
                            break;
                        case (int)RuleConstants.RULE_COMPAREEXP_LT:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LessThan;
                            break;
                        case (int)RuleConstants.RULE_COMPAREEXP_LTEQ:
                            expression = new GrapeConditionalExpression();
                            expression.FileName = currentFileName;
                            conditionalExpression = expression as GrapeConditionalExpression;
                            conditionalExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            conditionalExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            conditionalExpression.Type = GrapeConditionalExpression.GrapeConditionalExpressionType.LessThanOrEqual;
                            break;
                        // Mathematical expressions, e.g (expression) + (expression), (expression) % (expression), ~(expression), etc.
                        // Addition and subtraction expressions: (expression) + (expression), (expression) - (expression).
                        case (int)RuleConstants.RULE_ADDEXP_PLUS:
                        case (int)RuleConstants.RULE_ADDEXP_MINUS:
                            expression = new GrapeAddExpression();
                            expression.FileName = currentFileName;
                            addExpression = expression as GrapeAddExpression;
                            addExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            addExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            addExpression.Type = token.Rule.Id == (int)RuleConstants.RULE_ADDEXP_MINUS ? GrapeAddExpression.GrapeAddExpressionType.Subtraction : GrapeAddExpression.GrapeAddExpressionType.Addition;
                            break;
                        // Multiplication expressions: (expression) * (expression), (expression) / (expression), (expression) % (expression).
                        case (int)RuleConstants.RULE_MULTEXP_DIV:
                            expression = new GrapeMultiplicationExpression();
                            expression.FileName = currentFileName;
                            multiplicationExpression = expression as GrapeMultiplicationExpression;
                            multiplicationExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            multiplicationExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            multiplicationExpression.Type = GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Division;
                            break;
                        case (int)RuleConstants.RULE_MULTEXP_PERCENT:
                            expression = new GrapeMultiplicationExpression();
                            expression.FileName = currentFileName;
                            multiplicationExpression = expression as GrapeMultiplicationExpression;
                            multiplicationExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            multiplicationExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            multiplicationExpression.Type = GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Mod;
                            break;
                        case (int)RuleConstants.RULE_MULTEXP_TIMES:
                            expression = new GrapeMultiplicationExpression();
                            expression.FileName = currentFileName;
                            multiplicationExpression = expression as GrapeMultiplicationExpression;
                            multiplicationExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            multiplicationExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            multiplicationExpression.Type = GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Multiplication;
                            break;
                        // Shift expressions: (expression) << (expression), (expression) >> (expression).
                        case (int)RuleConstants.RULE_SHIFTEXP_GTGT:
                            expression = new GrapeShiftExpression();
                            expression.FileName = currentFileName;
                            shiftExpression = expression as GrapeShiftExpression;
                            shiftExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            shiftExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            shiftExpression.Type = GrapeShiftExpression.GrapeShiftExpressionType.ShiftRight;
                            break;
                        case (int)RuleConstants.RULE_SHIFTEXP_LTLT:
                            expression = new GrapeShiftExpression();
                            expression.FileName = currentFileName;
                            shiftExpression = expression as GrapeShiftExpression;
                            shiftExpression.Left = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
                            shiftExpression.Right = CreateExpression(token.Tokens[2] as NonterminalToken, ref lastToken);
                            shiftExpression.Type = GrapeShiftExpression.GrapeShiftExpressionType.ShiftLeft;
                            break;
                        // Unary expressions: !(expression), -(expression), ~(expression).
                        case (int)RuleConstants.RULE_UNARYEXP_EXCLAM:
                            expression = new GrapeUnaryExpression();
                            expression.FileName = currentFileName;
                            unaryExpression = expression as GrapeUnaryExpression;
                            unaryExpression.Type = GrapeUnaryExpression.GrapeUnaryExpressionType.Not;
                            unaryExpression.Value = CreateExpression(token.Tokens[1] as NonterminalToken, ref lastToken);
                            break;
                        case (int)RuleConstants.RULE_UNARYEXP_MINUS:
                            expression = new GrapeUnaryExpression();
                            expression.FileName = currentFileName;
                            unaryExpression = expression as GrapeUnaryExpression;
                            unaryExpression.Type = GrapeUnaryExpression.GrapeUnaryExpressionType.Negate;
                            unaryExpression.Value = CreateExpression(token.Tokens[1] as NonterminalToken, ref lastToken);
                            break;
                        case (int)RuleConstants.RULE_UNARYEXP_TILDE:
                            expression = new GrapeUnaryExpression();
                            expression.FileName = currentFileName;
                            unaryExpression = expression as GrapeUnaryExpression;
                            unaryExpression.Type = GrapeUnaryExpression.GrapeUnaryExpressionType.CurlyEqual;
                            unaryExpression.Value = CreateExpression(token.Tokens[1] as NonterminalToken, ref lastToken);
                            break;
                    }
                } else {
                    expression = CreateExpression(token.Tokens[0] as NonterminalToken, ref lastToken);
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
                TerminalToken randomTerminalToken = GetFirstTerminalToken(token.Tokens);
                if (randomTerminalToken != null) {
                    expression.Line = randomTerminalToken.Location.LineNr;
                    expression.Offset = randomTerminalToken.Location.Position;
                    expression.Length = 1;
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
            GrapeInitStatement
        }

        private GrapeStatement CreateStatement(NonterminalToken token, GrapeStatementType statementType) {
            TerminalToken dummyToken = null;
            return CreateStatement(token, statementType, ref dummyToken);
        }

        private GrapeIfStatement currentIfStatement;
        private GrapeSwitchStatement currentSwitchStatement;
        private GrapeTryStatement currentTryStatement;
        private GrapeStatement CreateStatement(NonterminalToken token, GrapeStatementType statementType, ref TerminalToken lastToken) {
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
            TerminalToken blockFirstToken = null;
            TerminalToken blockLastToken = null;
            NonterminalToken conditionToken = null;
            NonterminalToken expressionToken = null;
            int currentIndex = 0;
            int startIndex = 0;
            int index = 1;
            switch (statementType) {
                case GrapeStatementType.GrapeForEachStatement:
                    statement = new GrapeForEachStatement();
                    forEachStatement = statement as GrapeForEachStatement;
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = GetEndToken(token.Tokens);
                    index = 1;
                    while (token.Tokens[index] is TerminalToken) {
                        index++;
                    }

                    startIndex = index - 1;
                    GrapeVariable v = new GrapeVariable();
                    GrapeExpression type = GetTypeFromTypeToken(token.Tokens, v);
                    v.Type = type;
                    TerminalToken nameToken = token.Tokens[startIndex + 1] as TerminalToken;
                    if (nameToken != null) {
                        v.Name = nameToken.Text;
                    }

                    forEachStatement.Variable = v;
                    NonterminalToken containerExpressionToken = token.Tokens[startIndex + 3] as NonterminalToken;
                    if (containerExpressionToken != null) {
                        forEachStatement.ContainerExpression = CreateExpression(containerExpressionToken);
                        forEachStatement.ContainerExpression.Parent = forEachStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        forEachStatement.Offset = blockFirstToken.Location.Position;
                        forEachStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
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
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = GetEndToken(token.Tokens);
                    conditionToken = token.Tokens[1] as NonterminalToken;
                    if (conditionToken == null && token.Tokens[1] is TerminalToken && ((TerminalToken)token.Tokens[1]).Text == "(") {
                        conditionToken = token.Tokens[2] as NonterminalToken;
                    }

                    if (conditionToken != null) {
                        ifStatement.Condition = CreateExpression(conditionToken);
                        ifStatement.Condition.Parent = ifStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        ifStatement.Offset = blockFirstToken.Location.Position;
                        ifStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
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
                        blockFirstToken = token.Tokens[0] as TerminalToken;
                        blockLastToken = GetEndToken(token.Tokens);
                        conditionToken = token.Tokens[1] as NonterminalToken;
                        if (conditionToken == null && token.Tokens[1] is TerminalToken && ((TerminalToken)token.Tokens[1]).Text == "(") {
                            conditionToken = token.Tokens[2] as NonterminalToken;
                        }

                        if (conditionToken != null) {
                            elseIfStatement.Condition = CreateExpression(conditionToken);
                            elseIfStatement.Condition.Parent = elseIfStatement;
                        }

                        if (blockFirstToken != null && blockLastToken != null) {
                            elseIfStatement.Offset = blockFirstToken.Location.Position;
                            elseIfStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = elseIfStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "An elseif statement must have an if statement as a parent.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
                    }

                    break;
                case GrapeStatementType.GrapeElseStatement:
                    if (currentIfStatement != null) {
                        statement = new GrapeElseStatement();
                        elseStatement = statement as GrapeElseStatement;
                        elseStatement.IfStatement = currentIfStatement;
                        currentIfStatement.ElseStatements.Add(elseStatement);
                        blockFirstToken = token.Tokens[0] as TerminalToken;
                        blockLastToken = GetEndToken(token.Tokens);
                        if (blockFirstToken != null && blockLastToken != null) {
                            elseStatement.Offset = blockFirstToken.Location.Position;
                            elseStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = elseStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                        if (currentIfStatement.ElseStatements.Count > 1) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "An if statement can only have one else statement.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
                        }
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "An else statement must have an if statement as a parent.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
                    }

                    break;
                case GrapeStatementType.GrapeSwitchStatement:
                    statement = new GrapeSwitchStatement();
                    switchStatement = statement as GrapeSwitchStatement;
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = GetEndToken(token.Tokens);
                    currentSwitchStatement = switchStatement;
                    if (token.Tokens.Length >= 1) {
                        currentIndex = 0;
                        expressionToken = null;
                        while (currentIndex < token.Tokens.Length) {
                            if (token.Tokens[currentIndex] is NonterminalToken && (((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                expressionToken = token.Tokens[currentIndex] as NonterminalToken;
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
                        switchStatement.Offset = blockFirstToken.Location.Position;
                        switchStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
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
                        blockFirstToken = token.Tokens[0] as TerminalToken;
                        blockLastToken = GetEndToken(token.Tokens);
                        if (token.Tokens.Length >= 1) {
                            currentIndex = 0;
                            expressionToken = null;
                            while (currentIndex < token.Tokens.Length) {
                                if (token.Tokens[currentIndex] is NonterminalToken && (((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                    expressionToken = token.Tokens[currentIndex] as NonterminalToken;
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
                            caseStatement.Offset = blockFirstToken.Location.Position;
                            caseStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = caseStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A case statement must have a switch statement as a parent.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
                    }

                    break;
                case GrapeStatementType.GrapeDefaultStatement:
                    if (currentSwitchStatement != null) {
                        statement = new GrapeDefaultStatement();
                        defaultStatement = statement as GrapeDefaultStatement;
                        defaultStatement.SwitchStatement = currentSwitchStatement;
                        currentSwitchStatement.DefaultStatement = defaultStatement;
                        blockFirstToken = token.Tokens[0] as TerminalToken;
                        blockLastToken = GetEndToken(token.Tokens);
                        if (blockFirstToken != null && blockLastToken != null) {
                            defaultStatement.Offset = blockFirstToken.Location.Position;
                            defaultStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = defaultStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A default statement must have a switch statement as a parent.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
                    }

                    break;
                case GrapeStatementType.GrapeTryStatement:
                    statement = new GrapeTryStatement();
                    tryStatement = statement as GrapeTryStatement;
                    currentTryStatement = tryStatement;
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = GetEndToken(token.Tokens);
                    if (blockFirstToken != null && blockLastToken != null) {
                        tryStatement.Offset = blockFirstToken.Location.Position;
                        tryStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
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
                        blockFirstToken = token.Tokens[0] as TerminalToken;
                        blockLastToken = GetEndToken(token.Tokens);
                        index = 1;
                        while (token.Tokens[index] is TerminalToken) {
                            index++;
                        }

                        startIndex = index - 1;
                        if (token.Tokens.Length > 1) {
                            GrapeVariable catchVariable = new GrapeVariable();
                            GrapeExpression catchVariableType = GetTypeFromTypeToken(token.Tokens, catchVariable);
                            catchVariable.Type = catchVariableType;
                            if (token.Tokens.Length > startIndex + 1) {
                                TerminalToken catchVariableName = token.Tokens[startIndex + 1] as TerminalToken;
                                if (catchVariableName != null) {
                                    catchVariable.Name = catchVariableName.Text;
                                }
                            }

                            catchStatement.Variable = catchVariable;
                        }

                        if (blockFirstToken != null && blockLastToken != null) {
                            catchStatement.Offset = blockFirstToken.Location.Position;
                            catchStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = catchStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A catch statement must have a try statement as a parent.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
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

                        blockFirstToken = token.Tokens[0] as TerminalToken;
                        blockLastToken = GetEndToken(token.Tokens);
                        if (blockFirstToken != null && blockLastToken != null) {
                            finallyStatement.Offset = blockFirstToken.Location.Position;
                            finallyStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                        }

                        lastToken = blockLastToken;
                        currentEntityWithBlock = finallyStatement;
                        AddGrapeEntityToCurrentParent(statement);
                        CreateBlock(token);
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A finally statement must have a try statement as a parent.", FileName = currentFileName, Offset = statement.Offset, Length = statement.Length });
                    }

                    break;
                case GrapeStatementType.GrapeWhileStatement:
                    statement = new GrapeWhileStatement();
                    whileStatement = statement as GrapeWhileStatement;
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = GetEndToken(token.Tokens);
                    currentIndex = 0;
                    expressionToken = null;
                    while (currentIndex < token.Tokens.Length) {
                        if (token.Tokens[currentIndex] is NonterminalToken && (((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                            expressionToken = token.Tokens[currentIndex] as NonterminalToken;
                            break;
                        }

                        currentIndex++;
                    }

                    if (expressionToken != null) {
                        whileStatement.Condition = CreateExpression(expressionToken);
                        whileStatement.Condition.Parent = whileStatement;
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        whileStatement.Offset = blockFirstToken.Location.Position;
                        whileStatement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                    }

                    lastToken = blockLastToken;
                    currentEntityWithBlock = whileStatement;
                    AddGrapeEntityToCurrentParent(statement);
                    CreateBlock(token);
                    break;
                case GrapeStatementType.GrapeBreakStatement:
                    statement = new GrapeBreakStatement();
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Location.Position;
                        statement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeContinueStatement:
                    statement = new GrapeContinueStatement();
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Location.Position;
                        statement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeReturnStatement:
                    statement = new GrapeReturnStatement();
                    returnStatement = statement as GrapeReturnStatement;
                    if (token.Tokens.Length > 1) {
                        currentIndex = 0;
                        expressionToken = null;
                        while (currentIndex < token.Tokens.Length) {
                            if (token.Tokens[currentIndex] is NonterminalToken && (((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                expressionToken = token.Tokens[currentIndex] as NonterminalToken;
                                break;
                            }

                            currentIndex++;
                        }

                        if (expressionToken != null) {
                            returnStatement.ReturnValue = CreateExpression(expressionToken);
                            returnStatement.ReturnValue.Parent = returnStatement;
                        }
                    }

                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Location.Position;
                        statement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeThrowStatement:
                    statement = new GrapeThrowStatement();
                    throwStatement = statement as GrapeThrowStatement;
                    if (token.Tokens.Length > 1) {
                        currentIndex = 0;
                        expressionToken = null;
                        while (currentIndex < token.Tokens.Length) {
                            if (token.Tokens[currentIndex] is NonterminalToken && (((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSIONOPT2 || ((NonterminalToken)token.Tokens[currentIndex]).Rule.Id == (int)RuleConstants.RULE_EXPRESSION_EQ)) {
                                expressionToken = token.Tokens[currentIndex] as NonterminalToken;
                                break;
                            }

                            currentIndex++;
                        }

                        if (expressionToken != null) {
                            throwStatement.ThrowExpression = CreateExpression(expressionToken);
                            throwStatement.ThrowExpression.Parent = throwStatement;
                        }
                    }

                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = blockFirstToken;
                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Location.Position;
                        statement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
                case GrapeStatementType.GrapeInitStatement:
                    statement = new GrapeInitStatement();
                    initStatement = statement as GrapeInitStatement;
                    blockFirstToken = token.Tokens[0] as TerminalToken;
                    blockLastToken = blockFirstToken;
                    TerminalToken initToken = token.Tokens[1] as TerminalToken;
                    if (initToken != null) {
                        initStatement.Type = GrapeInitStatement.GrapeInitStatementType.Unknown;
                        if (initToken.Text == "base") {
                            initStatement.Type = GrapeInitStatement.GrapeInitStatementType.Base;
                        } else if (initToken.Text == "this") {
                            initStatement.Type = GrapeInitStatement.GrapeInitStatementType.This;
                        }
                    }

                    int lastTokenIndex;
                    NonterminalToken parametersToken = token.Tokens[3] as NonterminalToken;
                    if (parametersToken != null) {
                        AddParametersToFunction(initStatement, parametersToken, 0, out lastTokenIndex);
                    }

                    if (blockFirstToken != null && blockLastToken != null) {
                        statement.Offset = blockFirstToken.Location.Position;
                        statement.Length = blockLastToken.Location.Position - blockFirstToken.Location.Position;
                    }

                    lastToken = blockLastToken;
                    AddGrapeEntityToCurrentParent(statement);
                    break;
            }

            return statement;
        }

        private void CreateVariable(NonterminalToken token) {
            if (token.Tokens.Length >= 4) {
                GrapeVariable v;
                GrapeField f = null;
                TerminalToken firstToken = null;
                TerminalToken lastToken = null;
                NonterminalToken modifiers = token.Tokens[0] as NonterminalToken;
                if ((modifiers != null && modifiers.Rule.Id != (int)RuleConstants.RULE_QUALIFIEDID && modifiers.Tokens.Length > 0) || currentBlock.Parent is GrapeClass) {
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
                GrapeExpression type = GetTypeFromTypeToken(token.Tokens, v);
                v.Type = type;
                if (nameTokenIndex >= token.Tokens.Length) {
                    nameTokenIndex = 1;
                }

                TerminalToken nameToken = token.Tokens[nameTokenIndex] as TerminalToken;
                if (nameToken.Text == "=") {
                    nameTokenIndex--;
                    nameToken = token.Tokens[nameTokenIndex] as TerminalToken;
                }

                if (nameToken != null) {
                    v.Name = nameToken.Text;
                    firstToken = nameToken;
                }

                TerminalToken endToken = nameToken;
                if (token.Tokens.Length > nameTokenIndex + 1) {
                    TerminalToken initializerToken = token.Tokens[nameTokenIndex + 1] as TerminalToken;
                    if (initializerToken != null && initializerToken.Text == "=") {
                        NonterminalToken valueExpressionToken = token.Tokens[nameTokenIndex + 2] as NonterminalToken;
                        if (valueExpressionToken != null) {
                            v.Value = CreateExpression(valueExpressionToken, ref endToken);
                            v.Value.Parent = v;
                            processedExpressionTokens.Add(valueExpressionToken);
                        }
                    }
                }

                lastToken = endToken;
                if (firstToken != null && lastToken != null) {
                    v.Offset = firstToken.Location.Position;
                    v.Length = lastToken.Location.Position - firstToken.Location.Position;
                }

                AddGrapeEntityToCurrentParent(v);
            }
        }

        private void AddParametersToCallExpression(GrapeCallExpression callExpression, NonterminalToken token, int startIndex, out int lastTokenIndex) {
            lastTokenIndex = startIndex;
            if (token.Tokens.Length > 0) {
                NonterminalToken formalParamListToken = token.Tokens[0] as NonterminalToken;
                foreach (Token formalParamToken in formalParamListToken.Tokens) {
                    NonterminalToken formalParamNonterminalToken = formalParamToken as NonterminalToken;
                    if (formalParamNonterminalToken != null) {
                        if (formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLIST || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLIST_COMMA || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLISTOPT || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLISTOPT2) {
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

        private void AddParametersToFunction(GrapeInitStatement i, NonterminalToken token, int startIndex, out int lastTokenIndex) {
            lastTokenIndex = startIndex;
            if (token.Tokens.Length > 0) {
                NonterminalToken formalParamListToken = token.Tokens[0] as NonterminalToken;
                foreach (Token formalParamToken in formalParamListToken.Tokens) {
                    NonterminalToken formalParamNonterminalToken = formalParamToken as NonterminalToken;
                    if (formalParamNonterminalToken != null) {
                        if (formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLIST || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLIST_COMMA || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLISTOPT || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_ARGLISTOPT2) {
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

        private void AddParametersToFunction(GrapeFunction f, NonterminalToken token, int startIndex, out int lastTokenIndex) {
            lastTokenIndex = startIndex;
            if (token.Tokens.Length > 0) {
                NonterminalToken formalParamListToken = token.Tokens[0] as NonterminalToken;
                foreach (Token formalParamToken in formalParamListToken.Tokens) {
                    NonterminalToken formalParamNonterminalToken = formalParamToken as NonterminalToken;
                    if (formalParamNonterminalToken != null) {
                        if (formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_FORMALPARAMLIST || formalParamNonterminalToken.Rule.Id == (int)RuleConstants.RULE_FORMALPARAMLIST_COMMA) {
                            AddParametersToFunction(f, formalParamListToken, startIndex, out lastTokenIndex);
                        } else {
                            GrapeVariable param = new GrapeVariable();
                            param.IsParameter = true;
                            param.FileName = currentFileName;
                            param.Parent = f;
                            foreach (Token childToken in formalParamNonterminalToken.Tokens) {
                                NonterminalToken childNonterminalToken = childToken as NonterminalToken;
                                TerminalToken childTerminalToken = childToken as TerminalToken;
                                if (childNonterminalToken != null) {
                                    GrapeExpression paramType = GetTypeFromTypeToken(formalParamNonterminalToken.Tokens, param);
                                    param.Type = paramType;
                                } else if (childTerminalToken != null && childTerminalToken.Text != ",") {
                                    string paramName = childTerminalToken.Text;
                                    param.Name = paramName;
                                    param.Offset = childTerminalToken.Location.Position;
                                    param.Length = childTerminalToken.Text.Length;
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

        private void CreateFunction(NonterminalToken token) {
            if (token.Tokens.Length >= 4) {
                GrapeFunction f = new GrapeFunction();
                TerminalToken firstToken = null;
                TerminalToken lastToken = null;
                NonterminalToken modifiers = token.Tokens[0] as NonterminalToken;
                if (modifiers != null && modifiers.Tokens.Length > 0) {
                    f.Modifiers = GetModifiers(modifiers, out firstToken);
                }

                int nameTokenIndex = 2;
                GrapeExpression type = GetTypeFromTypeToken(token.Tokens, f);
                TerminalToken ctorToken = token.Tokens[1] as TerminalToken;
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

                TerminalToken nameToken = token.Tokens[nameTokenIndex] as TerminalToken;
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
                    NonterminalToken parametersToken = token.Tokens[nameTokenIndex + 2] as NonterminalToken;
                    AddParametersToFunction(f, parametersToken, nameTokenIndex + 2, out lastTokenIndex);
                    if (lastTokenIndex > -1) {
                        lastToken = token.Tokens[lastTokenIndex] as TerminalToken;
                    }
                }

                TerminalToken endToken = GetEndToken(token.Tokens);
                if (endToken != null) {
                    lastToken = endToken;
                }

                if (firstToken != null && lastToken != null) {
                    f.Offset = firstToken.Location.Position;
                    f.Length = lastToken.Location.Position - firstToken.Location.Position;
                }

                currentEntityWithBlock = f;
                AddGrapeEntityToCurrentParent(f);
                CreateBlock(token);
            }
        }

        private void CreateBlock(NonterminalToken token) {
            if (currentEntityWithBlock != null && currentEntityWithBlock is GrapeEntity && currentEntityWithBlock != lastEntityWithBlock) {
                GrapeBlock block = new GrapeBlock();
                GrapeEntity currentEntity = currentEntityWithBlock as GrapeEntity;
                block.Offset = currentEntity.Offset;
                block.Length = currentEntity.Length;
                block.Parent = currentEntity;
                currentBlock = block;
                this.currentEntity = currentEntity;
                currentEntityWithBlock.Block = block;
                lastEntityWithBlock = currentEntityWithBlock;
            }
        }

        private void CreateClass(NonterminalToken token) {
            if (token.Tokens.Length >= 3) {
                GrapeClass c = new GrapeClass();
                TerminalToken firstToken = null;
                TerminalToken lastToken = null;
                NonterminalToken modifiers = token.Tokens[0] as NonterminalToken;
                if (modifiers != null && modifiers.Tokens.Length > 0) {
                    c.Modifiers = GetModifiers(modifiers, out firstToken);
                }

                TerminalToken nameToken = token.Tokens[2] as TerminalToken;
                if (nameToken != null) {
                    c.Name = nameToken.Text;
                    firstToken = nameToken;
                    lastToken = nameToken;
                }

                if (token.Tokens.Length >= 4) {
                    NonterminalToken inheritanceToken = token.Tokens[3] as NonterminalToken;
                    if (inheritanceToken != null && inheritanceToken.Tokens.Length >= 2) {
                        NonterminalToken qualifiedSuperIdToken = inheritanceToken.Tokens[1] as NonterminalToken;
                        if (qualifiedSuperIdToken != null && qualifiedSuperIdToken.Tokens.Length >= 1) {
                            NonterminalToken qualifiedIdToken = qualifiedSuperIdToken.Tokens[0] as NonterminalToken;
                            c.Inherits = CreateExpression(qualifiedIdToken);
                            c.Inherits.Parent = c;
                        }
                    } else if (c.Name != "void" && c.Name != "void_base" && c.Name != "object") {
                        c.Inherits = new GrapeIdentifierExpression { Identifier = "object" };
                        c.Inherits.Parent = c;
                    }
                }

                TerminalToken endToken = GetEndToken(token.Tokens);
                if (endToken != null) {
                    lastToken = endToken;
                }

                if (firstToken != null && lastToken != null) {
                    c.Offset = firstToken.Location.Position;
                    c.Length = lastToken.Location.Position - firstToken.Location.Position;
                }

                currentEntityWithBlock = c;
                AddGrapeEntityToCurrentParent(c);
                CreateBlock(token);
            }
        }

        private void CreatePackageDeclaration(NonterminalToken token) {
            if (token.Tokens.Length >= 2) {
                GrapePackageDeclaration packageDeclaration = new GrapePackageDeclaration();
                TerminalToken firstToken = token.Tokens[0] as TerminalToken;
                NonterminalToken qualifiedIdToken = token.Tokens[1] as NonterminalToken;
                if (qualifiedIdToken != null) {
                    TerminalToken lastToken;
                    string packageName = GetQualifiedIdText(qualifiedIdToken, out lastToken);
                    packageDeclaration.PackageName = packageName;
                    packageDeclaration.FileName = currentFileName;
                    if (firstToken != null && lastToken != null) {
                        packageDeclaration.Offset = firstToken.Location.Position;
                        packageDeclaration.Length = lastToken.Location.Position - firstToken.Location.Position;
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

        private void CreateImportDeclaration(NonterminalToken token) {
            if (token.Tokens.Length >= 2) {
                GrapeImportDeclaration importDeclaration = new GrapeImportDeclaration();
                TerminalToken firstToken = token.Tokens[0] as TerminalToken;
                NonterminalToken qualifiedIdToken = token.Tokens[1] as NonterminalToken;
                if (qualifiedIdToken != null) {
                    TerminalToken lastToken;
                    string importedPackageName = GetQualifiedIdText(qualifiedIdToken, out lastToken);
                    importDeclaration.ImportedPackage = importedPackageName;
                    importDeclaration.FileName = currentFileName;
                    if (firstToken != null && lastToken != null) {
                        importDeclaration.Offset = firstToken.Location.Position;
                        importDeclaration.Length = lastToken.Location.Position - firstToken.Location.Position;
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

        private void ProcessNonterminalTokensOfToken(NonterminalToken token) {
            foreach (Token childNormalToken in token.Tokens) {
                NonterminalToken childToken = childNormalToken as NonterminalToken;
                if (childToken != null) {
                    switch (childToken.Rule.Id) {
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

        private void AcceptEvent(LALRParser parser, AcceptEventArgs args) {
            processedExpressionTokens.Clear();
            ProcessNonterminalTokensOfToken(args.Token);
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args) {
            string message = "Token error with input: '" + args.Token.ToString() + "'";
            if (config.ContinueOnError) {
                args.Continue = true;
            }

            if (config.OutputErrors) {
                errorSink.AddError(new GrapeErrorSink.Error { Description = message, FileName = currentFileName, Offset = args.Token.Location.Position, Length = 1 });
            }
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args) {
            if (args.UnexpectedToken.ToString() != "(EOF)") {
                string message = "Unexpected token: '" + args.UnexpectedToken.ToString() + "'";
                if (config.ContinueOnError) {
                    args.Continue = ContinueMode.Skip;
                }

                if (config.OutputErrors) {
                    errorSink.AddError(new GrapeErrorSink.Error { Description = message, FileName = currentFileName, Offset = args.UnexpectedToken.Location.Position, Length = 1 });
                }
            }
        }
    }
}
