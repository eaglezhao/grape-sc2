using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeClassVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type NodeType {
            get {
                return typeof(GrapeField);
            }
        }

        public void VisitNode(object obj) {
            GrapeClass c = obj as GrapeClass;
            if (c != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(c);
                }

                if (isValid) {
                    // TODO: insert class code generation here.
                }
            }
        }
    }
}
