using System;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal interface IAstNodeVisitor {
        Type[] NodeType { get; }
        GrapeCodeGeneratorConfiguration Config { get; set; }
        IAstNodeValidator Validator { get; set; }

        void VisitNode(object obj);
    }
}
