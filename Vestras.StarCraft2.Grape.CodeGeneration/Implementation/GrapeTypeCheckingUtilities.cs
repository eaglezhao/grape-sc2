using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeTypeCheckingUtilities {
        private static Dictionary<GrapeLiteralExpression.GrapeLiteralExpressionType, string> literalTypes = new Dictionary<GrapeLiteralExpression.GrapeLiteralExpressionType, string>();

        public static bool IsTypeInClassInheritanceTree(GrapeAst ast, string typeName, GrapeClass c) {
            if (c.Inherits != null) {
                if (GetTypeNameForTypeAccessExpression(c.Inherits) == typeName) {
                    return true;
                }

                return IsTypeInClassInheritanceTree(ast, typeName, GrapeAstUtilities.GetClassWithNameFromImportedPackagesInFile(ast, GetTypeNameForTypeAccessExpression(c.Inherits), c.FileName));
            }

            return false;
        }

        public static string GetTypeNameForTypeAccessExpression(GrapeExpression expression) {
            if (expression is GrapeIdentifierExpression) {
                return ((GrapeIdentifierExpression)expression).Identifier;
            } else if (expression is GrapeMemberExpression) {
                return ((GrapeMemberExpression)expression).GetMemberExpressionQualifiedId();
            }

            return "";
        }

        public static bool DoesExpressionResolveToType(GrapeAst ast, GrapeExpression expression, string typeName) {
            if (expression is GrapeLiteralExpression) {
                if (typeName == literalTypes[((GrapeLiteralExpression)expression).Type]) {
                    return true;
                }

                return false;
            } else if (expression is GrapeConditionalExpression) {
                GrapeConditionalExpression conditionalExpression = expression as GrapeConditionalExpression;
                if (conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryAnd || conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryOr) {
                    return typeName == "int_base";
                }

                return typeName == "bool_base";
            }

            return false;
        }

        public static bool DoesExpressionResolveToType(GrapeAst ast, GrapeExpression expression, GrapeExpression type) {
            return DoesExpressionResolveToType(ast, expression, GetTypeNameForTypeAccessExpression(type));
        }

        public static bool DoesTypeExist(GrapeAst ast, string typeName, string fileName) {
            IList<string> segmentsInTypeName = GrapeAstUtilities.GetSegmentsInQualifiedId(typeName, true);
            string actualTypeName = segmentsInTypeName.Count > 0 ? segmentsInTypeName[segmentsInTypeName.Count - 1] : typeName;
            foreach (Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute> tuple in GalaxyNativeInterfaceAggregator.Types) {
                if (tuple.Item1.NativeAlias == actualTypeName) {
                    actualTypeName = tuple.Item1.TypeName;
                }
            }

            int lastDotIndex = typeName.LastIndexOf('.');
            if (lastDotIndex > -1) {
                typeName = typeName.Substring(0, lastDotIndex + 1) + actualTypeName;
            }

            GrapeClass c = GrapeAstUtilities.GetClassWithNameFromImportedPackagesInFile(ast, actualTypeName, fileName);
            return c != null;
        }

        public static bool DoesTypeExist(GrapeAst ast, GrapeExpression type, string fileName) {
            return DoesTypeExist(ast, GetTypeNameForTypeAccessExpression(type), fileName);
        }

        static GrapeTypeCheckingUtilities() {
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.True, "bool_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.False, "bool_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Hexadecimal, "int_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Int, "int_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Null, "object");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Real, "fixed_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.String, "string_base");
        }
    }
}
