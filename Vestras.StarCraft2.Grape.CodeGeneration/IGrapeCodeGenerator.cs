using System;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration {
    public interface IGrapeCodeGenerator {
        void Generate(GrapeAst ast, bool outputErrors, bool continueOnError, string outputFileName);
    }
}
