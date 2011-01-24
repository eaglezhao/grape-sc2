using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeBreakStatementVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeBreakStatement) };
            }
        }

        public void VisitNode(object obj) {
            GrapeBreakStatement s = obj as GrapeBreakStatement;
            if (s != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(s);
                }

                if (isValid) {
                    // TODO: insert break statement code generation here.
                }
            }
        }
    }
}
