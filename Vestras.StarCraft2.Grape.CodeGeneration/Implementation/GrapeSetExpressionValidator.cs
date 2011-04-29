using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeSetExpressionValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] {
                    typeof(GrapeSetExpression),
                };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeSetExpression s = obj as GrapeSetExpression;
                if (s != null) {
                    string errorMessage;
                    GrapeEntity entityBeingSet = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForAccessExpression(Config, s, s, out errorMessage)))[0];
                    if (entityBeingSet == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find object for expression '" + s.MemberAccess.GetAccessExpressionQualifiedId() + "'. " + errorMessage, FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (!(entityBeingSet is GrapeVariable)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot set an object that is not a variable.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (s.MemberAccess is GrapeArrayExpression) {
                        if (!ValidateNode(s.MemberAccess) && !Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (s.Value is GrapeAccessExpression) {
                        IEnumerable<GrapeEntity> valueEntities = typeCheckingUtils.GetEntitiesForAccessExpression(Config, s.Value as GrapeAccessExpression, s, out errorMessage);
                        if (valueEntities.Count() == 0) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find object for expression '" + ((GrapeAccessExpression)s.Value).GetAccessExpressionQualifiedId() + "'. " + errorMessage, FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    GrapeVariable variable = entityBeingSet as GrapeVariable;
                    if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.Value, variable.Type, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, variable.Type) + "'. " + errorMessage, FileName = s.FileName, Entity = s });
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
