using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class GalaxyConstantAttribute : Attribute {
        private string type;
        private string name;

        public string Type {
            get {
                return type;
            }
        }

        public string Name {
            get {
                return name;
            }
        }

        public GalaxyConstantAttribute(string type, string name) {
            if (string.IsNullOrEmpty(type)) {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            this.type = type;
            this.name = name;
        }
    }
}
