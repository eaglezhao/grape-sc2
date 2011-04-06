using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeFieldValidator : IAstNodeValidator {
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
                return new Type[] { typeof(GrapeField) };
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

        private bool ValidateModifiers(GrapeField f, out string errorMessage) {
            errorMessage = "";
            bool isAbstract = f.Modifiers.Contains(GrapeModifier.GrapeModifierType.Abstract);
            bool isSealed = f.Modifiers.Contains(GrapeModifier.GrapeModifierType.Sealed);
            bool isOverride = f.Modifiers.Contains(GrapeModifier.GrapeModifierType.Override);
            if (f.Modifiers.HasInvalidAccessModifiers()) {
                errorMessage = "Invalid access modifiers found.";
                return false;
            }

            if (isAbstract) {
                errorMessage = "A field cannot be declared abstract.";
                return false;
            }

            if (isSealed) {
                errorMessage = "A field cannot be declared sealed.";
                return false;
            }

            if (isOverride) {
                errorMessage = "A field cannot be declared override.";
                return false;
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeField f = obj as GrapeField;
                if (f != null) {
                    if (f.Parent == null || f.Parent is GrapePackageDeclaration) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Top level field declarations are not allowed.", FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (!f.IsLogicalChildOfEntityType<GrapeClass>()) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A field must be the child of a class.", FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (f.Parent != null && f.Parent is GrapeClass && ((GrapeClass)f.Parent).Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                        if (!f.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare instance members in a static class.", FileName = f.FileName, Entity = f });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    if (f.Field.Type != null && !typeCheckingUtils.DoesTypeExist(Config, f.Field.Type, f.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.Field.Type) + "' could not be found.", FileName = f.FileName, Entity = f.Field.Type });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage;
                    GrapeEntity type = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForAccessExpression(Config, f.Field.Type, f, out errorMessage)))[0];
                    if (type is GrapeClass) {
                        GrapeClass typeClass = type as GrapeClass;
                        if (typeClass.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare a field of static type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.Field.Type) + "'.", FileName = f.FileName, Entity = f.Field.Type });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    string modifiersErrorMessage;
                    if (!ValidateModifiers(f, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = f.FileName, Entity = f });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (f.Field.Initializer != null && f.Field.Initializer is GrapeValueInitializer && !typeCheckingUtils.DoesExpressionResolveToType(Config, f, ((GrapeValueInitializer)f.Field.Initializer).Value, f.Field.Type, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.Field.Type) + "'. " + errorMessage, FileName = f.FileName, Entity = f });
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
