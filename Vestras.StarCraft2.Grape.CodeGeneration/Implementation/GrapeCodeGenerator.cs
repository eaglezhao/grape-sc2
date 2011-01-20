using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Galaxy.Interop;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export(typeof(IGrapeCodeGenerator))]
    internal sealed class GrapeCodeGenerator : IGrapeCodeGenerator {
        [Import]
        private GrapeAstVisitor astVisitor = null;

        internal static List<GalaxyConstantAttribute> Constants { get; set; }
        internal static List<GalaxyFunctionAttribute> Functions { get; set; }
        internal static List<Tuple<GalaxyLiteralAttribute, GalaxyLiteralTypeAttribute>> Literals { get; set; }
        internal static List<Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute>> Types { get; set; }

        public void Generate(GrapeAst ast, bool outputErrors, bool continueOnError, string outputFileName) {
            GrapeCodeGeneratorConfiguration config = new GrapeCodeGeneratorConfiguration(ast, outputErrors, continueOnError);
            astVisitor.VisitNodes(config);
            using (StreamWriter writer = new StreamWriter(outputFileName)) {
                writer.Write(config.OutputCode);
            }
        }

        static GrapeCodeGenerator() {
            Constants = GalaxyNativeInterfaceAggregator.Constants;
            Functions = GalaxyNativeInterfaceAggregator.Functions;
            Literals = GalaxyNativeInterfaceAggregator.Literals;
            Types = GalaxyNativeInterfaceAggregator.Types;
        }
    }
}
