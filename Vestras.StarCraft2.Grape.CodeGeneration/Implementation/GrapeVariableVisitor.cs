using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeVariableVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type NodeType {
            get {
                return typeof(GrapeVariable);
            }
        }

        public void VisitNode(object obj) {
            GrapeVariable v = obj as GrapeVariable;
            if (v != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(v);
                }

                if (isValid) {
                    // TODO: insert field code generation here.
                }
            }
        }
    }
}
