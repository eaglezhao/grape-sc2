using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
    [Export(typeof(IGrapeParser))]
    internal sealed class GrapeParser : IGrapeParser {
        [Import]
        private GrapeErrorSink errorSink = null;
        public GrapeAst Parse(string file, bool outputErrors, bool continueOnError) {
            return Parse(new string[] { file }, outputErrors, continueOnError);
        }

        public GrapeAst Parse(string[] files, bool outputErrors, bool continueOnError) {
            GrapeAst ast = new GrapeAst();
            GrapeParserConfiguration config = new GrapeParserConfiguration(ast, outputErrors, continueOnError);
            Stream stream = GetType().Assembly.GetManifestResourceStream("Vestras.StarCraft2.Grape.Core.Implementation.grape.cgt");
            GrapeSkeletonParser skeletonParser = new GrapeSkeletonParser(stream, config);
            skeletonParser.errorSink = errorSink;
            foreach (string file in files) {
                using (StreamReader reader = new StreamReader(file)) {
                    skeletonParser.currentFileName = file;
                    skeletonParser.Parse(reader.ReadToEnd());
                }
            }

            return ast;
        }
    }
}
