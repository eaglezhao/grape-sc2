using System;
using Vestras.StarCraft2.Grape.Core.Ast;
using System.ComponentModel.Composition;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export]
    internal sealed class GrapeExpressionGenerator {

        public GrapeCodeGeneratorConfiguration Config { get; set; } //<- public var

        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public string VisitExpression( GrapeExpression e ){
            string s="";

            if (e.GetType() ==  typeof(GrapeLiteralExpression)) {
                GrapeLiteralExpression a = (GrapeLiteralExpression)e;
                s = a.Value;
            } else if (e.GetType() == typeof(GrapeIdentifierExpression)) {
                GrapeIdentifierExpression a = (GrapeIdentifierExpression)e;
                s = a.Identifier;

            } else if (e.GetType() == typeof(GrapeAddExpression)) {
                GrapeAddExpression a=(GrapeAddExpression)e;

                if (a.Type == GrapeAddExpression.GrapeAddExpressionType.Addition) {
                    s= VisitExpression(a.Left) + "+" + VisitExpression(a.Right);
                } else { //Subtraction
                    s= VisitExpression(a.Left) + "+" + VisitExpression(a.Right); 
                }


            } else if (e.GetType() == typeof(GrapeMultiplicationExpression)) {
                GrapeMultiplicationExpression a = (GrapeMultiplicationExpression)e;

                if (a.Type == GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Multiplication) {
                    s = VisitExpression(a.Left) + "*" + VisitExpression(a.Right);
                } else if (a.Type == GrapeMultiplicationExpression.GrapeMultiplicationExpressionType.Division) {
                    s = VisitExpression(a.Left) + "/" + VisitExpression(a.Right);
                } else{ //Mod
                    s = VisitExpression(a.Left) + "%" + VisitExpression(a.Right);
                }

            } else if (e.GetType() == typeof(GrapeShiftExpression)) {
                GrapeShiftExpression a = (GrapeShiftExpression)e;

                if (a.Type == GrapeShiftExpression.GrapeShiftExpressionType.ShiftLeft) {
                    s = VisitExpression(a.Left) + "<<" + VisitExpression(a.Right);
                } else{ //ShiftLeft
                    s = VisitExpression(a.Left) + ">>" + VisitExpression(a.Right);
                }

            } else if (e.GetType() == typeof(GrapeTypecastExpression)){
                GrapeTypecastExpression a = (GrapeTypecastExpression)e;

                string conv = typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, a.Value);
                conv +="->";
                conv += typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, a.Type);


/*native int      BoolToInt (bool f);
native fixed    IntToFixed (int x);
native string   IntToString (int x);
native text     IntToText (int x);

const int c_fixedPrecisionAny = -1;
native int      FixedToInt (fixed x);
native string   FixedToString (fixed x, int precision); < -
native text     FixedToText (fixed x, int precision); < - 

native int      StringToInt (string x);
native fixed    StringToFixed (string x);*/

                switch(conv){
    
                    case "int_base->fixed_base":
                        s = "IntToFixed(" + VisitExpression(a.Value) + ")";
                        break;
                    case "int_base->string_base":
                        s = "IntToString(" + VisitExpression(a.Value) + ")";
                        break;

                    case "fixed_base->int_base":
                        s = "StringToInt(" + VisitExpression(a.Value) + ")";
                        break;
                    case "string_base->fixed_base":
                        s = "StringToFixed(" + VisitExpression(a.Value) + ")";
                        break;

                    default:
                        s = VisitExpression(a.Value);
                        break;
                }



                //int, fixed, string, bool, text
                

            }

            /*
            GrapeArrayAccessExpression,
            GrapeCallExpression,
            GrapeMemberExpression,
            GrapeObjectCreationExpression,
            GrapeTypecastExpression,
            GrapeUnaryExpression,
             */

            //if (encapsulate)
                s = "(" + s + ")";

            return s;
        }


    }
}
