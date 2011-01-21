using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeFieldVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type NodeType {
            get {
                return typeof(GrapeField);
            }
        }

        public void VisitNode(object obj) {
            GrapeField f = obj as GrapeField;
            if (f != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(f);
                }

                if (isValid) {
                    // TODO: insert field code generation here.
                }
            }
        }
    }
}
