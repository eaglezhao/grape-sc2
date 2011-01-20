using System;

namespace Vestras.StarCraft2.Grape.Core {
    public interface IGrapeParser {
        GrapeAst Parse(string file, bool outputErrors, bool continueOnError);
        GrapeAst Parse(string[] files, bool outputErrors, bool continueOnError);
    }
}
