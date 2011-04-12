using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor)), Export]
    internal sealed class GrapeObjectCreationExpressionVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] {
                    typeof(GrapeObjectCreationExpression)
                };
            }
        }

        public void VisitNode(object obj) {
            GrapeObjectCreationExpression s = obj as GrapeObjectCreationExpression;
            if (s != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(s);
                }

                if (isValid) {
                    // TODO: insert object creation expression code generation here.
                }
            }
        }
    }
}
