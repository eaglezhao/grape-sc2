using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

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

        private bool ValidateModifiers(GrapeClass c, out string errorMessage) {
            errorMessage = "";
            bool isStatic = c.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static);
            bool isAbstract = c.Modifiers.Contains(GrapeModifier.GrapeModifierType.Abstract);
            bool isSealed = c.Modifiers.Contains(GrapeModifier.GrapeModifierType.Sealed);
            bool isOverride = c.Modifiers.Contains(GrapeModifier.GrapeModifierType.Override);
            if (c.Modifiers.HasInvalidAccessModifiers()) {
                errorMessage = "Invalid access modifiers found.";
                return false;
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
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = c.FileName, Entity = c });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (c.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static) && c.Size != GrapeClass.DefaultSize) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A static class cannot have a size specified.", FileName = c.FileName, Entity = c });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (c.Size == 0) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The size of a class cannot be 0.", FileName = c.FileName, Entity = c });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (c.Inherits != null && !typeCheckingUtils.DoesTypeExist(Config, c.Inherits, c.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, c.Inherits) + "' could not be found.", FileName = c.FileName, Entity = c.Inherits });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string inheritsTypeQualifiedId = c.Inherits.ToString();
                    if (inheritsTypeQualifiedId == "void" || inheritsTypeQualifiedId == "void_base") {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The void type cannot be inherited.", FileName = c.FileName, Entity = c.Inherits });
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
