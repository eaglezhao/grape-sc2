﻿using System;
using System.Collections.Generic;
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
                string errorMessage = "";
                if (GrapeAstVisitor.IsTypeInTypeArray(expression.GetType(), memberExpressionValidator.NodeType)) {
                    return memberExpressionValidator.ValidateNode(expression);
                } else if (expression.GetType() == typeof(GrapeAddExpression)) {
                    GrapeAddExpression addExpression = expression as GrapeAddExpression;
                    if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Left, "int_base") && typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Right, "int_base", ref errorMessage)) || !(typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Left, "fixed_base", ref errorMessage) && typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Right, "fixed_base", ref errorMessage)) || !(typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Left, "string_base", ref errorMessage) && typeCheckingUtils.DoesExpressionResolveToType(config, expression, addExpression.Right, "string_base", ref errorMessage))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve addition expressions to the same type or addition expressions resolve to a type that is unable to be merged. " + errorMessage, FileName = expression.FileName, Entity = addExpression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeConditionalExpression)) {
                    GrapeConditionalExpression conditionalExpression = expression as GrapeConditionalExpression;
                    if (conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryAnd || conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryOr) {
                        if (!typeCheckingUtils.DoesExpressionResolveToType(config, expression, conditionalExpression.Left, "int_base", ref errorMessage) || !typeCheckingUtils.DoesExpressionResolveToType(config, expression, conditionalExpression.Right, "int_base", ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'int_base'. " + errorMessage, FileName = expression.FileName, Entity = conditionalExpression });
                            if (!config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        if (!typeCheckingUtils.DoesExpressionResolveToType(config, expression, conditionalExpression.Left, "bool_base", ref errorMessage) || !typeCheckingUtils.DoesExpressionResolveToType(config, expression, conditionalExpression.Right, "bool_base", ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'bool_base'. " + errorMessage, FileName = expression.FileName, Entity = conditionalExpression });
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
                    if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Left, "int_base", ref errorMessage) && typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Right, "int_base", ref errorMessage)) || !(typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Left, "fixed_base", ref errorMessage) && typeCheckingUtils.DoesExpressionResolveToType(config, expression, multiplicationExpression.Right, "fixed_base", ref errorMessage))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve addition expressions to the same type or addition expressions resolve to a type that is unable to be merged. " + errorMessage, FileName = expression.FileName, Entity = expression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeNameofExpression)) {
                    GrapeNameofExpression nameofExpression = expression as GrapeNameofExpression;
                    if (nameofExpression.Value.GetType() != typeof(GrapeMemberExpression)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to type, field or function.", FileName = expression.FileName, Entity = expression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }

                    string qualifiedId = (nameofExpression.Value as GrapeMemberExpression).GetMemberExpressionQualifiedId();
                    GrapeEntity valueEntity = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(config, nameofExpression.Value as GrapeMemberExpression, expression, out errorMessage, false)))[0];
                    if (valueEntity == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find entity for expression '" + qualifiedId + "'. " + errorMessage, FileName = expression.FileName, Entity = expression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeClass c = expression.GetLogicalParentOfEntityType<GrapeClass>();
                    string[] modifiers = c.GetAppropriateModifiersForEntityAccess(config, valueEntity);
                    string potentialModifiers = valueEntity.GetPotentialModifiersOfEntity();
                    bool invalidModifiers = false;
                    if (modifiers != null) {
                        if (potentialModifiers == null) {
                            if (modifiers.Length == 1 && modifiers[0] == "public") {
                                invalidModifiers = false;
                            } else {
                                invalidModifiers = true;
                            }
                        } else {
                            foreach (string modifier in modifiers) {
                                bool hasModifier = false;
                                foreach (string entityModifier in potentialModifiers.Split(' ')) {
                                    if (entityModifier == modifier) {
                                        hasModifier = true;
                                        break;
                                    }
                                }

                                if (!hasModifier) {
                                    invalidModifiers = true;
                                    break;
                                }
                            }
                        }
                    } else {
                        invalidModifiers = true;
                    }

                    if (invalidModifiers) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot access member '" + qualifiedId + "'.", FileName = expression.FileName, Entity = expression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeShiftExpression)) {
                    GrapeShiftExpression shiftExpression = expression as GrapeShiftExpression;
                    if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, shiftExpression.Left, "int_base", ref errorMessage) && typeCheckingUtils.DoesExpressionResolveToType(config, expression, shiftExpression.Right, "int_base", ref errorMessage))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve shift expressions to the same type or addition expressions resolve to a type that is unable to be merged. " + errorMessage, FileName = expression.FileName, Entity = expression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }
                } else if (expression.GetType() == typeof(GrapeStackExpression)) {
                    return ValidateExpression(config, ((GrapeStackExpression)expression).Child);
                } else if (expression.GetType() == typeof(GrapeTypecastExpression)) {
                    GrapeTypecastExpression typecastExpression = expression as GrapeTypecastExpression;
                    if (typeCheckingUtils.DoesExpressionResolveToType(config, typecastExpression, typecastExpression.Value, "text_base")) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot cast from type 'text_base'.", FileName = expression.FileName, Entity = expression });
                        if (!config.ContinueOnError) {
                            return false;
                        }
                    }

                    return typeCheckingUtils.DoesExpressionResolveToType(config, expression, typecastExpression.Value, typecastExpression.Type);
                } else if (expression.GetType() == typeof(GrapeUnaryExpression)) {
                    GrapeUnaryExpression unaryExpression = expression as GrapeUnaryExpression;
                    if (unaryExpression.Type == GrapeUnaryExpression.GrapeUnaryExpressionType.Not) {
                        if (!typeCheckingUtils.DoesExpressionResolveToType(config, expression, unaryExpression.Value, "bool_base", ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'bool_base'. " + errorMessage, FileName = expression.FileName, Entity = expression });
                            if (!config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (!(typeCheckingUtils.DoesExpressionResolveToType(config, expression, unaryExpression.Value, "int_base", ref errorMessage))) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'int_base'. " + errorMessage, FileName = expression.FileName, Entity = expression });
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
