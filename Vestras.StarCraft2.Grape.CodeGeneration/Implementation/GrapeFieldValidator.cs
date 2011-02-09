using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

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
            bool hasAccessModifier = false;
            bool isAbstract = false;
            bool isSealed = false;
            bool isOverride = false;
            foreach (string modifier in f.Modifiers.Split(' ')) {
                if (IsModifierAccessModifier(modifier)) {
                    if (hasAccessModifier) {
                        errorMessage = "A field cannot have multiple access modifiers.";
                        return false;
                    }

                    hasAccessModifier = true;
                } else {
                    if (modifier == "abstract") {
                        isAbstract = true;
                    } else if (modifier == "sealed") {
                        isSealed = true;
                    } else if (modifier == "override") {
                        isOverride = true;
                    }
                }
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
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Top level field declarations are not allowed.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (!f.IsLogicalChildOfEntityType<GrapeClass>()) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A field must be the child of a class.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (f.Parent != null && f.Parent is GrapeClass && ((GrapeClass)f.Parent).Modifiers.Contains("static")) {
                        if (!f.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare instance members in a static class.", FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    if (f.Type != null && !typeCheckingUtils.DoesTypeExist(Config, f.Type, f.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.Type) + "' could not be found.", FileName = f.FileName, Offset = f.Type.Offset, Length = f.Type.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage;
                    GrapeEntity type = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForMemberExpression(Config, f.Type as GrapeMemberExpression, f, out errorMessage)))[0];
                    if (type is GrapeClass) {
                        GrapeClass typeClass = type as GrapeClass;
                        if (typeClass.Modifiers.Contains("static")) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare a field of static type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.Type) + "'.", FileName = f.FileName, Offset = f.Type.Offset, Length = f.Type.Length });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    string modifiersErrorMessage;
                    if (!ValidateModifiers(f, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = f.FileName, Offset = f.Offset, Length = f.Length });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (f.Value != null && !typeCheckingUtils.DoesExpressionResolveToType(Config, f, f.Value, f.Type, ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, f.Type) + "'. " + errorMessage, FileName = f.FileName, Offset = f.Value.Offset, Length = f.Value.Length });
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
