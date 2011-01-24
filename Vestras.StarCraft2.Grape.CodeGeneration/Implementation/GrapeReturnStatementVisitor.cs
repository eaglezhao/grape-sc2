using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeReturnStatementVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeReturnStatement) };
            }
        }

        public void VisitNode(object obj) {
            GrapeReturnStatement s = obj as GrapeReturnStatement;
            if (s != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(s);
                }

                if (isValid) {
                    // TODO: insert return statement code generation here.
                }
            }
        }
    }
}
