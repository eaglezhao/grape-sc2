using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator)), Export]
    internal class GrapeMemberExpressionValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { 
                    typeof(GrapeMemberExpression),
                    typeof(GrapeArrayAccessExpression),
                    typeof(GrapeCallExpression),
                    typeof(GrapeSetExpression)
                };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapeMemberExpression s = obj as GrapeMemberExpression;
                if (s != null) {
                    GrapeArrayAccessExpression arrayAccessExpression = s as GrapeArrayAccessExpression;
                    GrapeCallExpression callExpression = s as GrapeCallExpression;
                    GrapeSetExpression setExpression = s as GrapeSetExpression;
                    if (arrayAccessExpression != null) {
                    } else if (callExpression != null) {
                    } else if (setExpression != null) {
                    }
                }
            }

            return true;
        }
    }
}
