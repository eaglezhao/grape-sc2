using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeInitStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeMemberExpressionValidator memberExpressionValidator = null;
        [Import]
        private GrapeAstUtilities astUtils = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeInitStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeInitStatement s = obj as GrapeInitStatement;
                if (s != null) {
                    if (s.Type == GrapeInitStatement.GrapeInitStatementType.Base) {
                        GrapeClass c = s.GetLogicalParentOfEntityType<GrapeClass>();
                        if (c.Inherits == null) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot access base constructor when the current class has no base class.", FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        GrapeEntity inheritingEntity = astUtils.GetClassWithNameFromImportedPackagesInFile(Config.Ast, typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, c.Inherits), c.FileName);
                        if (inheritingEntity != null && inheritingEntity is GrapeClass) {
                            string errorMessage = "";
                            GrapeClass inheritingClass = inheritingEntity as GrapeClass;
                            GrapeMethod method = null;
                            foreach (GrapeEntity entity in inheritingClass.GetChildren()) {
                                if (entity != null && entity is GrapeMethod && ((GrapeMethod)entity).Name == inheritingClass.Name && ((GrapeMethod)entity).Type == GrapeMethod.GrapeMethodType.Constructor) {
                                    method = entity as GrapeMethod;
                                    break;
                                }
                            }

                            if (method == null) {
                                GrapeList<GrapeModifier> methodModifiers = new GrapeList<GrapeModifier>(new GrapePublicModifier());
                                GrapeList<GrapeIdentifier> nameParts = new GrapeList<GrapeIdentifier>(new GrapeIdentifier(inheritingClass.Name));
                                method = new GrapeMethod(methodModifiers, new GrapeSimpleType(nameParts), new GrapeIdentifier(inheritingClass.Name), new GrapeList<GrapeParameter>(), new GrapeList<GrapeStatement>());
                                method.Parent = inheritingClass;
                            }

                            GrapeModifier.GrapeModifierType modifiers = c.GetAppropriateModifiersForEntityAccess(Config, method);
                            bool valid = memberExpressionValidator.ValidateFunctionSignatureAndOverloads(new GrapeCallExpression { Length = s.Length, Member = new GrapeIdentifierExpression { Identifier = inheritingClass.Name }, Offset = s.Offset, Parameters = new ObservableCollection<GrapeExpression>(s.Parameters), Parent = s }, method, modifiers, ref errorMessage);
                            if (!valid) {
                                errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = s.FileName, Entity = s });
                                if (!Config.ContinueOnError) {
                                    return false;
                                }
                            }
                        } else {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find base class '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, c.Inherits) + "'.", FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (s.Type == GrapeInitStatement.GrapeInitStatementType.This) {
                        GrapeClass c = s.GetLogicalParentOfEntityType<GrapeClass>();
                        GrapeMethod method = null;
                        foreach (GrapeEntity entity in c.GetChildren()) {
                            if (entity != null && entity is GrapeMethod && ((GrapeMethod)entity).Name == c.Name && ((GrapeMethod)entity).Type == GrapeMethod.GrapeMethodType.Constructor) {
                                method = entity as GrapeMethod;
                                break;
                            }
                        }

                        if (method == null) {
                            GrapeList<GrapeModifier> methodModifiers = new GrapeList<GrapeModifier>(new GrapePublicModifier());
                            GrapeList<GrapeIdentifier> nameParts = new GrapeList<GrapeIdentifier>(new GrapeIdentifier(c.Name));
                            method = new GrapeMethod(methodModifiers, new GrapeSimpleType(nameParts), new GrapeIdentifier(c.Name), new GrapeList<GrapeParameter>(), new GrapeList<GrapeStatement>());
                            method.Parent = c;
                        }

                        string errorMessage = "";
                        GrapeModifier.GrapeModifierType modifiers = c.GetAppropriateModifiersForEntityAccess(Config, method);
                        bool valid = memberExpressionValidator.ValidateFunctionSignatureAndOverloads(new GrapeCallExpression { FileName = s.FileName, Length = s.Length, Member = new GrapeIdentifierExpression { Identifier = c.Name }, Offset = s.Offset, Parameters = new ObservableCollection<GrapeExpression>(s.Parameters), Parent = s }, method, modifiers, ref errorMessage);
                        if (!valid) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = s.FileName, Entity = s });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find initialization type.", FileName = s.FileName, Entity = s });
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
