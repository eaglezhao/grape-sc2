using System;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeExpressionExtensionMethods {
        public static string GetMemberExpressionQualifiedId(this GrapeMemberExpression expression) {
            string result = "";
            GrapeMemberExpression currentExpression = expression;
            while (currentExpression != null) {
                GrapeExpression member = currentExpression.Member;
                while (member != null) {
                    if (member is GrapeIdentifierExpression) {
                        result += ((GrapeIdentifierExpression)member).Identifier + ".";
                        break;
                    } else if (member is GrapeMemberExpression) {
                        member = ((GrapeMemberExpression)member).Member;
                    }
                }

                if (currentExpression.Next == null && currentExpression.Member is GrapeMemberExpression) {
                    currentExpression = currentExpression.Member as GrapeMemberExpression;
                }

                currentExpression = currentExpression.Next;
            }

            result = result.Trim('.');
            return result;
        }
    }
}
