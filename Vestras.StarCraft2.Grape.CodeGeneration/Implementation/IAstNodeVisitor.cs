using System;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    public interface IAstNodeVisitor {
        Type NodeType { get; }
        GrapeCodeGeneratorConfiguration Config { get; set; }

        void VisitNode(object obj);
    }
}
