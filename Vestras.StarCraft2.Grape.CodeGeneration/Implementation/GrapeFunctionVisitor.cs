using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeFunctionVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeFunction) };
            }
        }

        public void VisitNode(object obj) {
            GrapeFunction f = obj as GrapeFunction;
            if (f != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(f);
                }

                if (isValid) {
                    // TODO: insert function code generation here.
                }
            }
        }
    }
}
