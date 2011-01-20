using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class GalaxyLiteralTypeAttribute : Attribute {
        private string type;

        public string Type {
            get {
                return type;
            }
        }

        public GalaxyLiteralTypeAttribute(string type) {
            if (string.IsNullOrEmpty(type)) {
                throw new ArgumentNullException("type");
            }

            this.type = type;
        }
    }
}
