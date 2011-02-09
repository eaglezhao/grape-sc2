using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Vestras.StarCraft2.Grape.CodeGeneration;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.Compiler {
    [Export]
    internal class Program : IPartImportsSatisfiedNotification {
        [Import]
        private IGrapeParser parser = null;
        [Import]
        private IGrapeCodeGenerator codeGenerator = null;
        [Import]
        private GrapeErrorSink errorSink = null;
        private static string[] arguments;
        private CompositionContainer container;

        private class Argument {
            public string Id { get; private set; }
            public string Value { get; private set; }

            public Argument(string id, string value) {
                if (string.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException("id");
                }

                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException("value");
                }

                Id = id;
                Value = value;
            }
        }

        private static IEnumerable<Argument> ProcessArgs(string[] args, out bool error) {
            error = false;
            List<Argument> processedArgs = new List<Argument>();
            foreach (string arg in args) {
                int firstColon = arg.IndexOf(':');
                List<string> argList = new List<string>();
                argList.Add(arg.Substring(0, firstColon));
                argList.Add(arg.Substring(firstColon + 1, arg.Length - (firstColon + 1)));
                string[] splitArg = argList.ToArray();
                if (splitArg.Length != 2) {
                    error = true;
                    break;
                }

                processedArgs.Add(new Argument(splitArg[0], splitArg[1]));
            }

            return processedArgs;
        }

        private static void Main(string[] args) {
            arguments = args;
            new Program();
        }

        public void OnImportsSatisfied() {
            const string BaseErrorMessage = "An error occurred reading command line arguments. Aborting...";
            bool hasError;
            IEnumerable<Argument> processedArgs = ProcessArgs(arguments, out hasError);
            if (hasError) {
                Console.WriteLine(BaseErrorMessage);
                return;
            }

            List<string> inputFiles = new List<string>();
            string outputFile = "";
            bool generateCode = true;
            foreach (Argument argument in processedArgs) {
                if (argument.Id == "i" || argument.Id == "input") {
                    if (!File.Exists(argument.Value)) {
                        Console.WriteLine(BaseErrorMessage + " (Unable to find specified file: " + argument.Value + ")");
                        return;
                    }

                    inputFiles.Add(argument.Value);
                } else if (argument.Id == "d" || argument.Id == "dir") {
                    if (!Directory.Exists(argument.Value)) {
                        Console.WriteLine(BaseErrorMessage + " (Unable to find specified directory: " + argument.Value + ")");
                        return;
                    }

                    foreach (FileInfo info in new DirectoryInfo(argument.Value).GetFiles("*.gp")) {
                        inputFiles.Add(info.FullName);
                    }
                } else if (argument.Id == "o" || argument.Id == "output") {
                    outputFile = argument.Value;
                } else if (argument.Id == "gc" || argument.Id == "generate") {
                    bool result;
                    if (!bool.TryParse(argument.Value, out result)) {
                        Console.WriteLine(BaseErrorMessage);
                        return;
                    }

                    generateCode = result;
                }
            }

            if (inputFiles.Count == 0) {
                Console.WriteLine("No input files specified.");
                return;
            }

            GrapeAst.ClearAllEntities();
            DateTime startCompilationTime = DateTime.Now;
            GrapeAst ast = null;
            foreach (string inputFile in inputFiles) {
                GrapeAst newAst = parser.Parse(inputFile, true, false);
                if (ast == null) {
                    ast = newAst;
                } else {
                    ast = GrapeAst.Merge(ast, newAst);
                }
            }

            codeGenerator.Generate(ast, true, false, generateCode, outputFile);
            ReadOnlyCollection<GrapeErrorSink.Error> errors = errorSink.Errors;
            Console.WriteLine(string.Format("gpc - Grape compiler{0}Compilation took {1} milliseconds.", Environment.NewLine, (DateTime.Now - startCompilationTime).Milliseconds));
            if (errors.Count == 0) {
                Console.WriteLine("The compilation was succesful." + Environment.NewLine);
            } else {
                Console.WriteLine(errors.Count + " errors were found." + Environment.NewLine);
            }

            foreach (GrapeErrorSink.Error error in errors) {
                Console.WriteLine(error.ToString());
                System.Diagnostics.Debug.Write(error.ToString() + Environment.NewLine);
            }
        }

        private Program() {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog("."));
            container = new CompositionContainer(catalog);
            try {
                container.ComposeParts(this);
            } catch (CompositionException compositionException) {
                Console.WriteLine(compositionException.ToString());
            }
        }
    }
}
