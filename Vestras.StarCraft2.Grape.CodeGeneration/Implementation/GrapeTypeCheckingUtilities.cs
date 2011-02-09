using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export]
    internal class GrapeTypeCheckingUtilities {
        private static Dictionary<GrapeLiteralExpression.GrapeLiteralExpressionType, string> literalTypes = new Dictionary<GrapeLiteralExpression.GrapeLiteralExpressionType, string>();

        [Import]
        private GrapeAstUtilities astUtils = null;
        [Import]
        private GrapeMemberExpressionValidator memberExpressionValidator = null;
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
                string dummyMessage = "";
                string inheritsTypeName = GetTypeNameForTypeAccessExpression(config, c.Inherits, ref dummyMessage);
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

        public string GetTypeNameForTypeAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeExpression expression) {
            string dummyMessage = "";
            return GetTypeNameForTypeAccessExpression(config, expression, ref dummyMessage);
        }

        public string GetTypeNameForTypeAccessExpression(GrapeCodeGeneratorConfiguration config, GrapeExpression expression, ref string errorMessage) {
            if (expression is GrapeIdentifierExpression) {
                return GetCorrectNativeTypeName(((GrapeIdentifierExpression)expression).Identifier);
            } else if (expression is GrapeMemberExpression) {
                GrapeEntity entity = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeMemberExpression)expression), expression, out errorMessage)))[0];
                if (entity is GrapeVariable && ((GrapeVariable)entity).Type is GrapeMemberExpression) {
                    return GetCorrectNativeTypeName((((GrapeVariable)entity).Type as GrapeMemberExpression).GetMemberExpressionQualifiedId());
                } else if (entity is GrapeVariable && ((GrapeVariable)entity).Type is GrapeIdentifierExpression) {
                    return GetCorrectNativeTypeName((((GrapeVariable)entity).Type as GrapeIdentifierExpression).Identifier);
                } else if (entity is GrapeFunction && ((GrapeFunction)entity).ReturnType is GrapeMemberExpression) {
                    return GetCorrectNativeTypeName((((GrapeFunction)entity).ReturnType as GrapeMemberExpression).GetMemberExpressionQualifiedId());
                } else if (entity is GrapeFunction && ((GrapeFunction)entity).ReturnType is GrapeIdentifierExpression) {
                    return GetCorrectNativeTypeName((((GrapeFunction)entity).ReturnType as GrapeIdentifierExpression).Identifier);
                } else if (entity is GrapeClass) {
                    return GetCorrectNativeTypeName((entity as GrapeClass).Name);
                }

                return GetCorrectNativeTypeName(((GrapeMemberExpression)expression).GetMemberExpressionQualifiedId());
            } else if (expression is GrapeStackExpression) {
                return GetTypeNameForTypeAccessExpression(config, ((GrapeStackExpression)expression).Child, ref errorMessage);
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
                parent = parent.GetLogicalParentOfEntityType<GrapeFunction>();
                if (parent == null) {
                    parent = oldParent.GetLogicalParentOfEntityType<GrapeClass>();
                }
            }

            IEnumerable<GrapeVariable> variables = astUtils.GetVariablesWithNameFromImportedPackagesInFile(config, variableName, parent.FileName, parent);
            if (variables.Count() == 0) {
                variables = astUtils.GetVariablesWithNameFromImportedPackagesInFile(config, variableName, parent.FileName, parent.GetLogicalParentOfEntityType<GrapeClass>());
            }

            IEnumerable<GrapeFunction> functions = astUtils.GetFunctionsWithNameFromImportedPackagesInFile(config, variableName, parent.FileName, parent.GetLogicalParentOfEntityType<GrapeClass>());
            List<GrapeEntity> list = new List<GrapeEntity>();
            list.AddRange(variables);
            list.AddRange(functions);
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

        public string GetFunctionSignatureString(GrapeCodeGeneratorConfiguration config, string name, GrapeExpression returnType, List<GrapeExpression> parameters) {
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

        public List<GrapeFunction> GetFunctionWithSignature(GrapeCodeGeneratorConfiguration config, List<GrapeFunction> functions, string[] modifiers, string name, GrapeExpression returnType, List<GrapeExpression> parameters, ref string errorMessage) {
            List<GrapeFunction> foundFunctions = new List<GrapeFunction>();
            string parameterErrorMessage = "";
            foreach (GrapeFunction function in functions) {
                if (function.Name == name && GetTypeNameForTypeAccessExpression(config, function.ReturnType) == GetTypeNameForTypeAccessExpression(config, returnType) && function.Parameters.Count == parameters.Count) {
                    if (modifiers != null) {
                        bool shouldContinue = false;
                        foreach (string modifier in modifiers) {
                            bool hasModifier = false;
                            foreach (string functionModifier in function.Modifiers.Split(' ')) {
                                if (functionModifier == modifier) {
                                    hasModifier = true;
                                    break;
                                }
                            }

                            if (!hasModifier) {
                                shouldContinue = true;
                                break;
                            }
                        }

                        if (shouldContinue) {
                            continue;
                        }
                    }

                    int currentParameterIndex = 0;
                    bool validParameters = true;
                    foreach (GrapeVariable param in function.Parameters) {
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
                        foundFunctions.Add(function);
                    }
                }
            }

            if (foundFunctions.Count == 0) {
                errorMessage = "Cannot find function with signature '" + GetFunctionSignatureString(config, name, returnType, parameters) + "'. " + parameterErrorMessage;
            }

            return foundFunctions;
        }

        public IEnumerable<GrapeEntity> GetEntitiesForMemberExpression(GrapeCodeGeneratorConfiguration config, GrapeMemberExpression memberExpression, GrapeEntity parent, out string errorMessage) {
            errorMessage = "";
            string memberExpressionQualifiedId = memberExpression.GetMemberExpressionQualifiedId();
            if (memberExpressionQualifiedId == "this") {
                GrapeClass thisClass = parent.GetLogicalParentOfEntityType<GrapeClass>();
                if (thisClass != null) {
                    return new GrapeEntity[] { thisClass };
                }

                return new GrapeEntity[] { null };
            } else if (memberExpressionQualifiedId == "base") {
                GrapeClass thisClass = parent.GetLogicalParentOfEntityType<GrapeClass>();
                if (thisClass != null && thisClass.Inherits != null) {
                    GrapeClass baseClass = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, thisClass.Inherits, ref errorMessage), parent.FileName);
                    return new GrapeEntity[] { baseClass };
                }

                return new GrapeEntity[] { null };
            } else if (memberExpression is GrapeObjectCreationExpression) {
                GrapeObjectCreationExpression objectCreationExpression = memberExpression as GrapeObjectCreationExpression;
                GrapeClass c = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, objectCreationExpression.Member), memberExpression.FileName);
                if (c != null) {
                    List<GrapeFunction> constructors = c.GetConstructors();
                    if (constructors.Count == 0) {
                        return new GrapeEntity[] { new GrapeFunction { FileName = c.FileName, Block = new GrapeBlock(), Parent = c.Block, Type = GrapeFunction.GrapeFunctionType.Constructor, ReturnType = new GrapeIdentifierExpression { Identifier = c.Name }, Modifiers = "public", Name = c.Name } };
                    } else {
                        errorMessage = "";
                        string[] modifiers = objectCreationExpression.GetLogicalParentOfEntityType<GrapeClass>().GetAppropriateModifiersForEntityAccess(config, constructors[0]);
                        List<GrapeFunction> functionsWithSignature = GetFunctionWithSignature(config, constructors, modifiers, c.Name, new GrapeIdentifierExpression { Identifier = c.Name }, new List<GrapeExpression>(objectCreationExpression.Parameters), ref errorMessage);
                        if (functionsWithSignature.Count > 1) {
                            errorMessage = "Multiple functions with signature '" + GetFunctionSignatureString(config, c.Name, new GrapeIdentifierExpression { Identifier = c.Name }, new List<GrapeExpression>(objectCreationExpression.Parameters)) + "' found.";
                            return new GrapeEntity[] { null };
                        }

                        if (functionsWithSignature.Count == 0) {
                            return new GrapeEntity[] { null };
                        }

                        return functionsWithSignature;
                    }
                }

                return new GrapeEntity[] { null };
            } else if (memberExpression is GrapeCallExpression || memberExpression is GrapeSetExpression) {
                GrapeMemberExpression identifierExpression = memberExpression.GetMemberExpressionInMemberExpression();
                if (identifierExpression == null && memberExpression.Member is GrapeIdentifierExpression) {
                    identifierExpression = new GrapeMemberExpression { Member = memberExpression.Member as GrapeIdentifierExpression };
                }

                GrapeEntity entity = null;
                GrapeEntity lastParent = null;
                GrapeEntity lastLastParent = null;
                GrapeEntity lastMemberAccess = null;
                GrapeMemberExpression currentExpression = identifierExpression;
                bool lastClassAccessWasThisOrBaseAccess = false;
                bool shouldBeStatic = false;
                bool staticnessMatters = false;
                while (currentExpression != null) {
                    GrapeMemberExpression expressionWithNext = currentExpression;
                    GrapeMemberExpression lastExpressionWithoutNext = currentExpression;
                    while (true) {
                        if (expressionWithNext.Next != null) {
                            break;
                        }

                        if (expressionWithNext.Member == null || !(expressionWithNext.Member is GrapeMemberExpression)) {
                            break;
                        }

                        expressionWithNext = expressionWithNext.Member as GrapeMemberExpression;
                    }

                    if (expressionWithNext != null) {
                        if (expressionWithNext.Member is GrapeIdentifierExpression) {
                            lastExpressionWithoutNext = new GrapeMemberExpression { Member = expressionWithNext.Member };
                        } else {
                            lastExpressionWithoutNext = expressionWithNext.Member as GrapeMemberExpression;
                        }
                    }

                    while (lastExpressionWithoutNext.Member != null && lastExpressionWithoutNext.Member is GrapeMemberExpression) {
                        lastExpressionWithoutNext = lastExpressionWithoutNext.Member as GrapeMemberExpression;
                    }

                    lastParent = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, lastExpressionWithoutNext, lastParent == null ? parent : lastParent.GetLogicalParentOfEntityType<GrapeClass>(), out errorMessage)))[0];
                    if (lastParent == null) {
                        break;
                    }

                    if (lastLastParent != null) {
                        if (lastMemberAccess != null) {
                            GrapeClass c = null;
                            if (lastMemberAccess is GrapeVariable) {
                                c = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeVariable)lastMemberAccess).Type as GrapeMemberExpression, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            } else if (lastMemberAccess is GrapeFunction) {
                                c = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeFunction)lastMemberAccess).ReturnType as GrapeMemberExpression, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            }

                            if (c != null) {
                                lastLastParent = c;
                                shouldBeStatic = false;
                                staticnessMatters = true;
                            }

                            lastMemberAccess = null;
                        } else if (lastLastParent is GrapeClass) {
                            shouldBeStatic = !lastClassAccessWasThisOrBaseAccess;
                            staticnessMatters = true;
                        } else if (lastLastParent is GrapeField) {
                            string variableModifiers = ((GrapeField)lastLastParent).Modifiers;
                            shouldBeStatic = variableModifiers.Contains("static");
                            lastMemberAccess = lastLastParent as GrapeField;
                            staticnessMatters = true;
                        } else if (lastLastParent is GrapeVariable) {
                            shouldBeStatic = false;
                            staticnessMatters = true;
                            lastMemberAccess = lastLastParent as GrapeVariable;
                        } else if (lastLastParent is GrapeFunction) {
                            string functionModifiers = ((GrapeFunction)lastLastParent).Modifiers;
                            shouldBeStatic = functionModifiers.Contains("static");
                            staticnessMatters = true;
                            lastMemberAccess = lastLastParent as GrapeFunction;
                        }
                    }

                    string modifiers = lastParent.GetPotentialModifiersOfEntity();
                    if (modifiers != null && staticnessMatters) {
                        if (shouldBeStatic && !modifiers.Contains("static")) {
                            errorMessage = "Cannot access instance member when the context implies that a static member should be accessed.";
                            return new GrapeEntity[] { null };
                        } else if (!shouldBeStatic && modifiers.Contains("static")) {
                            errorMessage = "Cannot access static member when the context implies that an instance member should be accessed.";
                            return new GrapeEntity[] { null };
                        }
                    }

                    lastLastParent = lastParent;
                    if (lastLastParent is GrapeVariable || lastLastParent is GrapeFunction) {
                        lastMemberAccess = lastLastParent;
                    }

                    lastClassAccessWasThisOrBaseAccess = lastExpressionWithoutNext.GetMemberExpressionQualifiedId() == "this" || lastExpressionWithoutNext.GetMemberExpressionQualifiedId() == "base";
                    currentExpression = expressionWithNext.Next;
                }

                entity = lastParent;
                if (entity != null) {
                    GrapeFunction function = entity as GrapeFunction;
                    if (function != null && memberExpression is GrapeCallExpression && !memberExpressionValidator.ValidateFunctionSignatureAndOverloads(((GrapeCallExpression)memberExpression), function, null, ref errorMessage)) {
                        return new GrapeEntity[] { entity };
                    }

                    GrapeMemberExpression memberExpressionWithNext = memberExpression.Next == null ? identifierExpression : memberExpression;
                    if (memberExpressionWithNext.Next != null) {
                        GrapeEntity newParent = null;
                        if (entity is GrapeClass) {
                            newParent = entity as GrapeClass;
                        } else if (entity is GrapeFunction && ((GrapeFunction)entity).ReturnType is GrapeMemberExpression) {
                            newParent = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeFunction)entity).ReturnType as GrapeMemberExpression, parent, out errorMessage)))[0];
                        } else if (entity is GrapeVariable && ((GrapeVariable)entity).Type is GrapeMemberExpression) {
                            newParent = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeVariable)entity).Type as GrapeMemberExpression, parent, out errorMessage)))[0];
                        }

                        return GetEntitiesForMemberExpression(config, memberExpressionWithNext.Next, newParent, out errorMessage);
                    }

                    return new GrapeEntity[] { entity };
                }
            } else if (memberExpression is GrapeMemberExpression) {
                GrapeEntity foundEntity = null;
                GrapeEntity lastParent = null;
                GrapeEntity lastEntity = null;
                GrapeEntity lastLastEntity = null;
                GrapeEntity lastMemberAccess = null;
                GrapeMemberExpression currentExpression = memberExpression;
                IEnumerable<GrapeEntity> members = null;
                IEnumerable<GrapeEntity> classes = null;
                bool lastClassAccessWasThisOrBaseAccess = false;
                bool shouldBeStatic = false;
                bool staticnessMatters = false;
                while (currentExpression != null) {
                    GrapeMemberExpression expressionWithNext = currentExpression;
                    GrapeMemberExpression lastExpressionWithoutNext = currentExpression;
                    while (true) {
                        if (expressionWithNext.Next != null) {
                            break;
                        }

                        if (expressionWithNext.Member == null || !(expressionWithNext.Member is GrapeMemberExpression)) {
                            break;
                        }

                        expressionWithNext = expressionWithNext.Member as GrapeMemberExpression;
                    }

                    if (expressionWithNext != null) {
                        if (expressionWithNext.Member is GrapeIdentifierExpression) {
                            lastExpressionWithoutNext = new GrapeMemberExpression { Member = expressionWithNext.Member };
                        } else {
                            lastExpressionWithoutNext = expressionWithNext.Member as GrapeMemberExpression;
                        }
                    }

                    while (lastExpressionWithoutNext.Member != null && lastExpressionWithoutNext.Member is GrapeMemberExpression) {
                        lastExpressionWithoutNext = lastExpressionWithoutNext.Member as GrapeMemberExpression;
                    }

                    string expressionWithoutNextQualifiedId = lastExpressionWithoutNext.GetMemberExpressionQualifiedId();
                    IList<GrapeEntity> currentMembers = new List<GrapeEntity>(GetMembersForExpression(config, expressionWithoutNextQualifiedId, lastParent == null ? parent : lastParent));
                    IList<GrapeEntity> currentClasses = new List<GrapeEntity>(GetClassesForExpression(config, expressionWithoutNextQualifiedId, lastParent == null ? parent : lastParent));
                    lastParent = currentClasses.Count() == 1 && currentClasses[0] != null ? currentClasses[0] : currentMembers.Count() > 0 ? currentMembers[0] is GrapeVariable ? astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, ((GrapeVariable)currentMembers[0]).Type), parent.FileName) : currentMembers[0] is GrapeFunction ? astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetTypeNameForTypeAccessExpression(config, ((GrapeFunction)currentMembers[0]).ReturnType), parent.FileName) : null : null;
                    lastEntity = currentClasses.Count() == 1 && currentClasses[0] != null ? currentClasses[0] : currentMembers.Count() > 0 ? currentMembers[0] : null;
                    if (lastParent == null) {
                        members = null;
                        classes = null;
                        break;
                    }

                    if (lastLastEntity != null) {
                        if (lastMemberAccess != null) {
                            GrapeClass c = null;
                            if (lastMemberAccess is GrapeVariable) {
                                c = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeVariable)lastMemberAccess).Type as GrapeMemberExpression, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            } else if (lastMemberAccess is GrapeFunction) {
                                c = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, ((GrapeFunction)lastMemberAccess).ReturnType as GrapeMemberExpression, lastMemberAccess, out errorMessage)))[0] as GrapeClass;
                            }

                            if (c != null) {
                                lastLastEntity = c;
                                shouldBeStatic = false;
                                staticnessMatters = true;
                            }

                            lastMemberAccess = null;
                        } else if (lastLastEntity is GrapeClass) {
                            shouldBeStatic = !lastClassAccessWasThisOrBaseAccess;
                            staticnessMatters = true;
                        } else if (lastLastEntity is GrapeField) {
                            string variableModifiers = ((GrapeField)lastLastEntity).Modifiers;
                            shouldBeStatic = variableModifiers.Contains("static");
                            staticnessMatters = true;
                            lastMemberAccess = lastLastEntity as GrapeField;
                        } else if (lastLastEntity is GrapeVariable) {
                            shouldBeStatic = false;
                            staticnessMatters = true;
                            lastMemberAccess = lastLastEntity as GrapeVariable;
                        } else if (lastLastEntity is GrapeFunction) {
                            string functionModifiers = ((GrapeFunction)lastLastEntity).Modifiers;
                            shouldBeStatic = functionModifiers.Contains("static");
                            staticnessMatters = true;
                            lastMemberAccess = lastLastEntity as GrapeFunction;
                        }
                    }

                    string modifiers = lastEntity.GetPotentialModifiersOfEntity();
                    if (modifiers != null && staticnessMatters) {
                        if (shouldBeStatic && !modifiers.Contains("static")) {
                            errorMessage = "Cannot access instance member when the context implies that a static member should be accessed.";
                            return new GrapeEntity[] { null };
                        } else if (!shouldBeStatic && modifiers.Contains("static")) {
                            errorMessage = "Cannot access static member when the context implies that an instance member should be accessed.";
                            return new GrapeEntity[] { null };
                        }
                    }

                    lastLastEntity = lastEntity;
                    if (lastLastEntity is GrapeVariable || lastLastEntity is GrapeFunction) {
                        lastMemberAccess = lastLastEntity;
                    }

                    lastClassAccessWasThisOrBaseAccess = lastExpressionWithoutNext.GetMemberExpressionQualifiedId() == "this" || lastExpressionWithoutNext.GetMemberExpressionQualifiedId() == "base";

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
                                    if (member is GrapeFunction && ((GrapeFunction)member).Name == c.Name) {
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
                    errorMessage = "Cannot find variable, field, type or function with name '" + memberExpressionQualifiedId + "'.";
                    return new GrapeEntity[] { null };
                }

                GrapeFunction function = null;
                foreach (GrapeEntity entity in entities) {
                    if (entity is GrapeFunction) {
                        function = entity as GrapeFunction;
                        break;
                    }
                }

                if (function != null) {
                    GrapeMemberExpression currentMemberExpression = memberExpression;
                    GrapeCallExpression callExpression = null;
                    while (currentMemberExpression != null) {
                        if (currentMemberExpression is GrapeCallExpression && ((GrapeCallExpression)currentMemberExpression).GetMemberExpressionQualifiedId() == function.Name) {
                            callExpression = currentMemberExpression as GrapeCallExpression;
                        }

                        currentMemberExpression = currentMemberExpression.Next;
                    }

                    if (callExpression != null && !memberExpressionValidator.ValidateFunctionSignatureAndOverloads(callExpression, function, null, ref errorMessage)) {
                        return entities;
                    }
                }

                return entities;
            }

            return new GrapeEntity[] { null };
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

                return leftType == rightType;
            } else if (expression is GrapeMemberExpression) {
                GrapeEntity entity = (new List<GrapeEntity>(GetEntitiesForMemberExpression(config, expression as GrapeMemberExpression, parent, out errorMessage)))[0];
                if (entity != null) {
                    GrapeClass type = null;
                    if (entity is GrapeVariable) {
                        type = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetCorrectNativeTypeName(GetTypeNameForTypeAccessExpression(config, ((GrapeVariable)entity).Type, ref errorMessage)), parent.FileName);
                    } else if (entity is GrapeFunction) {
                        type = astUtils.GetClassWithNameFromImportedPackagesInFile(config.Ast, GetCorrectNativeTypeName(GetTypeNameForTypeAccessExpression(config, ((GrapeFunction)entity).ReturnType, ref errorMessage)), parent.FileName);
                    } else if (entity is GrapeClass) {
                        type = entity as GrapeClass;
                    }

                    if (type != null) {
                        return type.Name == GetCorrectNativeTypeName(typeName) || IsTypeInClassInheritanceTree(config, typeName, type);
                    }
                } else {
                    if (expression.GetType() == typeof(GrapeMemberExpression)) {
                        string qualifiedId = ((GrapeMemberExpression)expression).GetMemberExpressionQualifiedId();
                        foreach (GalaxyConstantAttribute constant in GalaxyNativeInterfaceAggregator.Constants) {
                            if (constant.Name == qualifiedId) {
                                return GetCorrectNativeTypeName(constant.Type) == GetCorrectNativeTypeName(typeName);
                            }
                        }
                    } else if (expression.GetType() == typeof(GrapeCallExpression)) {
                        GrapeCallExpression callExpression = expression as GrapeCallExpression;
                        GrapeMemberExpression memberExpression = callExpression.GetMemberExpressionInMemberExpression();
                        string qualifiedId = memberExpression.GetMemberExpressionQualifiedId();
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
            } else if (expression is GrapeStackExpression) {
                GrapeStackExpression stackExpression = expression as GrapeStackExpression;
                return DoesExpressionResolveToType(config, parent, stackExpression.Child, typeName, ref errorMessage);
            } else if (expression is GrapeTypecastExpression) {
                GrapeTypecastExpression typecastExpression = expression as GrapeTypecastExpression;
                if (DoesExpressionResolveToType(config, typecastExpression, typecastExpression.Value, "text_base")) {
                    errorMessage = "Cannot cast from type 'text_base'.";
                    return false;
                }

                return DoesExpressionResolveToType(config, parent, typecastExpression.Type, typeName, ref errorMessage);
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
