using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GalaxyLiteralAttribute : Attribute {
        private string name;
        private string compilationName;

        public string Name {
            get {
                return name;
            }
        }

        public string CompilationName {
            get {
                return compilationName;
            }
        }

        public GalaxyLiteralAttribute(string name, string compilationName) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(compilationName)) {
                throw new ArgumentNullException("compilationName");
            }

            this.name = name;
            this.compilationName = compilationName;
        }
    }
}
