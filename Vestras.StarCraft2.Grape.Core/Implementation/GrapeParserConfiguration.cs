using System;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
    internal struct GrapeParserConfiguration {
        private GrapeAst ast;
        private bool outputErrors;
        private bool continueOnError;
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

        public GrapeParserConfiguration(GrapeAst ast, bool outputErrors, bool continueOnError) {
            this.ast = ast;
            this.outputErrors = outputErrors;
            this.continueOnError = continueOnError;
        }
    }
}
