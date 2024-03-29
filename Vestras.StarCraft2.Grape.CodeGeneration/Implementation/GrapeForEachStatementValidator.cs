﻿using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapeForEachStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;
        [Import]
        private GrapeVariableValidator variableValidator = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeForEachStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeForEachStatement s = obj as GrapeForEachStatement;
                if (s != null) {
                    string errorMessage = "";
                    if (!typeCheckingUtils.DoesExpressionResolveToType(Config, s, s.ValueExpression, "abstract_enumerator", ref errorMessage)) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "Cannot resolve expression to the type 'abstract_enumerator'. " + errorMessage, FileName = s.FileName, Entity = s.ValueExpression });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    return variableValidator.ValidateNode(s.IteratorVariable);
                }
            }

            return true;
        }
    }
}
