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
                    GrapeMethod logicalMethodParent = s.GetLogicalParentOfEntityType<GrapeMethod>();
                    if (logicalMethodParent == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A return statement must be the logical child of a method.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage = "";
                    string returnTypeQualifiedId = logicalMethodParent.ReturnType.ToString();
                    if (returnTypeQualifiedId == "void" || returnTypeQualifiedId == "void_base") {
                        if (s.ReturnValue != null) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Since " + logicalMethodParent.Name + " returns void, a return keyword must not be followed by an expression.", FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (s.ReturnValue == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Since " + logicalMethodParent.Name + " does not return void, a return keyword must be followed by an expression.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    } else if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.ReturnValue, logicalMethodParent.ReturnType, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, logicalMethodParent.ReturnType) + "'. " + errorMessage, FileName = s.FileName, Entity = s.ReturnValue });
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
