using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export]
    internal class GrapeTypeCheckingUtilities {
        private static Dictionary<GrapeLiteralExpression.GrapeLiteralExpressionType, string> literalTypes = new Dictionary<GrapeLiteralExpression.GrapeLiteralExpressionType, string>();

        [Import]
        private GrapeAstUtilities astUtils = null;
        [Import]
        private GrapeAccessExpressionValidator accessExpressionValidator = null;
        internal static GrapeTypeCheckingUtilities Instance;

        public bool IsTypeInClassInheritanceTree(GrapeCodeGeneratorConfiguration config, string typeName, GrapeClass c) {
            typeName = GetCorrectNativeTypeName(typeName);
            if (typeName == c.Name) {
                return true;
            }

            if (c == null) {
                return false;
            }

            if (c.Inherits != null) {
                string inheritsTypeName = c.Inherits.ToString();
                if (inheritsTypeName == typeName) {
                    return true;
                }

                return IsTypeInClassInheritanceTree(config, typeName, astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, inheritsTypeName, c.FileName));
            }

            return false;
        }

        public bool IsSignatureValid(GrapeCodeGeneratorConfiguration config, GalaxyFunctionAttribute function, GrapeCallExpression callExpression, out string errorMessage) {
            errorMessage = "";
            string[] parameters = function.Params.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parameters.Length != callExpression.Parameters.Count) {
                errorMessage = "Cannot find function with argument count '" + callExpression.Parameters.Count + "'.";
                return false;
            }

            int index = 0;
            foreach (string parameter in parameters) {
                string[] splitParam = parameter.Trim().Split(' ');
                if (splitParam.Length != 2) {
                    errorMessage = "The native interface function has malformed parameters.";
                    return false;
                }

                string type = GetCorrectNativeTypeName(splitParam[0]);
                string name = splitParam[1];
                GrapeExpression expression = callExpression.Parameters[index];
                if (!DoesExpressionResolveToType(config, expression, expression, type, ref errorMessage)) {
                    errorMessage = "Cannot resolve parameter expression to type '" + type + "'. " + errorMessage;
                    return false;
                }

                index++;
            }

            return true;
        }

        public string GetTypeNameForTypeAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeType type) { // left for compat
            string dummyMessage = "";
            return GetTypeNameForTypeAccessExpression(config, type, ref dummyMessage);
        }

        public string GetTypeNameForTypeAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeType type, ref string errorMessage) { // left for compat
            return type.ToString();
        }

        public string GetTypeNameForTypeAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeExpression expression) {
            string dummyMessage = "";
            return GetTypeNameForTypeAccessExpression(config, expression, ref dummyMessage);
        }

        public string GetTypeNameForTypeAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeExpression expression, ref string errorMessage) {
            if (expression is GrapeMemberExpression) {
                GrapeEntity entity = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeMemberExpression)expression), expression, out errorMessage)))[0];
                if (entity is GrapeVariable) {
                    return GetCorrectNativeTypeName(((GrapeVariable)entity).Type.ToString());
                } else if (entity is GrapeMethod) {
                    return GetCorrectNativeTypeName(((GrapeMethod)entity).ReturnType.ToString());
                } else if (entity is GrapeClass) {
                    return GetCorrectNativeTypeName((entity as GrapeClass).Name);
                } else if (entity == null) {
                    string qualifiedId = ((GrapeMemberExpression)expression).GetAccessExpressionQualifiedId();
                    foreach (GalaxyFunctionAttribute function in GalaxyNativeInterfaceAggregator.Functions) {
                        if (function.Name == qualifiedId) {
                            errorMessage = "";
                            return GetCorrectNativeTypeName(function.Type);
                        }
                    }

                    foreach (GalaxyConstantAttribute constant in GalaxyNativeInterfaceAggregator.Constants) {
                        if (constant.Name == qualifiedId) {
                            errorMessage = "";
                            return GetCorrectNativeTypeName(constant.Type);
                        }
                    }

                    foreach (Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute> type in GalaxyNativeInterfaceAggregator.Types) {
                        if (type.Item1.NativeAlias == qualifiedId || type.Item1.TypeName == qualifiedId) {
                            errorMessage = "";
                            return GetCorrectNativeTypeName(type.Item1.NativeAlias);
                        }
                    }
                }

                return "";// GetCorrectNativeTypeName(((GrapeMemberExpression)expression).GetMemberExpressionQualifiedId());
            } else if (expression is GrapeLiteralExpression) {
                return literalTypes[((GrapeLiteralExpression)expression).Type];
            } else if (expression is GrapeAddExpression) {
                GrapeAddExpression addExpression = expression as GrapeAddExpression;
                string leftType = GetTypeNameForTypeAccessExpression(config, addExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, addExpression.Right, ref errorMessage);
                if (leftType != rightType) {
                    errorMessage = "The left and right expressions do not resolve to the same type.";
                }

                return leftType;
            } else if (expression is GrapeMultiplicationExpression) {
                GrapeMultiplicationExpression multiplicationExpression = expression as GrapeMultiplicationExpression;
                string leftType = GetTypeNameForTypeAccessExpression(config, multiplicationExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, multiplicationExpression.Right, ref errorMessage);
                if (leftType != rightType) {
                    errorMessage = "The left and right expressions do not resolve to the same type.";
                }

                return leftType;
            } else if (expression is GrapeShiftExpression) {
                GrapeShiftExpression shiftExpression = expression as GrapeShiftExpression;
                string leftType = GetTypeNameForTypeAccessExpression(config, shiftExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, shiftExpression.Right, ref errorMessage);
                if (leftType != rightType) {
                    errorMessage = "The left and right expressions do not resolve to the same type.";
                }

                return leftType;
            } else if (expression is GrapeUnaryExpression) {
                GrapeUnaryExpression unaryExpression = expression as GrapeUnaryExpression;
                return GetTypeNameForTypeAccessExpression(config, unaryExpression.Value, ref errorMessage);
            }

            return "";
        }

        public IEnumerable<GrapeEntity> GetMembersForExpression(GrapeCodeGeneratorConfiguration config, string variableName, GrapeEntity parent) {
            if (parent is GrapeExpression || parent is GrapeStatement) {
                GrapeEntity oldParent = parent;
                parent = parent.GetLogicalParentOfEntityType<GrapeMethod>();
                if (parent == null) {
                    parent = oldParent.GetLogicalParentOfEntityType<GrapeClass>();
                }
            }

            IEnumerable<GrapeVariable> variables = astUtils.GetVariablesWithNameFromImportedPackagesInFile(config, variableName, parent.FileName, parent);
            if (variables.Count() == 0) {
                variables = astUtils.GetVariablesWithNameFromImportedPackagesInFile(config, variableName, parent.FileName, parent.GetLogicalParentOfEntityType<GrapeClass>());
            }

            IEnumerable<GrapeMethod> methods = astUtils.GetMethodsWithNameFromImportedPackagesInFile(config, variableName, parent.FileName, parent.GetLogicalParentOfEntityType<GrapeClass>());
            List<GrapeEntity> list = new List<GrapeEntity>();
            list.AddRange(variables);
            list.AddRange(methods);
            return list;
        }

        public IEnumerable<GrapeEntity> GetClassesForExpression(GrapeCodeGeneratorConfiguration config, string className, GrapeEntity parent) {
            List<GrapeEntity> list = new List<GrapeEntity>();
            if (className == "this") {
                GrapeClass c = parent.GetLogicalParentOfEntityType<GrapeClass>();
                list.Add(c);
            } else if (className == "base") {
                GrapeClass c = parent.GetLogicalParentOfEntityType<GrapeClass>();
                if (c.Inherits == null) {
                    list.Add(null);
                } else {
                    GrapeClass inheritingClass = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, c.Inherits), parent.FileName);
                    list.Add(inheritingClass);
                }
            } else {
                list.Add(astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetCorrectNativeTypeName(className), parent.FileName));
            }

            return list;
        }

        public string GetMethodSignatureString(GrapeCodeGeneratorConfiguration config, string name, GrapeType returnType, List<GrapeExpression> parameters) {
            string result = GetTypeNameForTypeAccessExpression(config, returnType) + " " + name + "(";
            int index = 0;
            foreach (GrapeExpression parameter in parameters) {
                result += GetTypeNameForTypeAccessExpression(config, parameter);
                if (index < parameters.Count - 1) {
                    result += ", ";
                }

                index++;
            }

            result += ")";
            return result;
        }

        public string GetMethodSignatureString(GrapeCodeGeneratorConfiguration config, string name, GrapeType returnType, List<GrapeVariable> parameters) {
            string result = GetTypeNameForTypeAccessExpression(config, returnType) + " " + name + "(";
            int index = 0;
            foreach (GrapeVariable parameter in parameters) {
                result += GetTypeNameForTypeAccessExpression(config, parameter.Type);
                if (index < parameters.Count - 1) {
                    result += ", ";
                }

                index++;
            }

            result += ")";
            return result;
        }

        public List<GrapeMethod> GetMethodsWithSignature(GrapeCodeGeneratorConfiguration config, List<GrapeMethod> methods, GrapeModifier.GrapeModifierType modifiers, string name, GrapeType returnType, List<GrapeExpression> parameters, ref string errorMessage) {
            List<GrapeMethod> foundMethods = new List<GrapeMethod>();
            string parameterErrorMessage = "";
            foreach (GrapeMethod method in methods) {
                if (method.Name == name && GetTypeNameForTypeAccessExpression(config, method.ReturnType) == GetTypeNameForTypeAccessExpression(config, returnType) && method.Parameters.Count == parameters.Count) {
                    if (modifiers != 0) {
                        bool shouldContinue = false;
                        if (modifiers != method.Modifiers) {
                            shouldContinue = true;
                        }

                        if (shouldContinue) {
                            continue;
                        }
                    }

                    int currentParameterIndex = 0;
                    bool validParameters = true;
                    foreach (GrapeVariable param in method.Parameters) {
                        GrapeExpression parameterExpression = parameters[currentParameterIndex];
                        if (!DoesExpressionResolveToType(config, parameterExpression, parameterExpression, param.Type, ref parameterErrorMessage)) {
                            validParameters = false;
                            if (parameterErrorMessage == "") {
                                parameterErrorMessage = "Cannot resolve parameter expression to type '" + GetTypeNameForTypeAccessExpression(config, param.Type) + "'.";
                            }

                            break;
                        }

                        currentParameterIndex++;
                    }

                    if (validParameters) {
                        foundMethods.Add(method);
                    }
                }
            }

            if (foundMethods.Count == 0) {
                errorMessage = "Cannot find function with signature '" + GetMethodSignatureString(config, name, returnType, parameters) + "'. " + parameterErrorMessage;
            }

            return foundMethods;
        }

        public List<GrapeMethod> GetMethodsWithSignature(GrapeCodeGeneratorConfiguration config, List<GrapeMethod> methods, GrapeModifier.GrapeModifierType modifiers, string name, GrapeType returnType, List<GrapeVariable> parameters, ref string errorMessage) {
            List<GrapeMethod> foundMethods = new List<GrapeMethod>();
            string parameterErrorMessage = "";
            foreach (GrapeMethod method in methods) {
                if (method.Name == name && GetTypeNameForTypeAccessExpression(config, method.ReturnType) == GetTypeNameForTypeAccessExpression(config, returnType) && method.Parameters.Count == parameters.Count) {
                    if (modifiers != 0) {
                        bool shouldContinue = false;
                        if (modifiers != method.Modifiers) {
                            shouldContinue = true;
                        }

                        if (shouldContinue) {
                            continue;
                        }
                    }

                    int currentParameterIndex = 0;
                    bool validParameters = true;
                    foreach (GrapeVariable param in method.Parameters) {
                        GrapeVariable currentParameter = parameters[currentParameterIndex];
                        if (!DoesExpressionResolveToType(config, currentParameter, currentParameter.Type, param.Type, ref parameterErrorMessage)) {
                            validParameters = false;
                            if (parameterErrorMessage == "") {
                                parameterErrorMessage = "Cannot resolve parameter type '" + GetTypeNameForTypeAccessExpression(config, currentParameter.Type) + "' to type '" + GetTypeNameForTypeAccessExpression(config, param.Type) + "'.";
                            }

                            break;
                        }

                        currentParameterIndex++;
                    }

                    if (validParameters) {
                        foundMethods.Add(method);
                    }
                }
            }

            if (foundMethods.Count == 0) {
                errorMessage = "Cannot find function with signature '" + GetMethodSignatureString(config, name, returnType, parameters) + "'. " + parameterErrorMessage;
            }

            return foundMethods;
        }

        public IEnumerable<GrapeEntity> GetEntitiesForAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeType type, GrapeEntity parent, out string errorMessage) { // left for compat
            return GetEntitiesForAccessExpression(config, type.ToExpression() as GrapeAccessExpression, parent, out errorMessage);
        }

        public IEnumerable<GrapeEntity> GetEntitiesForAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeType type, GrapeEntity parent, out string errorMessage, bool detectMethodsUsedAsTypesOrVars) { // left for compat
            return GetEntitiesForAccessExpression(config, type.ToExpression() as GrapeAccessExpression, parent, out errorMessage, detectMethodsUsedAsTypesOrVars);
        }

        public IEnumerable<GrapeEntity> GetEntitiesForAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeAccessExpression accessExpression, GrapeEntity parent, out string errorMessage) {
            return GetEntitiesForAccessExpression(config, accessExpression, parent, out errorMessage, true);
        }

        public IEnumerable<GrapeEntity> GetEntitiesForAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeAccessExpression accessExpression, GrapeEntity parent, out string errorMessage, bool detectMethodsUsedAsTypesOrVars) {
            errorMessage = "";
            string memberExpressionQualifiedId = accessExpression.GetAccessExpressionQualifiedId();
            if (memberExpressionQualifiedId == "this") {
                if ((accessExpression.GetType() != typeof(GrapeMemberExpression) && accessExpression.GetType() != typeof(GrapeSetExpression)) || (parent is GrapeExpression && parent.GetType() != typeof(GrapeMemberExpression) && parent.GetType() != typeof(GrapeSetExpression))) {
                    errorMessage = "Cannot access this class through a statement expression.";
                    return new GrapeEntity[] { null };
                }

                GrapeClass thisClass = parent.GetLogicalParentOfEntityType<GrapeClass>();
                if (thisClass != null) {
                    return new GrapeEntity[] { thisClass };
                }

                return new GrapeEntity[] { null };
            } else if (memberExpressionQualifiedId == "base") {
                if ((accessExpression.GetType() != typeof(GrapeMemberExpression) && accessExpression.GetType() != typeof(GrapeSetExpression)) || (parent is GrapeExpression && parent.GetType() != typeof(GrapeMemberExpression) && parent.GetType() != typeof(GrapeSetExpression))) {
                    errorMessage = "Cannot access base class through a statement expression.";
                    return new GrapeEntity[] { null };
                }

                GrapeClass thisClass = parent.GetLogicalParentOfEntityType<GrapeClass>();
                if (thisClass != null && thisClass.Inherits != null) {
                    GrapeClass baseClass = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, thisClass.Inherits, ref errorMessage), parent.FileName);
                    return new GrapeEntity[] { baseClass };
                }

                return new GrapeEntity[] { null };
            } else if (accessExpression is GrapeObjectCreationExpression) {
                GrapeObjectCreationExpression objectCreationExpression = accessExpression as GrapeObjectCreationExpression;
                GrapeClass c = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, objectCreationExpression.Member), accessExpression.FileName);
                if (c != null) {
                    List<GrapeMethod> constructors = c.GetConstructors();
                    if (constructors.Count == 0) {
                        GrapeMethod method = new GrapeConstructor(new GrapeList<GrapeModifier>(new GrapePublicModifier()), new GrapeIdentifier(c.Name), new GrapeList<GrapeParameter>(), new GrapeList<GrapeStatement>());
                        method.Parent = c;
                        return new GrapeEntity[] { method };
                    } else {
                        errorMessage = "";
                        GrapeModifier.GrapeModifierType modifiers = objectCreationExpression.GetLogicalParentOfEntityType<GrapeClass>().GetAppropriateModifiersForEntityAccess(config, constructors[0]);
                        List<GrapeMethod> functionsWithSignature = GetMethodsWithSignature(config, constructors, modifiers, c.Name, new GrapeSimpleType(new GrapeList<GrapeIdentifier>(new GrapeIdentifier(c.Name))), new List<GrapeExpression>(objectCreationExpression.Parameters), ref errorMessage);
                        if (functionsWithSignature.Count > 1) {
                            errorMessage = "Multiple methods with signature '" + GetMethodSignatureString(config, c.Name, new GrapeSimpleType(new GrapeList<GrapeIdentifier>(new GrapeIdentifier(c.Name))), new List<GrapeExpression>(objectCreationExpression.Parameters)) + "' found.";
                            return new GrapeEntity[] { null };
                        }

                        if (functionsWithSignature.Count == 0) {
                            return new GrapeEntity[] { null };
                        }

                        return functionsWithSignature;
                    }
                }

                return new GrapeEntity[] { null };
            } else if (accessExpression is GrapeCallExpression || accessExpression is GrapeSetExpression) {
                GrapeAccessExpression identifierExpression = accessExpression.GetAccessExpressionInAccessExpression();
                GrapeEntity entity = null;
                GrapeEntity lastParent = null;
                GrapeEntity lastLastParent = null;
                GrapeEntity lastMemberAccess = null;
                GrapeAccessExpression currentExpression = identifierExpression;
                bool lastClassAccessWasThisOrBaseAccess = false;
                bool shouldBeStatic = false;
                bool staticnessMatters = false;
                while (currentExpression != null) {
                    GrapeAccessExpression expressionWithNext = currentExpression;
                    GrapeAccessExpression lastExpressionWithoutNext = currentExpression;
                    while (true) {
                        if (expressionWithNext.Next != null) {
                            break;
                        }

                        if (expressionWithNext.Member == null || !(expressionWithNext.Member is GrapeAccessExpression)) {
                            break;
                        }

                        expressionWithNext = expressionWithNext.Member as GrapeAccessExpression;
                    }

                    if (expressionWithNext != null) {
                        if (expressionWithNext.Member is GrapeIdentifierExpression) {
                            lastExpressionWithoutNext = new GrapeAccessExpression { Member = expressionWithNext.Member };
                        } else {
                            lastExpressionWithoutNext = expressionWithNext.Member as GrapeAccessExpression;
                        }
                    }

                    while (lastExpressionWithoutNext.Member != null && lastExpressionWithoutNext.Member is GrapeAccessExpression) {
                        lastExpressionWithoutNext = lastExpressionWithoutNext.Member as GrapeAccessExpression;
                    }

                    lastParent = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, lastExpressionWithoutNext, lastParent == null ? accessExpression as GrapeEntity : lastParent.GetLogicalParentOfEntityType<GrapeClass>(), out errorMessage)))[0];
                    if (lastParent == null) {
                        break;
                    }

                    if (lastParent is GrapeMethod && !(currentExpression is GrapeCallExpression) && !(accessExpression is GrapeCallExpression) && !(parent is GrapeCallExpression) && detectMethodsUsedAsTypesOrVars) {
                        errorMessage = "Cannot use function '" + lastExpressionWithoutNext.GetAccessExpressionQualifiedId() + "' as a variable or type.";
                        return new GrapeEntity[] { null };
                    }

                    if (lastLastParent != null) {
                        if (lastMemberAccess != null) {
                            GrapeClass c = null;
                            if (lastMemberAccess is GrapeVariable) {
                                c = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeVariable)lastMemberAccess).Type, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            } else if (lastMemberAccess is GrapeMethod) {
                                c = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeMethod)lastMemberAccess).ReturnType, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            }

                            if (c != null) {
                                lastLastParent = c;
                                shouldBeStatic = false;
                                staticnessMatters = true;
                            }

                            lastMemberAccess = null;
                        } else if (lastLastParent is GrapeClass) {
                            shouldBeStatic = !lastClassAccessWasThisOrBaseAccess && !(lastParent is GrapeClass);
                            staticnessMatters = true;
                        } else if (lastLastParent is GrapeField) {
                            GrapeModifier.GrapeModifierType variableModifiers = ((GrapeField)lastLastParent).Modifiers;
                            shouldBeStatic = variableModifiers.Contains(GrapeModifier.GrapeModifierType.Static);
                            lastMemberAccess = lastLastParent as GrapeField;
                            staticnessMatters = true;
                        } else if (lastLastParent is GrapeVariable) {
                            shouldBeStatic = false;
                            staticnessMatters = true;
                            lastMemberAccess = lastLastParent as GrapeVariable;
                        } else if (lastLastParent is GrapeMethod) {
                            GrapeModifier.GrapeModifierType methodModifiers = ((GrapeMethod)lastLastParent).Modifiers;
                            shouldBeStatic = methodModifiers.Contains(GrapeModifier.GrapeModifierType.Static);
                            staticnessMatters = true;
                            lastMemberAccess = lastLastParent as GrapeMethod;
                        }
                    }

                    GrapeModifier.GrapeModifierType modifiers = lastParent.GetPotentialModifiersOfEntity();
                    if (modifiers != 0 && staticnessMatters) {
                        if (shouldBeStatic && !modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorMessage = "Cannot access instance member when the context implies that a static member should be accessed. The expression is '" + identifierExpression.GetAccessExpressionQualifiedId() + "'.";
                            return new GrapeEntity[] { null };
                        } else if (!shouldBeStatic && modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorMessage = "Cannot access static member when the context implies that an instance member should be accessed. The expression is '" + identifierExpression.GetAccessExpressionQualifiedId() + "'.";
                            return new GrapeEntity[] { null };
                        }
                    }

                    lastLastParent = lastParent;
                    if (lastLastParent is GrapeVariable || lastLastParent is GrapeMethod) {
                        lastMemberAccess = lastLastParent;
                    }

                    lastClassAccessWasThisOrBaseAccess = lastExpressionWithoutNext.GetAccessExpressionQualifiedId() == "this" || lastExpressionWithoutNext.GetAccessExpressionQualifiedId() == "base";
                    currentExpression = expressionWithNext.Next;
                }

                entity = lastParent;
                if (entity != null) {
                    GrapeMethod method = entity as GrapeMethod;
                    if (method != null && accessExpression is GrapeCallExpression && !accessExpressionValidator.ValidateMethodSignatureAndOverloads(((GrapeCallExpression)accessExpression), method, GrapeModifier.GrapeModifierType.Public, ref errorMessage)) {
                        return new GrapeEntity[] { entity };
                    }

                    GrapeAccessExpression memberExpressionWithNext = accessExpression.Next == null ? identifierExpression : accessExpression;
                    if (memberExpressionWithNext.Next != null) {
                        GrapeEntity newParent = null;
                        if (entity is GrapeClass) {
                            newParent = entity as GrapeClass;
                        } else if (entity is GrapeMethod) {
                            newParent = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeMethod)entity).ReturnType, parent, out errorMessage)))[0];
                        } else if (entity is GrapeVariable) {
                            newParent = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeVariable)entity).Type, parent, out errorMessage)))[0];
                        }

                        return GetEntitiesForAccessExpression(config, memberExpressionWithNext.Next, newParent, out errorMessage);
                    }

                    return new GrapeEntity[] { entity };
                }
            } else if (accessExpression is GrapeMemberExpression) {
                GrapeEntity foundEntity = null;
                GrapeEntity lastParent = null;
                GrapeEntity lastEntity = null;
                GrapeEntity lastLastEntity = null;
                GrapeEntity lastMemberAccess = null;
                GrapeAccessExpression currentExpression = accessExpression;
                IEnumerable<GrapeEntity> members = null;
                IEnumerable<GrapeEntity> classes = null;
                bool lastClassAccessWasThisOrBaseAccess = false;
                bool shouldBeStatic = false;
                bool staticnessMatters = false;
                while (currentExpression != null) {
                    GrapeAccessExpression expressionWithNext = currentExpression;
                    GrapeAccessExpression lastExpressionWithoutNext = currentExpression;
                    while (true) {
                        if (expressionWithNext.Next != null) {
                            break;
                        }

                        if (expressionWithNext.Member == null || !(expressionWithNext.Member is GrapeAccessExpression)) {
                            break;
                        }

                        expressionWithNext = expressionWithNext.Member as GrapeAccessExpression;
                    }

                    if (expressionWithNext != null) {
                        if (expressionWithNext.Member is GrapeIdentifierExpression) {
                            lastExpressionWithoutNext = new GrapeAccessExpression { Member = expressionWithNext.Member };
                        } else {
                            lastExpressionWithoutNext = expressionWithNext.Member as GrapeAccessExpression;
                        }
                    }

                    while (lastExpressionWithoutNext.Member != null && lastExpressionWithoutNext.Member is GrapeAccessExpression) {
                        lastExpressionWithoutNext = lastExpressionWithoutNext.Member as GrapeAccessExpression;
                    }

                    string expressionWithoutNextQualifiedId = lastExpressionWithoutNext.GetAccessExpressionQualifiedId();
                    IList<GrapeEntity> currentMembers = new List<GrapeEntity>(GetMembersForExpression(config, expressionWithoutNextQualifiedId, lastParent == null ? parent : lastParent));
                    IList<GrapeEntity> currentClasses = new List<GrapeEntity>(GetClassesForExpression(config, expressionWithoutNextQualifiedId, lastParent == null ? parent : lastParent));
                    lastParent = currentClasses.Count() == 1 && currentClasses[0] != null ? currentClasses[0] : currentMembers.Count() > 0 ? currentMembers[0] is GrapeVariable ? astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, ((GrapeVariable)currentMembers[0]).Type), parent.FileName) : currentMembers[0] is GrapeMethod ? astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, ((GrapeMethod)currentMembers[0]).ReturnType), parent.FileName) : null : null;
                    lastEntity = currentClasses.Count() == 1 && currentClasses[0] != null ? currentClasses[0] : currentMembers.Count() > 0 ? currentMembers[0] : null;
                    if (lastParent == null) {
                        members = null;
                        classes = null;
                        break;
                    }

                    if (lastEntity is GrapeMethod && !(currentExpression is GrapeCallExpression) && !(accessExpression is GrapeCallExpression) && !(parent is GrapeCallExpression) && detectMethodsUsedAsTypesOrVars) {
                        errorMessage = "Cannot use method '" + expressionWithoutNextQualifiedId + "' as a variable or type.";
                        return new GrapeEntity[] { null };
                    }

                    if (lastLastEntity != null) {
                        if (lastMemberAccess != null) {
                            GrapeClass c = null;
                            if (lastMemberAccess is GrapeVariable) {
                                c = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeVariable)lastMemberAccess).Type, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            } else if (lastMemberAccess is GrapeMethod) {
                                c = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, ((GrapeMethod)lastMemberAccess).ReturnType, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            }

                            if (c != null) {
                                lastLastEntity = c;
                                shouldBeStatic = false;
                                staticnessMatters = true;
                            }

                            lastMemberAccess = null;
                        } else if (lastLastEntity is GrapeClass) {
                            shouldBeStatic = !lastClassAccessWasThisOrBaseAccess && !(lastEntity is GrapeClass);
                            staticnessMatters = true;
                        } else if (lastLastEntity is GrapeField) {
                            GrapeModifier.GrapeModifierType variableModifiers = ((GrapeField)lastLastEntity).Modifiers;
                            shouldBeStatic = variableModifiers.Contains(GrapeModifier.GrapeModifierType.Static);
                            staticnessMatters = true;
                            lastMemberAccess = lastLastEntity as GrapeField;
                        } else if (lastLastEntity is GrapeVariable) {
                            shouldBeStatic = false;
                            staticnessMatters = true;
                            lastMemberAccess = lastLastEntity as GrapeVariable;
                        } else if (lastLastEntity is GrapeMethod) {
                            GrapeModifier.GrapeModifierType functionModifiers = ((GrapeMethod)lastLastEntity).Modifiers;
                            shouldBeStatic = functionModifiers.Contains(GrapeModifier.GrapeModifierType.Static);
                            staticnessMatters = true;
                            lastMemberAccess = lastLastEntity as GrapeMethod;
                        }
                    }

                    GrapeModifier.GrapeModifierType modifiers = lastEntity.GetPotentialModifiersOfEntity();
                    if (modifiers != 0 && staticnessMatters) {
                        if (shouldBeStatic && !modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorMessage = "Cannot access instance member when the context implies that a static member should be accessed. The expression is '" + accessExpression.GetAccessExpressionQualifiedId() + "'.";
                            return new GrapeEntity[] { null };
                        } else if (!shouldBeStatic && modifiers.Contains(GrapeModifier.GrapeModifierType.Static)) {
                            errorMessage = "Cannot access static member when the context implies that an instance member should be accessed. The expression is '" + accessExpression.GetAccessExpressionQualifiedId() + "'.";
                            return new GrapeEntity[] { null };
                        }
                    }

                    lastLastEntity = lastEntity;
                    if (lastLastEntity is GrapeVariable || lastLastEntity is GrapeMethod) {
                        lastMemberAccess = lastLastEntity;
                    }

                    lastClassAccessWasThisOrBaseAccess = lastExpressionWithoutNext.GetAccessExpressionQualifiedId() == "this" || lastExpressionWithoutNext.GetAccessExpressionQualifiedId() == "base";

                    members = currentMembers;
                    classes = currentClasses;
                    currentExpression = expressionWithNext.Next;
                }

                foundEntity = lastParent;
                List<GrapeEntity> entities = new List<GrapeEntity>();
                if (members != null) {
                    entities.AddRange(members);
                    if (classes != null && classes.Count() > 0) {
                        List<GrapeEntity> membersToRemove = new List<GrapeEntity>();
                        foreach (GrapeClass c in classes) {
                            if (c != null) {
                                foreach (GrapeEntity member in members) {
                                    if (member is GrapeMethod && ((GrapeMethod)member).Name == c.Name) {
                                        membersToRemove.Add(member);
                                    }
                                }
                            }
                        }

                        membersToRemove.ForEach(m => entities.Remove(m));
                    }
                }

                if (classes != null) {
                    entities.AddRange(classes);
                }

                if (entities.Count == 0) {
                    if (errorMessage == "") {
                        errorMessage = "Cannot find variable, field, type or method with name '" + memberExpressionQualifiedId + "'.";
                    }

                    return new GrapeEntity[] { null };
                }

                GrapeMethod method = null;
                foreach (GrapeEntity entity in entities) {
                    if (entity is GrapeMethod) {
                        method = entity as GrapeMethod;
                        break;
                    }
                }

                if (method != null) {
                    GrapeAccessExpression currentMemberExpression = accessExpression;
                    GrapeCallExpression callExpression = null;
                    while (currentMemberExpression != null) {
                        if (currentMemberExpression is GrapeCallExpression && ((GrapeCallExpression)currentMemberExpression).GetAccessExpressionQualifiedId() == method.Name) {
                            callExpression = currentMemberExpression as GrapeCallExpression;
                        }

                        currentMemberExpression = currentMemberExpression.Next;
                    }

                    if (callExpression != null && !accessExpressionValidator.ValidateMethodSignatureAndOverloads(callExpression, method, GrapeModifier.GrapeModifierType.Public, ref errorMessage)) {
                        return entities;
                    }
                }

                return entities;
            }

            return new GrapeEntity[] { null };
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeType expression, GrapeType type) { // left for compat
            return expression.ToString() == type.ToString();
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeType expression, GrapeType type, ref string errorMessage) { // left for compat
            return expression.ToString() == type.ToString();
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeExpression expression, GrapeType type) { // left for compat
            string dummyMessage = "";
            return DoesExpressionResolveToType(config, parent, expression, type, ref dummyMessage);
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeExpression expression, GrapeType type, ref string errorMessage) { // left for compat
            return DoesExpressionResolveToType(config, parent, expression, type.ToString(), ref errorMessage);
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeType expression, string typeName) { // left for compat
            return expression.ToString() == typeName;
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeType expression, string typeName, ref string errorMessage) { // left for compat
            return expression.ToString() == typeName;
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeExpression expression, GrapeExpression type) {
            string dummyMessage = "";
            return DoesExpressionResolveToType(config, parent, expression, GetTypeNameForTypeAccessExpression(config, type, ref dummyMessage));
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeExpression expression, string typeName) {
            string dummyMessage = "";
            return DoesExpressionResolveToType(config, parent, expression, typeName, ref dummyMessage);
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeExpression expression, string typeName, ref string errorMessage) {
            if (parent == null) {
                throw new ArgumentNullException("parent");
            }

            if (expression is GrapeLiteralExpression) {
                if (typeName == literalTypes[((GrapeLiteralExpression)expression).Type]) {
                    return true;
                } else {
                    GrapeClass c = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, typeName, expression.FileName);
                    if (c != null && c.IsClassInInheritanceTree(config, astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, literalTypes[((GrapeLiteralExpression)expression).Type], expression.FileName))) {
                        return true;
                    }
                }

                return false;
            } else if (expression is GrapeConditionalExpression) {
                GrapeConditionalExpression conditionalExpression = expression as GrapeConditionalExpression;
                if (conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryAnd || conditionalExpression.Type == GrapeConditionalExpression.GrapeConditionalExpressionType.BinaryOr) {
                    return DoesExpressionResolveToType(config, parent, conditionalExpression.Left, "int_base", ref errorMessage) && DoesExpressionResolveToType(config, parent, conditionalExpression.Right, "int_base", ref errorMessage);
                }

                string leftType = GetTypeNameForTypeAccessExpression(config, conditionalExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, conditionalExpression.Right, ref errorMessage);
                if (errorMessage != "") {
                    return false;
                }

                bool result = leftType == rightType;
                if (!result) {
                    GrapeClass left = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, leftType, parent.FileName);
                    GrapeClass right = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, rightType, parent.FileName);
                    if (left != null && right != null) {
                        if (left.IsClassInInheritanceTree(config, right)) {
                            return true;
                        }
                    }
                }

                if (!result) {
                    errorMessage = "Cannot resolve left and right expressions to the same type. The resolved left type is '" + leftType + "' and the resolved right type is '" + rightType + "'.";
                }

                return result;
            } else if (expression is GrapeMemberExpression) {
                GrapeEntity entity = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, expression as GrapeMemberExpression, parent, out errorMessage)))[0];
                if (entity != null) {
                    GrapeClass type = null;
                    if (entity is GrapeVariable) {
                        type = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetCorrectNativeTypeName(GetTypeNameForTypeAccessExpression(config, ((GrapeVariable)entity).Type, ref errorMessage)), parent.FileName);
                    } else if (entity is GrapeMethod) {
                        type = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetCorrectNativeTypeName(GetTypeNameForTypeAccessExpression(config, ((GrapeMethod)entity).ReturnType, ref errorMessage)), parent.FileName);
                    } else if (entity is GrapeClass) {
                        type = entity as GrapeClass;
                    }

                    if (type != null) {
                        return type.Name == GetCorrectNativeTypeName(typeName) || IsTypeInClassInheritanceTree(config, typeName, type);
                    }
                } else {
                    if (expression.GetType() == typeof(GrapeMemberExpression)) {
                        string qualifiedId = ((GrapeMemberExpression)expression).GetAccessExpressionQualifiedId();
                        foreach (GalaxyConstantAttribute constant in GalaxyNativeInterfaceAggregator.Constants) {
                            if (constant.Name == qualifiedId) {
                                return GetCorrectNativeTypeName(constant.Type) == GetCorrectNativeTypeName(typeName);
                            }
                        }
                    } else if (expression.GetType() == typeof(GrapeCallExpression)) {
                        GrapeCallExpression callExpression = expression as GrapeCallExpression;
                        GrapeAccessExpression accessExpression = callExpression.GetAccessExpressionInAccessExpression();
                        string qualifiedId = accessExpression.GetAccessExpressionQualifiedId();
                        foreach (GalaxyFunctionAttribute function in GalaxyNativeInterfaceAggregator.Functions) {
                            if (function.Name == qualifiedId) {
                                if (!IsSignatureValid(config, function, callExpression, out errorMessage)) {
                                    return false;
                                }

                                return GetCorrectNativeTypeName(function.Type) == GetCorrectNativeTypeName(typeName);
                            }
                        }
                    }
                }
            } else if (expression is GrapeNameofExpression) {
                GrapeNameofExpression nameofExpression = expression as GrapeNameofExpression;
                if (nameofExpression.Value.GetType() != typeof(GrapeMemberExpression)) {
                    errorMessage = "Cannot resolve expression to type, field or function.";
                    if (!config.ContinueOnError) {
                        return false;
                    }
                }

                string qualifiedId = nameofExpression.Value.ToString();
                GrapeEntity valueEntity = (new List<GrapeEntity>(GetEntitiesForAccessExpression(config, nameofExpression.Value, expression, out errorMessage, false)))[0];
                if (valueEntity == null) {
                    errorMessage = "Cannot find entity for expression '" + qualifiedId + "'. " + errorMessage;
                    if (!config.ContinueOnError) {
                        return false;
                    }
                }

                GrapeClass c = expression.GetLogicalParentOfEntityType<GrapeClass>();
                GrapeModifier.GrapeModifierType modifiers = c.GetAppropriateModifiersForEntityAccess(config, valueEntity);
                GrapeModifier.GrapeModifierType potentialModifiers = valueEntity.GetPotentialModifiersOfEntity();
                bool invalidModifiers = false;
                if (modifiers != 0) {
                    if (potentialModifiers == 0) {
                        if (modifiers == GrapeModifier.GrapeModifierType.Public) {
                            invalidModifiers = false;
                        } else {
                            invalidModifiers = true;
                        }
                    } else {
                        if (modifiers != potentialModifiers) {
                            invalidModifiers = true;
                        }
                    }
                } else {
                    invalidModifiers = true;
                }

                if (invalidModifiers) {
                    errorMessage = "Cannot access member '" + qualifiedId + "'.";
                    if (!config.ContinueOnError) {
                        return false;
                    }
                }

                return typeName == "string_base";
            } else if (expression is GrapeAddExpression) {
                GrapeAddExpression addExpression = expression as GrapeAddExpression;
                string leftType = GetTypeNameForTypeAccessExpression(config, addExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, addExpression.Right, ref errorMessage);
                if (errorMessage != "") {
                    return false;
                }

                return leftType == rightType;
            } else if (expression is GrapeMultiplicationExpression) {
                GrapeMultiplicationExpression multiplicationExpression = expression as GrapeMultiplicationExpression;
                string leftType = GetTypeNameForTypeAccessExpression(config, multiplicationExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, multiplicationExpression.Right, ref errorMessage);
                if (errorMessage != "") {
                    return false;
                }

                return leftType == rightType;
            } else if (expression is GrapeShiftExpression) {
                GrapeShiftExpression shiftExpression = expression as GrapeShiftExpression;
                string leftType = GetTypeNameForTypeAccessExpression(config, shiftExpression.Left, ref errorMessage);
                string rightType = GetTypeNameForTypeAccessExpression(config, shiftExpression.Right, ref errorMessage);
                if (errorMessage != "") {
                    return false;
                }

                return leftType == rightType;
            } else if (expression is GrapeTypecastExpression) {
                GrapeTypecastExpression typecastExpression = expression as GrapeTypecastExpression;
                if (DoesExpressionResolveToType(config, typecastExpression, typecastExpression.Value, "text_base")) {
                    errorMessage = "Cannot cast from type 'text_base'.";
                    return false;
                }

                return DoesExpressionResolveToType(config, parent, typecastExpression.TypeName, typeName, ref errorMessage);
            } else if (expression is GrapeUnaryExpression) {
                GrapeUnaryExpression unaryExpression = expression as GrapeUnaryExpression;
                return DoesExpressionResolveToType(config, parent, unaryExpression.Value, typeName, ref errorMessage);
            }

            return false;
        }

        public bool DoesExpressionResolveToType(GrapeCodeGeneratorConfiguration config, GrapeEntity parent, GrapeExpression expression, GrapeExpression type, ref string errorMessage) {
            return DoesExpressionResolveToType(config, parent, expression, GetTypeNameForTypeAccessExpression(config, type, ref errorMessage), ref errorMessage);
        }

        public string GetCorrectNativeTypeName(string typeName) {
            IList<string> segmentsInTypeName = astUtils.GetSegmentsInQualifiedId(typeName, true);
            string actualTypeName = segmentsInTypeName.Count > 0 ? segmentsInTypeName[segmentsInTypeName.Count - 1] : typeName;
            foreach (Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute> tuple in GalaxyNativeInterfaceAggregator.Types) {
                if (tuple.Item1.NativeAlias == actualTypeName) {
                    actualTypeName = tuple.Item1.TypeName;
                }
            }

            int lastDotIndex = typeName.LastIndexOf('.');
            if (lastDotIndex > -1) {
                typeName = typeName.Substring(0, lastDotIndex + 1) + actualTypeName;
            } else {
                typeName = actualTypeName;
            }

            return typeName;
        }

        public bool DoesTypeExist(GrapeCodeGeneratorConfiguration config, string typeName, string fileName) {
            GrapeClass c = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetCorrectNativeTypeName(typeName), fileName);
            return c != null;
        }

        public bool DoesTypeExist(GrapeCodeGeneratorConfiguration config, GrapeExpression type, string fileName) {
            string dummyMessage = "";
            return DoesTypeExist(config, GetTypeNameForTypeAccessExpression(config, type, ref dummyMessage), fileName);
        }

        public bool DoesTypeExist(GrapeCodeGeneratorConfiguration config, GrapeType type, string fileName) {
            string dummyMessage = "";
            return DoesTypeExist(config, GetTypeNameForTypeAccessExpression(config, type, ref dummyMessage), fileName);
        }

        public GrapeTypeCheckingUtilities() {
            Instance = this;
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.True, "bool_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.False, "bool_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Hexadecimal, "int_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Int, "int_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Null, "object");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.Real, "fixed_base");
            literalTypes.Add(GrapeLiteralExpression.GrapeLiteralExpressionType.String, "string_base");
        }
    }
}