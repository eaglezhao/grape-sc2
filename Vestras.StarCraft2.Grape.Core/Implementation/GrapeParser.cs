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
        private Dictionary<string, GrapeFunction> functionOverloads = new Dictionary<string, GrapeFunction>();
        private Dictionary<GrapeFunction, List<GrapeEntity>> functionOverloadsToRemove = new Dictionary<GrapeFunction, List<GrapeEntity>>();

        public GrapeAst Parse(string file, bool outputErrors, bool continueOnError) {
            return Parse(new string[] { file }, outputErrors, continueOnError);
        }

        private void FixFunctionOverloads(List<GrapeEntity> entities) {
            foreach (GrapeEntity entity in entities) {
                GrapeFunction function = entity as GrapeFunction;
                if (function != null && !string.IsNullOrEmpty(function.Name)) {
                    if (functionOverloads.ContainsKey(function.Name) && functionOverloads[function.Name].Type == function.Type) {
                        functionOverloads[function.Name].Overloads.Add(function);
                        functionOverloadsToRemove.Add(function, entities);
                    } else if (!functionOverloads.ContainsKey(function.Name)) {
                        functionOverloads.Add(function.Name, function);
                    }
                }

                if (entity is GrapeEntityWithBlock && ((GrapeEntityWithBlock)entity).Block != null) {
                    FixFunctionOverloads(((GrapeEntityWithBlock)entity).Block.Children);
                } else if (entity is GrapePackageDeclaration) {
                    FixFunctionOverloads(((GrapePackageDeclaration)entity).Children);
                }
            }
        }

        private void RemoveFunctionOverloads() {
            foreach (KeyValuePair<GrapeFunction, List<GrapeEntity>> pair in functionOverloadsToRemove) {
                if (pair.Value.Contains(pair.Key)) {
                    pair.Value.Remove(pair.Key);
                }
            }
        }

        private void FixAbstractSyntaxTree(GrapeAst ast) {
            functionOverloads.Clear();
            functionOverloadsToRemove.Clear();
            FixFunctionOverloads(ast.Children);
            RemoveFunctionOverloads();
        }

        public GrapeAst Parse(string[] files, bool outputErrors, bool continueOnError) {
            GrapeAst ast = new GrapeAst();
            GrapeParserConfiguration config = new GrapeParserConfiguration(ast, outputErrors, continueOnError);
            Stream stream = GetType().Assembly.GetManifestResourceStream("Vestras.StarCraft2.Grape.Core.Implementation.grape.cgt");
            GrapeSkeletonParser skeletonParser = new GrapeSkeletonParser(stream, config);
            skeletonParser.errorSink = errorSink;
            foreach (string file in files) {
                using (StreamReader reader = new StreamReader(file)) {
                    skeletonParser.Parse(reader.ReadToEnd());
                }
            }

            FixAbstractSyntaxTree(ast);
            return ast;
        }
    }
}
