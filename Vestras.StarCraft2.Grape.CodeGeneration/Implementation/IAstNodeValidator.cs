using System;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal interface IAstNodeValidator {
        Type[] NodeType { get; }
        GrapeCodeGeneratorConfiguration Config { get; set; }

        bool ValidateNode(object obj);
    }
}
