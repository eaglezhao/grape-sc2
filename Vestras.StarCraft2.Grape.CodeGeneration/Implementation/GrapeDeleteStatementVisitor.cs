﻿using System;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IAstNodeVisitor))]
    internal sealed class GrapeDeleteStatementVisitor : IAstNodeVisitor {
        public GrapeCodeGeneratorConfiguration Config { get; set; }
        public IAstNodeValidator Validator { get; set; }
        public Type[] NodeType {
            get {
                return new Type[] { typeof(GrapeDeleteStatement) };
            }
        }

        public void VisitNode(object obj) {
            GrapeDeleteStatement s = obj as GrapeDeleteStatement;
            if (s != null) {
                bool isValid = true;
                if (Validator != null) {
                    isValid = Validator.ValidateNode(s);
                }

                if (isValid) {
                    // TODO: insert continue statement code generation here.
                }
            }
        }
    }
}
