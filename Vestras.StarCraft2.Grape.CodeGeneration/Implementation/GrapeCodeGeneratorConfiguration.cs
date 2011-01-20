using System;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal struct GrapeCodeGeneratorConfiguration {
        private GrapeAst ast;
        private bool outputErrors;
        private bool continueOnError;
        private string outputCode;
        public GrapeAst Ast {
            get {
                return ast;
            }
        }

        public bool OutputErrors {
            get {
                return outputErrors;
            }
        }

        public bool ContinueOnError {
            get {
                return continueOnError;
            }
        }

        public string OutputCode {
            get {
                return outputCode;
            }
            set {
                outputCode = value;
            }
        }

        public GrapeCodeGeneratorConfiguration(GrapeAst ast, bool outputErrors, bool continueOnError) {
            this.ast = ast;
            this.outputErrors = outputErrors;
            this.continueOnError = continueOnError;
            this.outputCode = "";
        }
    }
}
