using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class GalaxyTypeAttribute : Attribute {
        private string typeName;
        private string nativeAlias;

        public string TypeName {
            get {
                return typeName;
            }
        }

        public string NativeAlias {
            get {
                return nativeAlias;
            }
        }

        public GalaxyTypeAttribute(string typeName, string nativeAlias) {
            if (string.IsNullOrEmpty(nativeAlias)) {
                throw new ArgumentNullException("nativeAlias");
            }

            if (string.IsNullOrEmpty(typeName)) {
                throw new ArgumentNullException("typeName");
            }

            this.typeName = typeName;
            this.nativeAlias = nativeAlias;
        }
    }
}
