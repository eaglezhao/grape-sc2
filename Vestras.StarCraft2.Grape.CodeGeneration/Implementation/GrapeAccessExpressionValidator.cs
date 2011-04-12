using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator)), Export]
    internal class GrapeAccessExpressionValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeAstUtilities astUtils = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] {
                    typeof(GrapeAccessExpression),
                    typeof(GrapeArrayExpression),
                    typeof(GrapeCallExpression),
                };
            }
        }

        public bool ValidateMethodSignatureAndOverloads(GrapeCallExpression callExpression, GrapeMethod method, GrapeModifier.GrapeModifierType modifiers, ref string errorMessage) {
            errorMessage = "";
            List<GrapeMethod> methods = new List<GrapeMethod>();
            methods.AddRange(astUtils.GetMethodsWithNameFromImportedPackagesInFile(Config, method.Name, method.FileName, method.GetLogicalParentOfEntityType<GrapeClass>()));
            List<GrapeMethod> methodsWithSignature = typeCheckingUtils.GetMethodsWithSignature(Config, methods, modifiers, method.Name, method.ReturnType, new List<GrapeExpression>(callExpression.Parameters), ref errorMessage);
            if (errorMessage != "") {
                errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = callExpression.FileName, Entity = callExpression });
                if (!Config.ContinueOnError) {
                    return false;
                }
            }

            if (methodsWithSignature.Count > 1) {
                errorMessage = "Multiple functions with signature '" + typeCheckingUtils.GetMethodSignatureString(Config, method.Name, method.ReturnType, new List<GrapeExpression>(callExpression.Parameters)) + "' found.";
                errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = callExpression.FileName, Entity = callExpression });
                if (!Config.ContinueOnError) {
                    return false;
                }
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeAccessExpression s = obj as GrapeAccessExpression;
                if (s != null) {
                    GrapeArrayExpression arrayExpression = s as GrapeArrayExpression;
                    GrapeCallExpression callExpression = s as GrapeCallExpression;
                    if (arrayExpression != null) {
                        string errorMessage = "";
                        if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, arrayExpression.ArrayIndex, "int_base", ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve array access expression to the type 'int_base'. " + errorMessage, FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (callExpression != null) {
                        string errorMessage;
                        IList<GrapeEntity> entities = new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForAccessExpression(Config, callExpression, callExpression, out errorMessage));
                        bool foundCorrectFunction = false;
                        if (entities.Count == 1 && entities[0] == null) {
                            string qualifiedId = callExpression.GetAccessExpressionQualifiedId();
                            foreach (GalaxyFunctionAttribute function in GalaxyNativeInterfaceAggregator.Functions) {
                                if (function.Name == qualifiedId) {
                                    if (!typeCheckingUtils.IsSignatureValid(Config, function, callExpression, out errorMessage)) {
                                        foundCorrectFunction = false;
                                        break;
                                    }

                                    foundCorrectFunction = true;
                                    break;
                                }
                            }

                            if (!foundCorrectFunction) {
                                errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find object for expression '" + callExpression.GetAccessExpressionQualifiedId() + "'. " + errorMessage, FileName = s.FileName, Entity = s });
                                if (!Config.ContinueOnError) {
                                    return false;
                                }
                            }
                        }

                        GrapeEntity entity = (new List<GrapeEntity>(entities))[0];
                        if (!(entity is GrapeMethod) && !foundCorrectFunction) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot call an object that is not a method.", FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        GrapeMethod methodWithSignature = entity as GrapeMethod;
                        if (methodWithSignature != null) {
                            GrapeModifier.GrapeModifierType modifiers = callExpression.GetLogicalParentOfEntityType<GrapeClass>().GetAppropriateModifiersForEntityAccess(Config, methodWithSignature);
                            if (!ValidateMethodSignatureAndOverloads(callExpression, methodWithSignature, modifiers, ref errorMessage) && !Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        string errorMessage;
                        IEnumerable<GrapeEntity> entities = typeCheckingUtils.GetEntitiesForAccessExpression(Config, s, obj as GrapeEntity, out errorMessage);
                        if (errorMessage != "") {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        GrapeAccessExpression lastExpressionWithNext = s;
                        GrapeAccessExpression lastExpression = s;
                        while (lastExpression != null && !(lastExpression is GrapeCallExpression)) {
                            if (lastExpression.Next != null) {
                                lastExpressionWithNext = lastExpression;
                            }

                            lastExpression = lastExpression.Member as GrapeAccessExpression;
                            if (lastExpression == null && lastExpressionWithNext != null) {
                                lastExpression = lastExpressionWithNext.Next;
                                if (lastExpression.Member != null && !(lastExpression.Member is GrapeAccessExpression)) {
                                    break;
                                }
                            }
                        }

                        if (!(lastExpression is GrapeCallExpression)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Only call, set and new object expressions can be used as a statement.", FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}