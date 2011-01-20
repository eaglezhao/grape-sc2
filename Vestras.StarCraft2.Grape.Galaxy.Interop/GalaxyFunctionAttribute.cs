using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class GalaxyFunctionAttribute : Attribute {
        private string type;
        private string name;
        private string @params;
        
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

        public string Params {
            get {
                return @params;
            }
        }

        public GalaxyFunctionAttribute(string type, string name, string @params) {
            if (string.IsNullOrEmpty(type)) {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentNullException("name");
            }

            if (@params == null) {
                throw new ArgumentNullException("@params");
            }

            this.type = type;
            this.name = name;
            this.@params = @params;
        }
    }
}
