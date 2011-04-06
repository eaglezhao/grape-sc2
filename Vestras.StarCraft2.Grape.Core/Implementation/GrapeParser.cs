using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;

using bsn.GoldParser.Grammar;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	[Export(typeof(IGrapeParser))]
	public sealed class GrapeParser: IGrapeParser {
		private static readonly GrapeSemanticActions semanticActions = new GrapeSemanticActions();

		[Import]
		private GrapeErrorSink errorSink = null;

		public GrapeParser() {
			semanticActions.Initialize(Debugger.IsAttached); // emit semantic action diagnostics to debug log when debugging
		}

		public GrapeAst Parse(string file, bool outputErrors, bool continueOnError) {
			return Parse(new[] {file}, outputErrors, continueOnError);
		}

		public GrapeAst Parse(string[] files, bool outputErrors, bool continueOnError) {
			GrapeParserConfiguration configuration = new GrapeParserConfiguration(errorSink, outputErrors, continueOnError);
			GrapeAst ast = new GrapeAst();
			foreach (string file in files) {
				configuration.FileName = file;
				using (StreamReader reader = new StreamReader(file)) {
					GrapeProcessor processor = new GrapeProcessor(reader, configuration, semanticActions);
					if (processor.ParseAll() == ParseMessage.Accept) {
						foreach (GrapeDeclaration declaration in ((GrapeList<GrapeDeclaration>)processor.CurrentToken).Enumerate()) {
							declaration.Parent = null;
							ast.Children.Add(declaration);
						}
					}
				}
			}
			return ast;
		}
	}
}
