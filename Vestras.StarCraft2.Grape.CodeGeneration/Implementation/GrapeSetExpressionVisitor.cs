using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor)), Export]
    internal sealed class GrapeSetExpressionVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] {
                    typeof(GrapeSetExpression)
                };
            }
        }

        public void VisitNode(object obj) {
            GrapeSetExpression s = obj as GrapeSetExpression;
            if (s != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(s);
                }

                if (isValid) {
                    // TODO: insert set expression code generation here.
                }
            }
        }
    }
}
