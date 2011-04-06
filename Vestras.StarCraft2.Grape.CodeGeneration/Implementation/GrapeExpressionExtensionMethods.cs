using System;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeExpressionExtensionMethods {
        public static string GetAccessExpressionQualifiedId(this GrapeAccessExpression expression) {
            string result = "";
            GrapeAccessExpression currentExpression = expression;
            while (currentExpression != null) {
                if (currentExpression is GrapeMemberExpression) {
                    GrapeMemberExpression currentMemberExpression = currentExpression as GrapeMemberExpression;
                    GrapeIdentifier identifier = currentMemberExpression.Identifier;
                    result += identifier.Name + ".";
                }

                currentExpression = currentExpression.Next;
            }

            result = result.Trim('.');
            return result;
        }

        public static GrapeAccessExpression GetAccessExpressionInAccessExpression(this GrapeAccessExpression expression) {
            if (expression.Next != null) {
                return expression.Next.GetAccessExpressionInAccessExpression();
            }

            return expression;
        }
    }
}
