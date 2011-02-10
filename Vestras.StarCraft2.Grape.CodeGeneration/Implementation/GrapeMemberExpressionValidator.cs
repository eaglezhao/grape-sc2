using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator)), Export]
    internal class GrapeMemberExpressionValidator : IAstNodeValidator {
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
                    typeof(GrapeMemberExpression),
                    typeof(GrapeArrayAccessExpression),
                    typeof(GrapeCallExpression),
                    typeof(GrapeSetExpression),
                    typeof(GrapeObjectCreationExpression),
                };
            }
        }

        public bool ValidateFunctionSignatureAndOverloads(GrapeCallExpression callExpression, GrapeFunction function, string[] modifiers, ref string errorMessage) {
            errorMessage = "";
            List<GrapeFunction> functions = new List<GrapeFunction>();
            functions.AddRange(astUtils.GetFunctionsWithNameFromImportedPackagesInFile(Config, function.Name, function.FileName, function.GetLogicalParentOfEntityType<GrapeClass>()));
            List<GrapeFunction> functionsWithSignature = typeCheckingUtils.GetFunctionWithSignature(Config, functions, modifiers, function.Name, function.ReturnType, new List<GrapeExpression>(callExpression.Parameters), ref errorMessage);
            if (errorMessage != "") {
               errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = callExpression.FileName, Offset = callExpression.Offset, Length = callExpression.Length });
                if (!Config.ContinueOnError) {
                    return false;
                }
            }

            if (functionsWithSignature.Count > 1) {
                errorMessage = "Multiple functions with signature '" + typeCheckingUtils.GetFunctionSignatureString(Config, function.Name, function.ReturnType, new List<GrapeExpression>(callExpression.Parameters)) + "' found.";
                errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = callExpression.FileName, Offset = callExpression.Offset, Length = callExpression.Length });
                if (!Config.ContinueOnError) {
                    return false;
                }
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeMemberExpression s = obj as GrapeMemberExpression;
                if (s != null) {
                    GrapeArrayAccessExpression arrayAccessExpression = s as GrapeArrayAccessExpression;
                    GrapeCallExpression callExpression = s as GrapeCallExpression;
                    GrapeSetExpression setExpression = s as GrapeSetExpression;
                    GrapeObjectCreationExpression objectCreationExpression = s as GrapeObjectCreationExpression;
                    if (arrayAccessExpression != null) {
                        if (s.Parent is GrapeBlock) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "An array access expression cannot be the direct child of a block.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        string errorMessage = "";
                        if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, arrayAccessExpression.Array, "int_base", ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve array access expression to the type 'int_base'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (objectCreationExpression != null) {
                        if (!typeCheckingUtils.DoesTypeExist(Config, objectCreationExpression.Member, s.FileName)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, objectCreationExpression.Member) + "'.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        string errorMessage = "";
                        if (!typeCheckingUtils.DoesExpressionResolveToType(Config, objectCreationExpression, objectCreationExpression, objectCreationExpression.Member, ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, objectCreationExpression.Member) + "'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        GrapeEntity entity = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(Config, objectCreationExpression, objectCreationExpression, out errorMessage)))[0];
                        if (entity != null) {
                            GrapeFunction functionWithSignature = entity as GrapeFunction;
                            string[] modifiers = objectCreationExpression.GetLogicalParentOfEntityType<GrapeClass>().GetAppropriateModifiersForEntityAccess(Config, functionWithSignature);
                            if (functionWithSignature != null && !ValidateFunctionSignatureAndOverloads(objectCreationExpression, functionWithSignature, modifiers, ref errorMessage) && !Config.ContinueOnError) {
                                return false;
                            }
                        } else {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find constructor for type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, objectCreationExpression.Member) + "'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (callExpression != null) {
                        string errorMessage;
                        IList<GrapeEntity> entities = new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(Config, callExpression, callExpression, out errorMessage));
                        bool foundCorrectFunction = false;
                        if (entities.Count == 1 && entities[0] == null) {
                            string qualifiedId = callExpression.GetMemberExpressionQualifiedId();
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
                                errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find object for expression '" + callExpression.GetMemberExpressionQualifiedId() + "'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                                if (!Config.ContinueOnError) {
                                    return false;
                                }
                            }
                        }

                        GrapeEntity entity = (new List<GrapeEntity>(entities))[0];
                        if (!(entity is GrapeFunction) && !foundCorrectFunction) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot call an object that is not a function.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        GrapeFunction functionWithSignature = entity as GrapeFunction;
                        if (functionWithSignature != null) {
                            string[] modifiers = callExpression.GetLogicalParentOfEntityType<GrapeClass>().GetAppropriateModifiersForEntityAccess(Config, functionWithSignature);
                            if (!ValidateFunctionSignatureAndOverloads(callExpression, functionWithSignature, modifiers, ref errorMessage) && !Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (setExpression != null) {
                        string errorMessage;
                        GrapeEntity entityBeingSet = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(Config, setExpression as GrapeMemberExpression, setExpression, out errorMessage)))[0];
                        if (entityBeingSet == null) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find object for expression '" + setExpression.GetMemberExpressionQualifiedId() + "'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (!(entityBeingSet is GrapeVariable)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot set an object that is not a variable.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (setExpression.Member is GrapeArrayAccessExpression) {
                            if (!ValidateNode(setExpression.Member) && !Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (setExpression.Value is GrapeMemberExpression) {
                            IEnumerable<GrapeEntity> valueEntities = typeCheckingUtils.GetEntitiesForMemberExpression(Config, new GrapeMemberExpression { Member = setExpression.Value }, setExpression, out errorMessage);
                            if (valueEntities.Count() == 0) {
                                errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find object for expression '" + ((GrapeMemberExpression)setExpression.Value).GetMemberExpressionQualifiedId() + "'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                                if (!Config.ContinueOnError) {
                                    return false;
                                }
                            }
                        }

                        GrapeVariable variable = entityBeingSet as GrapeVariable;
                        if (!typeCheckingUtils.DoesExpressionResolveToType(Config, setExpression, setExpression.Value, variable.Type, ref errorMessage)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, variable.Type) + "'. " + errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        string errorMessage;
                        IEnumerable<GrapeEntity> entities = typeCheckingUtils.GetEntitiesForMemberExpression(Config, s, obj as GrapeEntity, out errorMessage);
                        if (errorMessage != "") {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
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
