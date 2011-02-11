using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeReturnStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeReturnStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeReturnStatement s = obj as GrapeReturnStatement;
                if (s != null) {
                    GrapeFunction logicalFunctionParent = s.GetLogicalParentOfEntityType<GrapeFunction>();
                    if (logicalFunctionParent == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A return statement must be the logical child of a function.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage = "";
                    GrapeMemberExpression returnTypeMemberExpression = logicalFunctionParent.ReturnType as GrapeMemberExpression;
                    string returnTypeQualifiedId = "";
                    if (returnTypeMemberExpression != null) {
                        returnTypeQualifiedId = returnTypeMemberExpression.GetMemberExpressionQualifiedId();
                    } else if (logicalFunctionParent.ReturnType is GrapeIdentifierExpression) {
                        returnTypeQualifiedId = ((GrapeIdentifierExpression)logicalFunctionParent.ReturnType).Identifier;
                    }

                    if (returnTypeQualifiedId == "void" || returnTypeQualifiedId == "void_base") {
                        if (s.ReturnValue != null) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Since " + logicalFunctionParent.Name + " returns void, a return keyword must not be followed by an expression.", FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (s.ReturnValue == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Since " + logicalFunctionParent.Name + " does not return void, a return keyword must be followed by an expression.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    } else if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.ReturnValue, returnTypeMemberExpression, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, logicalFunctionParent.ReturnType) + "'. " + errorMessage, FileName = s.FileName, Entity = s.ReturnValue });
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
