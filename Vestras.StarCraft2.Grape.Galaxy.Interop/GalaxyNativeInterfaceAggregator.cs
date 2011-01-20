using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    public static class GalaxyNativeInterfaceAggregator {
        public static event EventHandler Loaded;

        private static Assembly assembly = typeof(GalaxyNativeInterfaceAggregator).Assembly;
        private static Type[] types = assembly.GetTypes();

        public static List<GalaxyConstantAttribute> Constants { get; private set; }
        public static List<GalaxyFunctionAttribute> Functions { get; private set; }
        public static List<Tuple<GalaxyLiteralAttribute, GalaxyLiteralTypeAttribute>> Literals { get; private set; }
        public static List<Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute>> Types { get; private set; }

        private static T GetAttribute<T>(this Type type) {
            object[] attributes = type.GetCustomAttributes(false);
            foreach (object attribute in attributes) {
                if (attribute is T) {
                    return (T)attribute;
                }
            }

            return default(T);
        }

        private static T GetAttribute<T>(this FieldInfo field) {
            object[] attributes = field.GetCustomAttributes(false);
            foreach (object attribute in attributes) {
                if (attribute is T) {
                    return (T)attribute;
                }
            }

            return default(T);
        }

        private static bool HasAttributeApplied<T>(this Type type) {
            return type.GetAttribute<T>() != null;
        }

        private static bool HasAttributeApplied<T>(this FieldInfo field) {
            return field.GetAttribute<T>() != null;
        }

        private static void LoadConstants() {
            foreach (Type type in types) {
                if (type.HasAttributeApplied<GalaxyConstantAttribute>()) {
                    Constants.Add(type.GetAttribute<GalaxyConstantAttribute>());
                }
            }
        }

        private static void LoadFunctions() {
            foreach (Type type in types) {
                if (type.HasAttributeApplied<GalaxyFunctionAttribute>()) {
                    Functions.Add(type.GetAttribute<GalaxyFunctionAttribute>());
                }
            }
        }

        private static void LoadLiterals() {
            foreach (Type type in types) {
                foreach (FieldInfo field in type.GetFields()) {
                    if (field.HasAttributeApplied<GalaxyLiteralTypeAttribute>()) {
                        GalaxyLiteralTypeAttribute literalTypeAttribute = field.GetAttribute<GalaxyLiteralTypeAttribute>();
                        GalaxyLiteralAttribute literalAttribute = null;
                        if (field.HasAttributeApplied<GalaxyLiteralAttribute>()) {
                            literalAttribute = field.GetAttribute<GalaxyLiteralAttribute>();
                        }

                        Tuple<GalaxyLiteralAttribute, GalaxyLiteralTypeAttribute> tuple = new Tuple<GalaxyLiteralAttribute, GalaxyLiteralTypeAttribute>(literalAttribute, literalTypeAttribute);
                        Literals.Add(tuple);
                    }
                }
            }
        }

        private static void LoadTypes() {
            foreach (Type type in types) {
                if (type.HasAttributeApplied<GalaxyTypeAttribute>()) {
                    GalaxyTypeAttribute typeAttribute = type.GetAttribute<GalaxyTypeAttribute>();
                    GalaxyTypeDefaultValueAttribute typeDefaultValueAttribute = null;
                    if (type.HasAttributeApplied<GalaxyTypeDefaultValueAttribute>()) {
                        typeDefaultValueAttribute = type.GetAttribute<GalaxyTypeDefaultValueAttribute>();
                    }

                    Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute> tuple = new Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute>(typeAttribute, typeDefaultValueAttribute);
                    Types.Add(tuple);
                }
            }
        }

        static GalaxyNativeInterfaceAggregator() {
            Constants = new List<GalaxyConstantAttribute>();
            Functions = new List<GalaxyFunctionAttribute>();
            Literals = new List<Tuple<GalaxyLiteralAttribute, GalaxyLiteralTypeAttribute>>();
            Types = new List<Tuple<GalaxyTypeAttribute, GalaxyTypeDefaultValueAttribute>>();
            LoadConstants();
            LoadFunctions();
            LoadLiterals();
            LoadTypes();
            if (Loaded != null) {
                Loaded(null, EventArgs.Empty);
            }
        }
    }
}
