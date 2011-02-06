using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export]
    internal sealed class GrapeExpressionValidator {
        [Import]
        private GrapeMemberExpressionValidator memberExpressionValidator = null;
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public bool ValidateExpression(GrapeCodeGeneratorConfiguration config, GrapeExpression expression) {
            if (expression != null) {
                GrapeAst ast = config.Ast;
                if (GrapeAstVisitor.IsTypeInTypeArray(expression.GetType(), memberExpressionValidator.NodeType)) {
                    return memberExpressionValidator.ValidateNode(expression);
                } else if (expression.GetType() == typeof(GrapeAddExpression)) {
                    GrapeAddExpression addExpression = expression as GrapeAddExpression;
                    if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Left, "int_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Right, "int_base")) || !(typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Left, "fixed_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Right, "fixed_base")) || !(typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Left, "string_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Right, "string_base"))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve addition expressions to the same type or addition expressions resolve to a type that is unable to be merged.", FileName = expression.FileName, Offset = expression.Offset, Length = expression.Length });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeConditionalExpression)) {
                    GrapeConditionalExpression conditionalExpression = expression as GrapeConditionalExpression;
                    if (conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryAnd || conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryOr) {

                    } else {
                        if (!typeCheckingUtils.DoesExpressionResolveToType(config, expression, conditionalExpression.Left, "bool_base") || !typeCheckingUtils.DoesExpressionResolveToType(config, expression, conditionalExpression.Right, "bool_base")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'bool_base'.", FileName = expression.FileName, Offset = expression.Offset, Length = expression.Length });
                            if (!config.ContinueOnError) {
                                return false;
                            }
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeMemberExpression)) {
                    GrapeMemberExpression memberExpression = expression as GrapeMemberExpression;
                    return ValidateExpression(config, memberExpression.Member);
                } else if (expression.GetType() == typeof(GrapeMultiplicationExpression)) {
                    GrapeMultiplicationExpression multiplicationExpression = expression as GrapeMultiplicationExpression;
                    if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Left, "int_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Right, "int_base")) || !(typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Left, "fixed_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Right, "fixed_base"))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve addition expressions to the same type or addition expressions resolve to a type that is unable to be merged.", FileName = expression.FileName, Offset = expression.Offset, Length = expression.Length });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeShiftExpression)) {
                    GrapeShiftExpression shiftExpression = expression as GrapeShiftExpression;
                    if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, shiftExpression.Left, "int_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, shiftExpression.Right, "int_base"))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve shift expressions to the same type or addition expressions resolve to a type that is unable to be merged.", FileName = expression.FileName, Offset = expression.Offset, Length = expression.Length });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeStackExpression)) {
                    return ValidateExpression(config, ((GrapeStackExpression)expression).Child);
                } else if (expression.GetType() == typeof(GrapeTypecastExpression)) {
                    GrapeTypecastExpression typecastExpression = expression as GrapeTypecastExpression;
                    return typeCheckingUtils.DoesExpressionResolveToType(config, expression, typecastExpression.Value, typecastExpression.Type);
                } else if (expression.GetType() == typeof(GrapeUnaryExpression)) {
                    GrapeUnaryExpression unaryExpression = expression as GrapeUnaryExpression;
                    if (unaryExpression.Type == GrapeUnaryExpression.GrapeUnaryExpressionType.Not) {
                        if (!typeCheckingUtils.DoesExpressionResolveToType(config, expression, unaryExpression.Value, "bool_base")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'bool_base'.", FileName = expression.FileName, Offset = expression.Offset, Length = expression.Length });
                            if (!config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, unaryExpression.Value, "int_base"))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'int_base'.", FileName = expression.FileName, Offset = expression.Offset, Length = expression.Length });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
