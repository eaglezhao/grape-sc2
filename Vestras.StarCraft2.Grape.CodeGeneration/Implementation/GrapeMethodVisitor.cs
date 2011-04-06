using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeMethodVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeMethod) };
            }
        }

        public void VisitNode(object obj) {
            GrapeMethod m = obj as GrapeMethod;
            if (m != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(m);
                }

                if (isValid) {
                    // TODO: insert function code generation here.
                }
            }
        }
    }
}
