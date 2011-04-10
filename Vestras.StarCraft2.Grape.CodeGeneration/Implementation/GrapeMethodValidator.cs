using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal sealed class GrapeMethodValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeVariableValidator variableValidator = null;
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
                return new Type[] { typeof(GrapeMethod) };
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

        private bool ValidateModifiers(GrapeMethod m, out string errorMessage) {
            errorMessage = "";
            bool isStatic = m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static);
            bool isAbstract = m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Abstract);
            bool isSealed = m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Sealed);
            bool isOverride = m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Override);
            if (m.Modifiers.HasInvalidAccessModifiers()) {
                errorMessage = "Invalid access modifiers found.";
                return false;
            }

            if (isStatic) {
                if (isAbstract || isSealed || isOverride) {
                    errorMessage = "A function cannot be declared static and abstract, sealed or override at the same time.";
                    return false;
                }
            } else if (isAbstract && isSealed) {
                errorMessage = "A function cannot be declared abstract and sealed at the same time.";
                return false;
            } else if (isAbstract && isOverride) {
                errorMessage = "A function cannot be declared abstract and override at the same time.";
                return false;
            }

            return true;
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeMethod m = obj as GrapeMethod;
                if (m != null) {
                    if (m.Parent == null || m.Parent is GrapePackageDeclaration) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Top level method declarations are not allowed.", FileName = m.FileName, Entity = m });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (!m.IsLogicalChildOfEntityType<GrapeClass>()) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A method must be the child of a class.", FileName = m.FileName, Entity = m });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    if (m.Parent != null && m.Parent is GrapeClass && ((GrapeClass)m.Parent).Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                        if (!m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare instance members in a static class.", FileName = m.FileName, Entity = m });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    GrapeClass c = m.GetLogicalParentOfEntityType<GrapeClass>();
                    if (m.Type == GrapeMethod.GrapeMethodType.Constructor || m.Type == GrapeMethod.GrapeMethodType.Destructor) {
                        if (m.Name != c.Name) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A ctor/dctor must have the same name as the parent class.", FileName = m.FileName, Entity = m });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A ctor/dctor cannot be declared static.", FileName = m.FileName, Entity = m });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }

                        if (c.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A static class cannot have ctors/dctors.", FileName = m.FileName, Entity = m });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    } else {
                        if (m.Name == c.Name) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "A function cannot have the same name as the parent class.", FileName = m.FileName, Entity = m });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    if (m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Abstract) && (new List<GrapeEntity>(m.GetChildren())).Count > 0) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "An abstract function cannot have an implementation.", FileName = m.FileName, Entity = m });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string errorMessage;
                    if (m.Modifiers.Contains(GrapeModifier.GrapeModifierType.Override)) {
                        GrapeMethod baseMethod = m.GetOverridingMethod(Config, out errorMessage);
                        if (baseMethod == null) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = errorMessage, FileName = m.FileName, Entity = m });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    if (m.ReturnType != null && !typeCheckingUtils.DoesTypeExist(Config, m.ReturnType, m.FileName)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "The type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, m.ReturnType) + "' could not be found.", FileName = m.FileName, Entity = m.ReturnType });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    GrapeEntity type = (new List<GrapeEntity>(typeCheckingUtils.GetEntitiesForAccessExpression(Config, m.ReturnType, m, out errorMessage)))[0];
                    if (type is GrapeClass) {
                        GrapeClass typeClass = type as GrapeClass;
                        if (typeClass.Modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot declare a function of static type '" + typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, m.ReturnType) + "'.", FileName = m.FileName, Entity = m.ReturnType });
                            if (!Config.ContinueOnError) {
                                return false;
                            }
                        }
                    }

                    string modifiersErrorMessage;
                    if (!ValidateModifiers(m, out modifiersErrorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = modifiersErrorMessage, FileName = m.FileName, Entity = m });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    foreach (GrapeVariable param in m.Parameters) {
                        bool isParamValid = variableValidator.ValidateNode(param);
                        if (!isParamValid) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}