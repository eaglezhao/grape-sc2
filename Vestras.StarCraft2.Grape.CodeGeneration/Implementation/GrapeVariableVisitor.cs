using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor)), Export]
    internal sealed class GrapeVariableVisitor : IAstNodeVisitor {

        [Import]
        private GrapeExpressionGenerator expressionGenerator = null;
        [Import]
        private GrapeTypeCheckingUtilities typeCheckingUtils = null;


        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeVariable) };
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
                    expressionGenerator.Config = Config;

                    //string type = "[type]";
                    string type = typeCheckingUtils.GetTypeNameForTypeAccessExpression(Config, v.Type);
                    //type.Replace("_base", "");

                    string output = type + " " + v.Name + "=" + expressionGenerator.VisitExpression(v.Value) + ";";

                    Config.OutputCode += output + Environment.NewLine;
                    // TODO: insert field code generation here.
                }
            }
        }
    }
}
