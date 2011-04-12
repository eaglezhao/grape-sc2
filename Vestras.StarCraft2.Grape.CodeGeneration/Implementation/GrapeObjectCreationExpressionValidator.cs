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
    internal class GrapeObjectCreationExpressionValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeAstUtilities astUtils = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;
        [Import]
        private GrapeAccessExpressionValidator accessExpressionValidator = null;

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
                GrapeObjectCreationExpression s = obj as GrapeObjectCreationExpression;
                if (s != null) {
                    if (!typeCheckingUtils.DoesTypeExist(Config, s.ClassName, s.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, s.ClassName) + "'.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage = "";
                    if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s, s.ClassName, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, s.ClassName) + "'. " + errorMessage, FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeEntity entity = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForObjectCreationExpression(Config, s, s, out errorMessage)))[0];
                    if (entity != null) {
                        GrapeMethod methodWithSignature = entity as GrapeMethod;
                        GrapeModifier.GrapeModifierType modifiers = s.GetLogicalParentOfEntityType<GrapeClass>().GetAppropriateModifiersForEntityAccess(Config, methodWithSignature);
                        if (methodWithSignature != null && !accessExpressionValidator.ValidateMethodSignatureAndOverloads(GrapeCallExpression.Create(new GrapeIdentifier(s.ClassName.ToString()), GrapeList<GrapeExpression>.FromEnumerable(s.Parameters), null), methodWithSignature, modifiers, ref errorMessage) && !Config.ContinueOnError) {
                            return false;
                        }
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find constructor for type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, s.ClassName) + "'. " + errorMessage, FileName = s.FileName, Entity = s });
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
