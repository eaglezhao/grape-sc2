using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeDeleteStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeDeleteStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeDeleteStatement s = obj as GrapeDeleteStatement;
                if (s != null) {
                    string qualifiedId = s.Value is GrapeIdentifierExpression ? ((GrapeIdentifierExpression)s.Value).Identifier : s.Value is GrapeMemberExpression ? ((GrapeMemberExpression)s.Value).GetMemberExpressionQualifiedId() : typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, s.Value);
                    if (qualifiedId == "this" || qualifiedId == "base") {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot delete static type '" + qualifiedId + "'.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage = "";
                    if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.Value, "object", ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot delete object for expression that does not resolve to type 'object'. " + errorMessage, FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.Value, "void_base", ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot delete object for expression that resolves to type 'void_base'. " + errorMessage, FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeEntity entity = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(Config, s.Value as GrapeMemberExpression, s, out errorMessage)))[0];
                    if (entity != null && entity is GrapeClass) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot delete static type '" + qualifiedId + "'.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeClass c = s.GetLogicalParentOfEntityType<GrapeClass>();
                    string[] modifiers = c.GetAppropriateModifiersForEntityAccess(Config, entity);
                    string potentialModifiers = entity.GetPotentialModifiersOfEntity();
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
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot access member '" + qualifiedId + "'.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
