using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

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
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot access base constructor when the current class has no base class.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        GrapeEntity inheritingEntity = astUtils.GetClassWithNameFromImportedPackagesInFile(Config.Ast, typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, c.Inherits), c.FileName);
                        if (inheritingEntity != null && inheritingEntity is GrapeClass) {
                            string errorMessage = "";
                            GrapeClass inheritingClass = inheritingEntity as GrapeClass;
                            GrapeFunction function = null;
                            foreach (GrapeEntity entity in inheritingClass.Block.Children) {
                                if (entity != null && entity is GrapeFunction && ((GrapeFunction)entity).Name == inheritingClass.Name && ((GrapeFunction)entity).Type == GrapeFunction.GrapeFunctionType.Constructor) {
                                    function = entity as GrapeFunction;
                                    break;
                                }
                            }

                            if (function == null) {
                                function = new GrapeFunction { Block = new GrapeBlock(), FileName = inheritingClass.FileName, Modifiers = "public", Name = inheritingClass.Name, ReturnType = new GrapeIdentifierExpression { Identifier = inheritingClass.Name }, Type = GrapeFunction.GrapeFunctionType.Constructor, Parent = inheritingClass };
                            }

                            string[] modifiers = c.GetAppropriateModifiersForEntityAccess(Config, function);
                            bool valid = memberExpressionValidator.ValidateFunctionSignatureAndOverloads(new GrapeCallExpression { FileName = s.FileName, Length = s.Length, Member = new GrapeIdentifierExpression { Identifier = inheritingClass.Name }, Offset = s.Offset, Parameters = new ObservableCollection<GrapeExpression>(s.Parameters), Parent = s }, function, modifiers, ref errorMessage);
                            if (!valid) {
                                errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                                if (!Config.ContinueOnError) {
                                    return false;
                                }
                            }
                        } else {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find base class '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, c.Inherits) + "'.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else if (s.Type == GrapeInitStatement.GrapeInitStatementType.This) {
                        GrapeClass c = s.GetLogicalParentOfEntityType<GrapeClass>();
                        GrapeFunction function = null;
                        foreach (GrapeEntity entity in c.Block.Children) {
                            if (entity != null && entity is GrapeFunction && ((GrapeFunction)entity).Name == c.Name && ((GrapeFunction)entity).Type == GrapeFunction.GrapeFunctionType.Constructor) {
                                function = entity as GrapeFunction;
                                break;
                            }
                        }

                        if (function == null) {
                            function = new GrapeFunction { Block = new GrapeBlock(), FileName = c.FileName, Modifiers = "public", Name = c.Name, ReturnType = new GrapeIdentifierExpression { Identifier = c.Name }, Type = GrapeFunction.GrapeFunctionType.Constructor, Parent = c };
                        }

                        string errorMessage = "";
                        string[] modifiers = c.GetAppropriateModifiersForEntityAccess(Config, function);
                        bool valid = memberExpressionValidator.ValidateFunctionSignatureAndOverloads(new GrapeCallExpression { FileName = s.FileName, Length = s.Length, Member = new GrapeIdentifierExpression { Identifier = c.Name }, Offset = s.Offset, Parameters = new ObservableCollection<GrapeExpression>(s.Parameters), Parent = s }, function, modifiers, ref errorMessage);
                        if (!valid) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = s.FileName, Offset = s.Offset, Length = s.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot find initialization type.", FileName = s.FileName, Offset = s.Offset, Length = s.Length });
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
