using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeValidator))]
    internal class GrapePassStatementValidator : IAstNodeValidator {
        [Import]
        private GrapeErrorSink errorSink = null;

        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapePassStatement) };
            }
        }

        public bool ValidateNode(object obj) {
            if (Config.OutputErrors) {
                GrapePassStatement s = obj as GrapePassStatement;
                if (s != null) {
                    GrapeMethod m = s.Parent as GrapeMethod;
                    if (m == null) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A pass statement must be the direct child of a method.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    IList<GrapeEntity> list = new List<GrapeEntity>(m.GetChildren());
                    if (list.Count > 1) {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A method which has a pass statement as child must not have any other children.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }

                    string typeQualifiedId = m.ReturnType.ToString();
                    if (typeQualifiedId != "void_base" && typeQualifiedId != "void") {
                        errorSink.AddError(new GrapeErrorSink.Error { Description = "A method which has a pass statement as child must not return a value.", FileName = s.FileName, Entity = s });
                        if (!Config.ContinueOnError) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
