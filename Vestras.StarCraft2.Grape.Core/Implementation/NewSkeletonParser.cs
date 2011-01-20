
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;

namespace com.calitha.goldparser
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF                        =   0, // (EOF)
        SYMBOL_ERROR                      =   1, // (Error)
        SYMBOL_WHITESPACE                 =   2, // (Whitespace)
        SYMBOL_COMMENTEND                 =   3, // (Comment End)
        SYMBOL_COMMENTLINE                =   4, // (Comment Line)
        SYMBOL_COMMENTSTART               =   5, // (Comment Start)
        SYMBOL_MINUS                      =   6, // '-'
        SYMBOL_EXCLAM                     =   7, // '!'
        SYMBOL_EXCLAMEQ                   =   8, // '!='
        SYMBOL_PERCENT                    =   9, // '%'
        SYMBOL_AMP                        =  10, // '&'
        SYMBOL_AMPAMP                     =  11, // '&&'
        SYMBOL_LPARAN                     =  12, // '('
        SYMBOL_RPARAN                     =  13, // ')'
        SYMBOL_TIMES                      =  14, // '*'
        SYMBOL_COMMA                      =  15, // ','
        SYMBOL_DIV                        =  16, // '/'
        SYMBOL_LBRACKET                   =  17, // '['
        SYMBOL_RBRACKET                   =  18, // ']'
        SYMBOL_CARET                      =  19, // '^'
        SYMBOL_PIPE                       =  20, // '|'
        SYMBOL_PIPEPIPE                   =  21, // '||'
        SYMBOL_TILDE                      =  22, // '~'
        SYMBOL_PLUS                       =  23, // '+'
        SYMBOL_LT                         =  24, // '<'
        SYMBOL_LTLT                       =  25, // '<<'
        SYMBOL_LTEQ                       =  26, // '<='
        SYMBOL_EQ                         =  27, // '='
        SYMBOL_EQEQ                       =  28, // '=='
        SYMBOL_GT                         =  29, // '>'
        SYMBOL_GTEQ                       =  30, // '>='
        SYMBOL_GTGT                       =  31, // '>>'
        SYMBOL_ABSTRACT                   =  32, // abstract
        SYMBOL_AS                         =  33, // as
        SYMBOL_BASE                       =  34, // base
        SYMBOL_BREAK                      =  35, // break
        SYMBOL_CASE                       =  36, // case
        SYMBOL_CATCH                      =  37, // catch
        SYMBOL_CLASS                      =  38, // class
        SYMBOL_CONTINUE                   =  39, // continue
        SYMBOL_CTOR                       =  40, // ctor
        SYMBOL_DCTOR                      =  41, // dctor
        SYMBOL_DECIMALLITERAL             =  42, // DecimalLiteral
        SYMBOL_DEFAULT                    =  43, // default
        SYMBOL_ELSE                       =  44, // else
        SYMBOL_END                        =  45, // end
        SYMBOL_FALSE                      =  46, // false
        SYMBOL_FINALLY                    =  47, // finally
        SYMBOL_FOREACH                    =  48, // foreach
        SYMBOL_HEXLITERAL                 =  49, // HexLiteral
        SYMBOL_IDENTIFIER                 =  50, // Identifier
        SYMBOL_IF                         =  51, // if
        SYMBOL_IMPORT                     =  52, // import
        SYMBOL_IN                         =  53, // in
        SYMBOL_INHERITS                   =  54, // inherits
        SYMBOL_INIT                       =  55, // init
        SYMBOL_INTERNAL                   =  56, // internal
        SYMBOL_MEMBERNAME                 =  57, // MemberName
        SYMBOL_NEW                        =  58, // new
        SYMBOL_NEWLINE                    =  59, // NewLine
        SYMBOL_NULL                       =  60, // null
        SYMBOL_OVERRIDE                   =  61, // override
        SYMBOL_PACKAGE                    =  62, // package
        SYMBOL_PRIVATE                    =  63, // private
        SYMBOL_PROTECTED                  =  64, // protected
        SYMBOL_PUBLIC                     =  65, // public
        SYMBOL_REALLITERAL                =  66, // RealLiteral
        SYMBOL_RETURN                     =  67, // return
        SYMBOL_SEALED                     =  68, // sealed
        SYMBOL_STATIC                     =  69, // static
        SYMBOL_STRINGLITERAL              =  70, // StringLiteral
        SYMBOL_SWITCH                     =  71, // switch
        SYMBOL_THIS                       =  72, // this
        SYMBOL_THROW                      =  73, // throw
        SYMBOL_TRUE                       =  74, // true
        SYMBOL_TRY                        =  75, // try
        SYMBOL_WHILE                      =  76, // while
        SYMBOL_ACCESS                     =  77, // <Access>
        SYMBOL_ADDEXP                     =  78, // <Add Exp>
        SYMBOL_ANDEXP                     =  79, // <And Exp>
        SYMBOL_ARGLIST                    =  80, // <Arg List>
        SYMBOL_ARGLISTOPT                 =  81, // <Arg List Opt>
        SYMBOL_ARGUMENT                   =  82, // <Argument>
        SYMBOL_ARRAYINITIALIZER           =  83, // <Array Initializer>
        SYMBOL_ASSIGNTAIL                 =  84, // <Assign Tail>
        SYMBOL_CATCHCLAUSE                =  85, // <Catch Clause>
        SYMBOL_CATCHCLAUSES               =  86, // <Catch Clauses>
        SYMBOL_CLASSBASEOPT               =  87, // <Class Base Opt>
        SYMBOL_CLASSDECL                  =  88, // <Class Decl>
        SYMBOL_CLASSITEM                  =  89, // <Class Item>
        SYMBOL_CLASSITEMDECSOPT           =  90, // <Class Item Decs Opt>
        SYMBOL_COMPAREEXP                 =  91, // <Compare Exp>
        SYMBOL_CONSTRUCTORDEC             =  92, // <Constructor Dec>
        SYMBOL_CONSTRUCTORINITSTMS        =  93, // <Constructor Init Stms>
        SYMBOL_DESTRUCTORDEC              =  94, // <Destructor Dec>
        SYMBOL_EQUALITYEXP                =  95, // <Equality Exp>
        SYMBOL_EXPRESSION                 =  96, // <Expression>
        SYMBOL_EXPRESSIONOPT              =  97, // <Expression Opt>
        SYMBOL_FIELDDEC                   =  98, // <Field Dec>
        SYMBOL_FINALLYCLAUSEOPT           =  99, // <Finally Clause Opt>
        SYMBOL_FORMALPARAM                = 100, // <Formal Param>
        SYMBOL_FORMALPARAMLIST            = 101, // <Formal Param List>
        SYMBOL_FORMALPARAMLISTOPT         = 102, // <Formal Param List Opt>
        SYMBOL_IMPORT2                    = 103, // <Import>
        SYMBOL_LITERAL                    = 104, // <Literal>
        SYMBOL_LOCALVARDECL               = 105, // <Local Var Decl>
        SYMBOL_LOGICALANDEXP              = 106, // <Logical And Exp>
        SYMBOL_LOGICALOREXP               = 107, // <Logical Or Exp>
        SYMBOL_LOGICALXOREXP              = 108, // <Logical Xor Exp>
        SYMBOL_MEMBERLIST                 = 109, // <Member List>
        SYMBOL_METHODCALL                 = 110, // <Method Call>
        SYMBOL_METHODCALLS                = 111, // <Method Calls>
        SYMBOL_METHODDEC                  = 112, // <Method Dec>
        SYMBOL_MODIFIER                   = 113, // <Modifier>
        SYMBOL_MODIFIERS                  = 114, // <Modifiers>
        SYMBOL_MULTEXP                    = 115, // <Mult Exp>
        SYMBOL_NL                         = 116, // <NL>
        SYMBOL_NLOREOF                    = 117, // <NL Or EOF>
        SYMBOL_NONARRAYTYPE               = 118, // <Non Array Type>
        SYMBOL_NORMALSTM                  = 119, // <Normal Stm>
        SYMBOL_OBJECTS                    = 120, // <Objects>
        SYMBOL_OREXP                      = 121, // <Or Exp>
        SYMBOL_PACKAGE2                   = 122, // <Package>
        SYMBOL_QUALIFIEDID                = 123, // <Qualified ID>
        SYMBOL_RANKSPECIFIER              = 124, // <Rank Specifier>
        SYMBOL_RANKSPECIFIERS             = 125, // <Rank Specifiers>
        SYMBOL_SHIFTEXP                   = 126, // <Shift Exp>
        SYMBOL_START                      = 127, // <Start>
        SYMBOL_STATEMENT                  = 128, // <Statement>
        SYMBOL_STATEMENTEXP               = 129, // <Statement Exp>
        SYMBOL_STMLIST                    = 130, // <Stm List>
        SYMBOL_SWITCHLABEL                = 131, // <Switch Label>
        SYMBOL_SWITCHSECTIONSOPT          = 132, // <Switch Sections Opt>
        SYMBOL_TYPE                       = 133, // <Type>
        SYMBOL_TYPEDECL                   = 134, // <Type Decl>
        SYMBOL_TYPEDECLOPT                = 135, // <Type Decl Opt>
        SYMBOL_TYPECASTEXP                = 136, // <Typecast Exp>
        SYMBOL_UNARYEXP                   = 137, // <Unary Exp>
        SYMBOL_VALIDID                    = 138, // <Valid ID>
        SYMBOL_VALUE                      = 139, // <Value>
        SYMBOL_VARIABLEDECLARATOR         = 140, // <Variable Declarator>
        SYMBOL_VARIABLEDECLARATORBASE     = 141, // <Variable Declarator Base>
        SYMBOL_VARIABLEINITIALIZER        = 142, // <Variable Initializer>
        SYMBOL_VARIABLEINITIALIZERLIST    = 143, // <Variable Initializer List>
        SYMBOL_VARIABLEINITIALIZERLISTOPT = 144  // <Variable Initializer List Opt>
    };

    enum RuleConstants : int
    {
        RULE_NL_NEWLINE                                       =   0, // <NL> ::= NewLine <NL>
        RULE_NL_NEWLINE2                                      =   1, // <NL> ::= NewLine
        RULE_NLOREOF                                          =   2, // <NL Or EOF> ::= <NL>
        RULE_NLOREOF2                                         =   3, // <NL Or EOF> ::= 
        RULE_START                                            =   4, // <Start> ::= <Package> <Start>
        RULE_START2                                           =   5, // <Start> ::= <Import> <Start>
        RULE_START3                                           =   6, // <Start> ::= <Type Decl Opt> <Start>
        RULE_START4                                           =   7, // <Start> ::= 
        RULE_OBJECTS_THIS                                     =   8, // <Objects> ::= this
        RULE_OBJECTS_BASE                                     =   9, // <Objects> ::= base
        RULE_VALIDID_IDENTIFIER                               =  10, // <Valid ID> ::= Identifier
        RULE_VALIDID                                          =  11, // <Valid ID> ::= <Objects>
        RULE_QUALIFIEDID                                      =  12, // <Qualified ID> ::= <Valid ID> <Member List>
        RULE_MEMBERLIST_MEMBERNAME                            =  13, // <Member List> ::= <Member List> MemberName
        RULE_MEMBERLIST                                       =  14, // <Member List> ::= 
        RULE_LITERAL_TRUE                                     =  15, // <Literal> ::= true
        RULE_LITERAL_FALSE                                    =  16, // <Literal> ::= false
        RULE_LITERAL_DECIMALLITERAL                           =  17, // <Literal> ::= DecimalLiteral
        RULE_LITERAL_HEXLITERAL                               =  18, // <Literal> ::= HexLiteral
        RULE_LITERAL_REALLITERAL                              =  19, // <Literal> ::= RealLiteral
        RULE_LITERAL_STRINGLITERAL                            =  20, // <Literal> ::= StringLiteral
        RULE_LITERAL_NULL                                     =  21, // <Literal> ::= null
        RULE_PACKAGE_PACKAGE                                  =  22, // <Package> ::= package <Qualified ID> <NL>
        RULE_IMPORT_IMPORT                                    =  23, // <Import> ::= import <Qualified ID> <NL>
        RULE_TYPE                                             =  24, // <Type> ::= <Non Array Type>
        RULE_TYPE2                                            =  25, // <Type> ::= <Non Array Type> <Rank Specifiers>
        RULE_NONARRAYTYPE                                     =  26, // <Non Array Type> ::= <Qualified ID>
        RULE_RANKSPECIFIERS                                   =  27, // <Rank Specifiers> ::= <Rank Specifier>
        RULE_RANKSPECIFIER_LBRACKET_RBRACKET                  =  28, // <Rank Specifier> ::= '[' <Expression> ']'
        RULE_EXPRESSIONOPT                                    =  29, // <Expression Opt> ::= <Expression>
        RULE_EXPRESSIONOPT2                                   =  30, // <Expression Opt> ::= 
        RULE_EXPRESSION_EQ                                    =  31, // <Expression> ::= <Or Exp> '=' <Expression>
        RULE_EXPRESSION                                       =  32, // <Expression> ::= <Or Exp>
        RULE_OREXP_PIPEPIPE                                   =  33, // <Or Exp> ::= <Or Exp> '||' <And Exp>
        RULE_OREXP                                            =  34, // <Or Exp> ::= <And Exp>
        RULE_ANDEXP_AMPAMP                                    =  35, // <And Exp> ::= <And Exp> '&&' <Logical Or Exp>
        RULE_ANDEXP                                           =  36, // <And Exp> ::= <Logical Or Exp>
        RULE_LOGICALOREXP_PIPE                                =  37, // <Logical Or Exp> ::= <Logical Or Exp> '|' <Logical Xor Exp>
        RULE_LOGICALOREXP                                     =  38, // <Logical Or Exp> ::= <Logical Xor Exp>
        RULE_LOGICALXOREXP_CARET                              =  39, // <Logical Xor Exp> ::= <Logical Xor Exp> '^' <Logical And Exp>
        RULE_LOGICALXOREXP                                    =  40, // <Logical Xor Exp> ::= <Logical And Exp>
        RULE_LOGICALANDEXP_AMP                                =  41, // <Logical And Exp> ::= <Logical And Exp> '&' <Equality Exp>
        RULE_LOGICALANDEXP                                    =  42, // <Logical And Exp> ::= <Equality Exp>
        RULE_EQUALITYEXP_EQEQ                                 =  43, // <Equality Exp> ::= <Equality Exp> '==' <Compare Exp>
        RULE_EQUALITYEXP_EXCLAMEQ                             =  44, // <Equality Exp> ::= <Equality Exp> '!=' <Compare Exp>
        RULE_EQUALITYEXP                                      =  45, // <Equality Exp> ::= <Compare Exp>
        RULE_COMPAREEXP_LT                                    =  46, // <Compare Exp> ::= <Compare Exp> '<' <Shift Exp>
        RULE_COMPAREEXP_GT                                    =  47, // <Compare Exp> ::= <Compare Exp> '>' <Shift Exp>
        RULE_COMPAREEXP_LTEQ                                  =  48, // <Compare Exp> ::= <Compare Exp> '<=' <Shift Exp>
        RULE_COMPAREEXP_GTEQ                                  =  49, // <Compare Exp> ::= <Compare Exp> '>=' <Shift Exp>
        RULE_COMPAREEXP                                       =  50, // <Compare Exp> ::= <Shift Exp>
        RULE_SHIFTEXP_LTLT                                    =  51, // <Shift Exp> ::= <Shift Exp> '<<' <Add Exp>
        RULE_SHIFTEXP_GTGT                                    =  52, // <Shift Exp> ::= <Shift Exp> '>>' <Add Exp>
        RULE_SHIFTEXP                                         =  53, // <Shift Exp> ::= <Add Exp>
        RULE_ADDEXP_PLUS                                      =  54, // <Add Exp> ::= <Add Exp> '+' <Mult Exp>
        RULE_ADDEXP_MINUS                                     =  55, // <Add Exp> ::= <Add Exp> '-' <Mult Exp>
        RULE_ADDEXP                                           =  56, // <Add Exp> ::= <Mult Exp>
        RULE_MULTEXP_TIMES                                    =  57, // <Mult Exp> ::= <Mult Exp> '*' <Typecast Exp>
        RULE_MULTEXP_DIV                                      =  58, // <Mult Exp> ::= <Mult Exp> '/' <Typecast Exp>
        RULE_MULTEXP_PERCENT                                  =  59, // <Mult Exp> ::= <Mult Exp> '%' <Typecast Exp>
        RULE_MULTEXP                                          =  60, // <Mult Exp> ::= <Typecast Exp>
        RULE_TYPECASTEXP_AS                                   =  61, // <Typecast Exp> ::= <Unary Exp> as <Qualified ID>
        RULE_TYPECASTEXP                                      =  62, // <Typecast Exp> ::= <Unary Exp>
        RULE_UNARYEXP_MINUS                                   =  63, // <Unary Exp> ::= '-' <Value>
        RULE_UNARYEXP_EXCLAM                                  =  64, // <Unary Exp> ::= '!' <Value>
        RULE_UNARYEXP_TILDE                                   =  65, // <Unary Exp> ::= '~' <Value>
        RULE_UNARYEXP                                         =  66, // <Unary Exp> ::= <Value>
        RULE_LOCALVARDECL                                     =  67, // <Local Var Decl> ::= <Variable Declarator>
        RULE_STATEMENTEXP                                     =  68, // <Statement Exp> ::= <Qualified ID> <Method Calls>
        RULE_STATEMENTEXP_LBRACKET_RBRACKET                   =  69, // <Statement Exp> ::= <Qualified ID> '[' <Expression> ']' <Method Calls>
        RULE_STATEMENTEXP_LPARAN_RPARAN                       =  70, // <Statement Exp> ::= <Qualified ID> '(' <Arg List Opt> ')' <Method Calls>
        RULE_STATEMENTEXP2                                    =  71, // <Statement Exp> ::= <Qualified ID> <Method Calls> <Assign Tail>
        RULE_STATEMENTEXP_LPARAN_RPARAN2                      =  72, // <Statement Exp> ::= <Qualified ID> '(' <Arg List Opt> ')' <Method Calls> <Assign Tail>
        RULE_STATEMENTEXP_LBRACKET_RBRACKET2                  =  73, // <Statement Exp> ::= <Qualified ID> '[' <Expression> ']' <Method Calls> <Assign Tail>
        RULE_STATEMENTEXP3                                    =  74, // <Statement Exp> ::= <Variable Declarator Base>
        RULE_ASSIGNTAIL_EQ                                    =  75, // <Assign Tail> ::= '=' <Expression>
        RULE_METHODCALLS                                      =  76, // <Method Calls> ::= <Method Call> <Method Calls>
        RULE_METHODCALLS2                                     =  77, // <Method Calls> ::= 
        RULE_METHODCALL_MEMBERNAME                            =  78, // <Method Call> ::= MemberName
        RULE_METHODCALL_MEMBERNAME_LPARAN_RPARAN              =  79, // <Method Call> ::= MemberName '(' <Arg List Opt> ')'
        RULE_METHODCALL_MEMBERNAME_LBRACKET_RBRACKET          =  80, // <Method Call> ::= MemberName '[' <Expression> ']'
        RULE_VALUE_LPARAN_RPARAN                              =  81, // <Value> ::= '(' <Expression> ')'
        RULE_VALUE                                            =  82, // <Value> ::= <Literal>
        RULE_VALUE2                                           =  83, // <Value> ::= <Statement Exp>
        RULE_VALUE_NEW_LPARAN_RPARAN                          =  84, // <Value> ::= new <Qualified ID> '(' <Arg List Opt> ')'
        RULE_VALUE_NEW_LBRACKET_RBRACKET                      =  85, // <Value> ::= new <Qualified ID> '[' <Expression> ']'
        RULE_ARGLISTOPT                                       =  86, // <Arg List Opt> ::= <Arg List>
        RULE_ARGLISTOPT2                                      =  87, // <Arg List Opt> ::= 
        RULE_ARGLIST_COMMA                                    =  88, // <Arg List> ::= <Arg List> ',' <Argument>
        RULE_ARGLIST                                          =  89, // <Arg List> ::= <Argument>
        RULE_ARGUMENT                                         =  90, // <Argument> ::= <Expression>
        RULE_STMLIST                                          =  91, // <Stm List> ::= <Stm List> <Statement>
        RULE_STMLIST2                                         =  92, // <Stm List> ::= 
        RULE_STATEMENT                                        =  93, // <Statement> ::= <Local Var Decl>
        RULE_STATEMENT_IF_END                                 =  94, // <Statement> ::= if <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_IF_ELSE_END                            =  95, // <Statement> ::= if <Expression> <NL> <Stm List> else <NL> <Stm List> end <NL>
        RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END              =  96, // <Statement> ::= foreach <Qualified ID> Identifier in <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END2             =  97, // <Statement> ::= foreach <Qualified ID> <Rank Specifiers> Identifier in <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT_WHILE_END                              =  98, // <Statement> ::= while <Expression> <NL> <Stm List> end <NL>
        RULE_STATEMENT2                                       =  99, // <Statement> ::= <Normal Stm>
        RULE_NORMALSTM_SWITCH_END                             = 100, // <Normal Stm> ::= switch <Expression> <NL> <Switch Sections Opt> end <NL>
        RULE_NORMALSTM_TRY_END                                = 101, // <Normal Stm> ::= try <NL> <Stm List> end <NL>
        RULE_NORMALSTM                                        = 102, // <Normal Stm> ::= <Catch Clauses>
        RULE_NORMALSTM2                                       = 103, // <Normal Stm> ::= <Finally Clause Opt>
        RULE_NORMALSTM_BREAK                                  = 104, // <Normal Stm> ::= break <NL>
        RULE_NORMALSTM_CONTINUE                               = 105, // <Normal Stm> ::= continue <NL>
        RULE_NORMALSTM_RETURN                                 = 106, // <Normal Stm> ::= return <Expression Opt> <NL>
        RULE_NORMALSTM_THROW                                  = 107, // <Normal Stm> ::= throw <Expression Opt> <NL>
        RULE_NORMALSTM3                                       = 108, // <Normal Stm> ::= <Expression> <NL>
        RULE_NORMALSTM4                                       = 109, // <Normal Stm> ::= <Constructor Init Stms> <NL>
        RULE_CONSTRUCTORINITSTMS_INIT_BASE_LPARAN_RPARAN      = 110, // <Constructor Init Stms> ::= init base '(' <Arg List Opt> ')'
        RULE_CONSTRUCTORINITSTMS_INIT_THIS_LPARAN_RPARAN      = 111, // <Constructor Init Stms> ::= init this '(' <Arg List Opt> ')'
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER                = 112, // <Variable Declarator Base> ::= <Qualified ID> Identifier
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER2               = 113, // <Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ             = 114, // <Variable Declarator Base> ::= <Qualified ID> Identifier '=' <Variable Initializer>
        RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ2            = 115, // <Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier '=' <Variable Initializer>
        RULE_VARIABLEDECLARATOR                               = 116, // <Variable Declarator> ::= <Variable Declarator Base> <NL>
        RULE_VARIABLEINITIALIZER                              = 117, // <Variable Initializer> ::= <Expression>
        RULE_VARIABLEINITIALIZER2                             = 118, // <Variable Initializer> ::= <Array Initializer>
        RULE_SWITCHSECTIONSOPT                                = 119, // <Switch Sections Opt> ::= <Switch Sections Opt> <Switch Label>
        RULE_SWITCHSECTIONSOPT2                               = 120, // <Switch Sections Opt> ::= 
        RULE_SWITCHLABEL_CASE_END                             = 121, // <Switch Label> ::= case <Expression> <NL> <Stm List> end <NL>
        RULE_SWITCHLABEL_DEFAULT_END                          = 122, // <Switch Label> ::= default <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSES                                     = 123, // <Catch Clauses> ::= <Catch Clause> <Catch Clauses>
        RULE_CATCHCLAUSES2                                    = 124, // <Catch Clauses> ::= 
        RULE_CATCHCLAUSE_CATCH_IDENTIFIER_END                 = 125, // <Catch Clause> ::= catch <Qualified ID> Identifier <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSE_CATCH_END                            = 126, // <Catch Clause> ::= catch <Qualified ID> <NL> <Stm List> end <NL>
        RULE_CATCHCLAUSE_CATCH_END2                           = 127, // <Catch Clause> ::= catch <NL> <Stm List> end <NL>
        RULE_FINALLYCLAUSEOPT_FINALLY_END                     = 128, // <Finally Clause Opt> ::= finally <NL> <Stm List> end <NL>
        RULE_FINALLYCLAUSEOPT                                 = 129, // <Finally Clause Opt> ::= <NL>
        RULE_ACCESS_PRIVATE                                   = 130, // <Access> ::= private
        RULE_ACCESS_PROTECTED                                 = 131, // <Access> ::= protected
        RULE_ACCESS_PUBLIC                                    = 132, // <Access> ::= public
        RULE_ACCESS_INTERNAL                                  = 133, // <Access> ::= internal
        RULE_MODIFIER_ABSTRACT                                = 134, // <Modifier> ::= abstract
        RULE_MODIFIER_OVERRIDE                                = 135, // <Modifier> ::= override
        RULE_MODIFIER_SEALED                                  = 136, // <Modifier> ::= sealed
        RULE_MODIFIER_STATIC                                  = 137, // <Modifier> ::= static
        RULE_MODIFIER                                         = 138, // <Modifier> ::= <Access>
        RULE_MODIFIERS                                        = 139, // <Modifiers> ::= <Modifier> <Modifiers>
        RULE_MODIFIERS2                                       = 140, // <Modifiers> ::= 
        RULE_CLASSDECL_CLASS_IDENTIFIER_END                   = 141, // <Class Decl> ::= <Modifiers> class Identifier <Class Base Opt> <NL> <Class Item Decs Opt> end <NL Or EOF>
        RULE_CLASSBASEOPT_INHERITS                            = 142, // <Class Base Opt> ::= inherits <Non Array Type>
        RULE_CLASSBASEOPT                                     = 143, // <Class Base Opt> ::= 
        RULE_CLASSITEMDECSOPT                                 = 144, // <Class Item Decs Opt> ::= <Class Item Decs Opt> <Class Item>
        RULE_CLASSITEMDECSOPT2                                = 145, // <Class Item Decs Opt> ::= 
        RULE_CLASSITEM                                        = 146, // <Class Item> ::= <Method Dec>
        RULE_CLASSITEM2                                       = 147, // <Class Item> ::= <Constructor Dec>
        RULE_CLASSITEM3                                       = 148, // <Class Item> ::= <Destructor Dec>
        RULE_CLASSITEM4                                       = 149, // <Class Item> ::= <Type Decl>
        RULE_CLASSITEM5                                       = 150, // <Class Item> ::= <Field Dec>
        RULE_FIELDDEC_IDENTIFIER                              = 151, // <Field Dec> ::= <Modifiers> <Type> Identifier <NL>
        RULE_FIELDDEC_IDENTIFIER_EQ                           = 152, // <Field Dec> ::= <Modifiers> <Type> Identifier '=' <Expression> <NL>
        RULE_METHODDEC_IDENTIFIER_LPARAN_RPARAN_END           = 153, // <Method Dec> ::= <Modifiers> <Type> Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
        RULE_FORMALPARAMLISTOPT                               = 154, // <Formal Param List Opt> ::= <Formal Param List>
        RULE_FORMALPARAMLISTOPT2                              = 155, // <Formal Param List Opt> ::= 
        RULE_FORMALPARAMLIST                                  = 156, // <Formal Param List> ::= <Formal Param>
        RULE_FORMALPARAMLIST_COMMA                            = 157, // <Formal Param List> ::= <Formal Param List> ',' <Formal Param>
        RULE_FORMALPARAM_IDENTIFIER                           = 158, // <Formal Param> ::= <Type> Identifier
        RULE_TYPEDECL                                         = 159, // <Type Decl> ::= <Class Decl>
        RULE_TYPEDECLOPT                                      = 160, // <Type Decl Opt> ::= <Type Decl>
        RULE_TYPEDECLOPT2                                     = 161, // <Type Decl Opt> ::= <NL>
        RULE_CONSTRUCTORDEC_CTOR_IDENTIFIER_LPARAN_RPARAN_END = 162, // <Constructor Dec> ::= <Modifiers> ctor Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
        RULE_DESTRUCTORDEC_DCTOR_IDENTIFIER_LPARAN_RPARAN_END = 163, // <Destructor Dec> ::= <Modifiers> dctor Identifier '(' ')' <NL> <Stm List> end <NL>
        RULE_ARRAYINITIALIZER_LBRACKET_RBRACKET               = 164, // <Array Initializer> ::= '[' <Variable Initializer List Opt> ']'
        RULE_ARRAYINITIALIZER_LBRACKET_COMMA_RBRACKET         = 165, // <Array Initializer> ::= '[' <Variable Initializer List> ',' ']'
        RULE_VARIABLEINITIALIZERLISTOPT                       = 166, // <Variable Initializer List Opt> ::= <Variable Initializer List>
        RULE_VARIABLEINITIALIZERLISTOPT2                      = 167, // <Variable Initializer List Opt> ::= 
        RULE_VARIABLEINITIALIZERLIST                          = 168, // <Variable Initializer List> ::= <Variable Initializer>
        RULE_VARIABLEINITIALIZERLIST_COMMA                    = 169  // <Variable Initializer List> ::= <Variable Initializer List> ',' <Variable Initializer>
    };

    public class MyParser
    {
        private LALRParser parser;

        public MyParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnReduce += new LALRParser.ReduceHandler(ReduceEvent);
            parser.OnTokenRead += new LALRParser.TokenReadHandler(TokenReadEvent);
            parser.OnAccept += new LALRParser.AcceptHandler(AcceptEvent);
            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public void Parse(string source)
        {
            parser.Parse(source);

        }

        private void TokenReadEvent(LALRParser parser, TokenReadEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        private Object CreateObject(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //(Whitespace)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMENTEND :
                //(Comment End)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMENTLINE :
                //(Comment Line)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMENTSTART :
                //(Comment Start)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLAM :
                //'!'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLAMEQ :
                //'!='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PERCENT :
                //'%'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_AMP :
                //'&'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_AMPAMP :
                //'&&'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPARAN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPARAN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMA :
                //','
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACKET :
                //'['
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RBRACKET :
                //']'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CARET :
                //'^'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PIPE :
                //'|'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PIPEPIPE :
                //'||'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TILDE :
                //'~'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTLT :
                //'<<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQEQ :
                //'=='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTGT :
                //'>>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ABSTRACT :
                //abstract
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_AS :
                //as
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BASE :
                //base
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BREAK :
                //break
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CASE :
                //case
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CATCH :
                //catch
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASS :
                //class
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONTINUE :
                //continue
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CTOR :
                //ctor
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DCTOR :
                //dctor
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECIMALLITERAL :
                //DecimalLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DEFAULT :
                //default
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ELSE :
                //else
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_END :
                //end
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FALSE :
                //false
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FINALLY :
                //finally
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FOREACH :
                //foreach
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HEXLITERAL :
                //HexLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //Identifier
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IF :
                //if
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORT :
                //import
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IN :
                //in
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INHERITS :
                //inherits
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INIT :
                //init
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTERNAL :
                //internal
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MEMBERNAME :
                //MemberName
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEW :
                //new
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEWLINE :
                //NewLine
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NULL :
                //null
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OVERRIDE :
                //override
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PACKAGE :
                //package
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PRIVATE :
                //private
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PROTECTED :
                //protected
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PUBLIC :
                //public
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_REALLITERAL :
                //RealLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETURN :
                //return
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SEALED :
                //sealed
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATIC :
                //static
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRINGLITERAL :
                //StringLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCH :
                //switch
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_THIS :
                //this
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_THROW :
                //throw
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TRUE :
                //true
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TRY :
                //try
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILE :
                //while
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ACCESS :
                //<Access>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ADDEXP :
                //<Add Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ANDEXP :
                //<And Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ARGLIST :
                //<Arg List>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ARGLISTOPT :
                //<Arg List Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ARGUMENT :
                //<Argument>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ARRAYINITIALIZER :
                //<Array Initializer>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ASSIGNTAIL :
                //<Assign Tail>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CATCHCLAUSE :
                //<Catch Clause>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CATCHCLAUSES :
                //<Catch Clauses>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASSBASEOPT :
                //<Class Base Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASSDECL :
                //<Class Decl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASSITEM :
                //<Class Item>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLASSITEMDECSOPT :
                //<Class Item Decs Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMPAREEXP :
                //<Compare Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONSTRUCTORDEC :
                //<Constructor Dec>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONSTRUCTORINITSTMS :
                //<Constructor Init Stms>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DESTRUCTORDEC :
                //<Destructor Dec>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQUALITYEXP :
                //<Equality Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION :
                //<Expression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSIONOPT :
                //<Expression Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FIELDDEC :
                //<Field Dec>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FINALLYCLAUSEOPT :
                //<Finally Clause Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORMALPARAM :
                //<Formal Param>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORMALPARAMLIST :
                //<Formal Param List>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORMALPARAMLISTOPT :
                //<Formal Param List Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORT2 :
                //<Import>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LITERAL :
                //<Literal>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOCALVARDECL :
                //<Local Var Decl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOGICALANDEXP :
                //<Logical And Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOGICALOREXP :
                //<Logical Or Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOGICALXOREXP :
                //<Logical Xor Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MEMBERLIST :
                //<Member List>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODCALL :
                //<Method Call>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODCALLS :
                //<Method Calls>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_METHODDEC :
                //<Method Dec>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MODIFIER :
                //<Modifier>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MODIFIERS :
                //<Modifiers>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MULTEXP :
                //<Mult Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NL :
                //<NL>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NLOREOF :
                //<NL Or EOF>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NONARRAYTYPE :
                //<Non Array Type>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NORMALSTM :
                //<Normal Stm>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OBJECTS :
                //<Objects>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OREXP :
                //<Or Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PACKAGE2 :
                //<Package>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_QUALIFIEDID :
                //<Qualified ID>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RANKSPECIFIER :
                //<Rank Specifier>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RANKSPECIFIERS :
                //<Rank Specifiers>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SHIFTEXP :
                //<Shift Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_START :
                //<Start>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENT :
                //<Statement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENTEXP :
                //<Statement Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMLIST :
                //<Stm List>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHLABEL :
                //<Switch Label>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SWITCHSECTIONSOPT :
                //<Switch Sections Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPE :
                //<Type>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPEDECL :
                //<Type Decl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPEDECLOPT :
                //<Type Decl Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TYPECASTEXP :
                //<Typecast Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_UNARYEXP :
                //<Unary Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALIDID :
                //<Valid ID>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE :
                //<Value>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEDECLARATOR :
                //<Variable Declarator>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEDECLARATORBASE :
                //<Variable Declarator Base>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEINITIALIZER :
                //<Variable Initializer>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEINITIALIZERLIST :
                //<Variable Initializer List>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VARIABLEINITIALIZERLISTOPT :
                //<Variable Initializer List Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        private void ReduceEvent(LALRParser parser, ReduceEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        public static Object CreateObject(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_NL_NEWLINE :
                //<NL> ::= NewLine <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NL_NEWLINE2 :
                //<NL> ::= NewLine
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NLOREOF :
                //<NL Or EOF> ::= <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NLOREOF2 :
                //<NL Or EOF> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_START :
                //<Start> ::= <Package> <Start>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_START2 :
                //<Start> ::= <Import> <Start>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_START3 :
                //<Start> ::= <Type Decl Opt> <Start>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_START4 :
                //<Start> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_OBJECTS_THIS :
                //<Objects> ::= this
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_OBJECTS_BASE :
                //<Objects> ::= base
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALIDID_IDENTIFIER :
                //<Valid ID> ::= Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALIDID :
                //<Valid ID> ::= <Objects>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_QUALIFIEDID :
                //<Qualified ID> ::= <Valid ID> <Member List>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MEMBERLIST_MEMBERNAME :
                //<Member List> ::= <Member List> MemberName
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MEMBERLIST :
                //<Member List> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_TRUE :
                //<Literal> ::= true
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_FALSE :
                //<Literal> ::= false
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_DECIMALLITERAL :
                //<Literal> ::= DecimalLiteral
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_HEXLITERAL :
                //<Literal> ::= HexLiteral
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_REALLITERAL :
                //<Literal> ::= RealLiteral
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_STRINGLITERAL :
                //<Literal> ::= StringLiteral
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LITERAL_NULL :
                //<Literal> ::= null
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PACKAGE_PACKAGE :
                //<Package> ::= package <Qualified ID> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_IMPORT_IMPORT :
                //<Import> ::= import <Qualified ID> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPE :
                //<Type> ::= <Non Array Type>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPE2 :
                //<Type> ::= <Non Array Type> <Rank Specifiers>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NONARRAYTYPE :
                //<Non Array Type> ::= <Qualified ID>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RANKSPECIFIERS :
                //<Rank Specifiers> ::= <Rank Specifier>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RANKSPECIFIER_LBRACKET_RBRACKET :
                //<Rank Specifier> ::= '[' <Expression> ']'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSIONOPT :
                //<Expression Opt> ::= <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSIONOPT2 :
                //<Expression Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQ :
                //<Expression> ::= <Or Exp> '=' <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION :
                //<Expression> ::= <Or Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_OREXP_PIPEPIPE :
                //<Or Exp> ::= <Or Exp> '||' <And Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_OREXP :
                //<Or Exp> ::= <And Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ANDEXP_AMPAMP :
                //<And Exp> ::= <And Exp> '&&' <Logical Or Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ANDEXP :
                //<And Exp> ::= <Logical Or Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOGICALOREXP_PIPE :
                //<Logical Or Exp> ::= <Logical Or Exp> '|' <Logical Xor Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOGICALOREXP :
                //<Logical Or Exp> ::= <Logical Xor Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOGICALXOREXP_CARET :
                //<Logical Xor Exp> ::= <Logical Xor Exp> '^' <Logical And Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOGICALXOREXP :
                //<Logical Xor Exp> ::= <Logical And Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOGICALANDEXP_AMP :
                //<Logical And Exp> ::= <Logical And Exp> '&' <Equality Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOGICALANDEXP :
                //<Logical And Exp> ::= <Equality Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EQUALITYEXP_EQEQ :
                //<Equality Exp> ::= <Equality Exp> '==' <Compare Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EQUALITYEXP_EXCLAMEQ :
                //<Equality Exp> ::= <Equality Exp> '!=' <Compare Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EQUALITYEXP :
                //<Equality Exp> ::= <Compare Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMPAREEXP_LT :
                //<Compare Exp> ::= <Compare Exp> '<' <Shift Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMPAREEXP_GT :
                //<Compare Exp> ::= <Compare Exp> '>' <Shift Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMPAREEXP_LTEQ :
                //<Compare Exp> ::= <Compare Exp> '<=' <Shift Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMPAREEXP_GTEQ :
                //<Compare Exp> ::= <Compare Exp> '>=' <Shift Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMPAREEXP :
                //<Compare Exp> ::= <Shift Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXP_LTLT :
                //<Shift Exp> ::= <Shift Exp> '<<' <Add Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXP_GTGT :
                //<Shift Exp> ::= <Shift Exp> '>>' <Add Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SHIFTEXP :
                //<Shift Exp> ::= <Add Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ADDEXP_PLUS :
                //<Add Exp> ::= <Add Exp> '+' <Mult Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ADDEXP_MINUS :
                //<Add Exp> ::= <Add Exp> '-' <Mult Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ADDEXP :
                //<Add Exp> ::= <Mult Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MULTEXP_TIMES :
                //<Mult Exp> ::= <Mult Exp> '*' <Typecast Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MULTEXP_DIV :
                //<Mult Exp> ::= <Mult Exp> '/' <Typecast Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MULTEXP_PERCENT :
                //<Mult Exp> ::= <Mult Exp> '%' <Typecast Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MULTEXP :
                //<Mult Exp> ::= <Typecast Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPECASTEXP_AS :
                //<Typecast Exp> ::= <Unary Exp> as <Qualified ID>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPECASTEXP :
                //<Typecast Exp> ::= <Unary Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_UNARYEXP_MINUS :
                //<Unary Exp> ::= '-' <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_UNARYEXP_EXCLAM :
                //<Unary Exp> ::= '!' <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_UNARYEXP_TILDE :
                //<Unary Exp> ::= '~' <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_UNARYEXP :
                //<Unary Exp> ::= <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOCALVARDECL :
                //<Local Var Decl> ::= <Variable Declarator>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP :
                //<Statement Exp> ::= <Qualified ID> <Method Calls>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET :
                //<Statement Exp> ::= <Qualified ID> '[' <Expression> ']' <Method Calls>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN :
                //<Statement Exp> ::= <Qualified ID> '(' <Arg List Opt> ')' <Method Calls>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP2 :
                //<Statement Exp> ::= <Qualified ID> <Method Calls> <Assign Tail>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP_LPARAN_RPARAN2 :
                //<Statement Exp> ::= <Qualified ID> '(' <Arg List Opt> ')' <Method Calls> <Assign Tail>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP_LBRACKET_RBRACKET2 :
                //<Statement Exp> ::= <Qualified ID> '[' <Expression> ']' <Method Calls> <Assign Tail>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENTEXP3 :
                //<Statement Exp> ::= <Variable Declarator Base>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ASSIGNTAIL_EQ :
                //<Assign Tail> ::= '=' <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_METHODCALLS :
                //<Method Calls> ::= <Method Call> <Method Calls>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_METHODCALLS2 :
                //<Method Calls> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_METHODCALL_MEMBERNAME :
                //<Method Call> ::= MemberName
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_METHODCALL_MEMBERNAME_LPARAN_RPARAN :
                //<Method Call> ::= MemberName '(' <Arg List Opt> ')'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_METHODCALL_MEMBERNAME_LBRACKET_RBRACKET :
                //<Method Call> ::= MemberName '[' <Expression> ']'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_LPARAN_RPARAN :
                //<Value> ::= '(' <Expression> ')'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE :
                //<Value> ::= <Literal>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE2 :
                //<Value> ::= <Statement Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_NEW_LPARAN_RPARAN :
                //<Value> ::= new <Qualified ID> '(' <Arg List Opt> ')'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_NEW_LBRACKET_RBRACKET :
                //<Value> ::= new <Qualified ID> '[' <Expression> ']'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARGLISTOPT :
                //<Arg List Opt> ::= <Arg List>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARGLISTOPT2 :
                //<Arg List Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARGLIST_COMMA :
                //<Arg List> ::= <Arg List> ',' <Argument>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARGLIST :
                //<Arg List> ::= <Argument>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARGUMENT :
                //<Argument> ::= <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STMLIST :
                //<Stm List> ::= <Stm List> <Statement>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STMLIST2 :
                //<Stm List> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT :
                //<Statement> ::= <Local Var Decl>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_IF_END :
                //<Statement> ::= if <Expression> <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_IF_ELSE_END :
                //<Statement> ::= if <Expression> <NL> <Stm List> else <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END :
                //<Statement> ::= foreach <Qualified ID> Identifier in <Expression> <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_FOREACH_IDENTIFIER_IN_END2 :
                //<Statement> ::= foreach <Qualified ID> <Rank Specifiers> Identifier in <Expression> <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_WHILE_END :
                //<Statement> ::= while <Expression> <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_STATEMENT2 :
                //<Statement> ::= <Normal Stm>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM_SWITCH_END :
                //<Normal Stm> ::= switch <Expression> <NL> <Switch Sections Opt> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM_TRY_END :
                //<Normal Stm> ::= try <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM :
                //<Normal Stm> ::= <Catch Clauses>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM2 :
                //<Normal Stm> ::= <Finally Clause Opt>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM_BREAK :
                //<Normal Stm> ::= break <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM_CONTINUE :
                //<Normal Stm> ::= continue <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM_RETURN :
                //<Normal Stm> ::= return <Expression Opt> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM_THROW :
                //<Normal Stm> ::= throw <Expression Opt> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM3 :
                //<Normal Stm> ::= <Expression> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NORMALSTM4 :
                //<Normal Stm> ::= <Constructor Init Stms> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORINITSTMS_INIT_BASE_LPARAN_RPARAN :
                //<Constructor Init Stms> ::= init base '(' <Arg List Opt> ')'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORINITSTMS_INIT_THIS_LPARAN_RPARAN :
                //<Constructor Init Stms> ::= init this '(' <Arg List Opt> ')'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER :
                //<Variable Declarator Base> ::= <Qualified ID> Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER2 :
                //<Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ :
                //<Variable Declarator Base> ::= <Qualified ID> Identifier '=' <Variable Initializer>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATORBASE_IDENTIFIER_EQ2 :
                //<Variable Declarator Base> ::= <Qualified ID> <Rank Specifiers> Identifier '=' <Variable Initializer>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEDECLARATOR :
                //<Variable Declarator> ::= <Variable Declarator Base> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZER :
                //<Variable Initializer> ::= <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZER2 :
                //<Variable Initializer> ::= <Array Initializer>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SWITCHSECTIONSOPT :
                //<Switch Sections Opt> ::= <Switch Sections Opt> <Switch Label>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SWITCHSECTIONSOPT2 :
                //<Switch Sections Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SWITCHLABEL_CASE_END :
                //<Switch Label> ::= case <Expression> <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SWITCHLABEL_DEFAULT_END :
                //<Switch Label> ::= default <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CATCHCLAUSES :
                //<Catch Clauses> ::= <Catch Clause> <Catch Clauses>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CATCHCLAUSES2 :
                //<Catch Clauses> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_IDENTIFIER_END :
                //<Catch Clause> ::= catch <Qualified ID> Identifier <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_END :
                //<Catch Clause> ::= catch <Qualified ID> <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CATCHCLAUSE_CATCH_END2 :
                //<Catch Clause> ::= catch <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FINALLYCLAUSEOPT_FINALLY_END :
                //<Finally Clause Opt> ::= finally <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FINALLYCLAUSEOPT :
                //<Finally Clause Opt> ::= <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ACCESS_PRIVATE :
                //<Access> ::= private
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ACCESS_PROTECTED :
                //<Access> ::= protected
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ACCESS_PUBLIC :
                //<Access> ::= public
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ACCESS_INTERNAL :
                //<Access> ::= internal
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_ABSTRACT :
                //<Modifier> ::= abstract
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_OVERRIDE :
                //<Modifier> ::= override
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_SEALED :
                //<Modifier> ::= sealed
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIER_STATIC :
                //<Modifier> ::= static
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIER :
                //<Modifier> ::= <Access>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIERS :
                //<Modifiers> ::= <Modifier> <Modifiers>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_MODIFIERS2 :
                //<Modifiers> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSDECL_CLASS_IDENTIFIER_END :
                //<Class Decl> ::= <Modifiers> class Identifier <Class Base Opt> <NL> <Class Item Decs Opt> end <NL Or EOF>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSBASEOPT_INHERITS :
                //<Class Base Opt> ::= inherits <Non Array Type>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSBASEOPT :
                //<Class Base Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEMDECSOPT :
                //<Class Item Decs Opt> ::= <Class Item Decs Opt> <Class Item>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEMDECSOPT2 :
                //<Class Item Decs Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEM :
                //<Class Item> ::= <Method Dec>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEM2 :
                //<Class Item> ::= <Constructor Dec>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEM3 :
                //<Class Item> ::= <Destructor Dec>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEM4 :
                //<Class Item> ::= <Type Decl>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLASSITEM5 :
                //<Class Item> ::= <Field Dec>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FIELDDEC_IDENTIFIER :
                //<Field Dec> ::= <Modifiers> <Type> Identifier <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FIELDDEC_IDENTIFIER_EQ :
                //<Field Dec> ::= <Modifiers> <Type> Identifier '=' <Expression> <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_METHODDEC_IDENTIFIER_LPARAN_RPARAN_END :
                //<Method Dec> ::= <Modifiers> <Type> Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMLISTOPT :
                //<Formal Param List Opt> ::= <Formal Param List>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMLISTOPT2 :
                //<Formal Param List Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMLIST :
                //<Formal Param List> ::= <Formal Param>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAMLIST_COMMA :
                //<Formal Param List> ::= <Formal Param List> ',' <Formal Param>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_FORMALPARAM_IDENTIFIER :
                //<Formal Param> ::= <Type> Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPEDECL :
                //<Type Decl> ::= <Class Decl>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLOPT :
                //<Type Decl Opt> ::= <Type Decl>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TYPEDECLOPT2 :
                //<Type Decl Opt> ::= <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CONSTRUCTORDEC_CTOR_IDENTIFIER_LPARAN_RPARAN_END :
                //<Constructor Dec> ::= <Modifiers> ctor Identifier '(' <Formal Param List Opt> ')' <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_DESTRUCTORDEC_DCTOR_IDENTIFIER_LPARAN_RPARAN_END :
                //<Destructor Dec> ::= <Modifiers> dctor Identifier '(' ')' <NL> <Stm List> end <NL>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARRAYINITIALIZER_LBRACKET_RBRACKET :
                //<Array Initializer> ::= '[' <Variable Initializer List Opt> ']'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ARRAYINITIALIZER_LBRACKET_COMMA_RBRACKET :
                //<Array Initializer> ::= '[' <Variable Initializer List> ',' ']'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZERLISTOPT :
                //<Variable Initializer List Opt> ::= <Variable Initializer List>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZERLISTOPT2 :
                //<Variable Initializer List Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZERLIST :
                //<Variable Initializer List> ::= <Variable Initializer>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VARIABLEINITIALIZERLIST_COMMA :
                //<Variable Initializer List> ::= <Variable Initializer List> ',' <Variable Initializer>
                //todo: Create a new object using the stored user objects.
                return null;

            }
            throw new RuleException("Unknown rule");
        }

        private void AcceptEvent(LALRParser parser, AcceptEventArgs args)
        {
            //todo: Use your fully reduced args.Token.UserObject
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            //todo: Report message to UI?
        }


    }
}
