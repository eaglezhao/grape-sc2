using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeClassValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        private static readonly string[] AccessModifiers = new string[] {
            "public",
            "private",
            "protected",
            "internal"
        };

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeClass) };
            }
        }

        private bool IsModifierAccessModifier(string modifier) {
            foreach (string accessModifier in AccessModifiers) {
                if (modifier == accessModifier) {
                    return true;
                }
            }

            return false;
        }

        private bool ValidateModifiers(GrapeClass f, out string errorMessage) {
            errorMessage = "";
            bool hasAccessModifier = false;
            bool isStatic = false;
            bool isAbstract = false;
            bool isSealed = false;
            bool isOverride = false;
            foreach (string modifier in f.Modifiers.Split(' ')) {
                if (IsModifierAccessModifier(modifier)) {
                    if (hasAccessModifier) {
                        errorMessage = "A class cannot have multiple access modifiers.";
                        return false;
                    }

                    hasAccessModifier = true;
                } else {
                    if (modifier == "static") {
                        isStatic = true;
                    } else if (modifier == "abstract") {
                        isAbstract = true;
                    } else if (modifier == "sealed") {
                        isSealed = true;
                    } else if (modifier == "override") {
                        isOverride = true;
                    }
                }
            }

            if (isStatic && isAbstract) {
                errorMessage = "A class cannot be declared static and abstract at the same time.";
                return false;
            }

            if (isSealed) {
                errorMessage = "A class cannot be declared sealed.";
                return false;
            }

            if (isOverride) {
                errorMessage = "A class cannot be declared override.";
                return false;
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeClass c = obj as GrapeClass;
                if (c != null) {
                    string modifiersErrorMessage;
                    if (!ValidateModifiers(c, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = c.FileName, Offset = c.Offset, Length = c.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (c.Modifiers.Contains("static") && c.Size != GrapeClass.UseDefaultSize) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A static class cannot have a size specified.", FileName = c.FileName, Offset = c.Offset, Length = c.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (c.Size == 0) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The size of a class cannot be 0.", FileName = c.FileName, Offset = c.Offset, Length = c.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (c.Inherits != null && !typeCheckingUtils.DoesTypeExist(Config, c.Inherits, c.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, c.Inherits) + "' could not be found.", FileName = c.FileName, Offset = c.Inherits.Offset, Length = c.Inherits.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeMemberExpression inheritsTypeMemberExpression = c.Inherits as GrapeMemberExpression;
                    string inheritsTypeQualifiedId = "";
                    if (inheritsTypeMemberExpression != null) {
                        inheritsTypeQualifiedId = inheritsTypeMemberExpression.GetMemberExpressionQualifiedId();
                    } else if (c.Inherits is GrapeIdentifierExpression) {
                        inheritsTypeQualifiedId = ((GrapeIdentifierExpression)c.Inherits).Identifier;
                    }

                    if (inheritsTypeQualifiedId == "void" || inheritsTypeQualifiedId == "void_base") {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The void type cannot be inherited.", FileName = c.FileName, Offset = inheritsTypeMemberExpression.Offset, Length = inheritsTypeMemberExpression.Length });
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
