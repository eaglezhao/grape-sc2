using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class GalaxyTypeDefaultValueAttribute : Attribute {
        private string defaultValue;

        public string DefaultValue {
            get {
                return defaultValue;
            }
        }

        public GalaxyTypeDefaultValueAttribute(string defaultValue) {
            if (string.IsNullOrEmpty(defaultValue)) {
                throw new ArgumentNullException("defaultValue");
            }

            this.defaultValue = defaultValue;
        }
    }
}
