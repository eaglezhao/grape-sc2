using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop {
    public static class GalaxyLiterals {
        [GalaxyLiteral("null", "null")]
        [GalaxyLiteralType("object")]
        public static object NullObject = null;

        [GalaxyLiteralType("int_base")]
        public static int HexadecimalLiteral = 0x00;

        [GalaxyLiteralType("fixed_base")]
        public static double FixedLiteral = 0.00;

        [GalaxyLiteralType("int_base")]
        public static int IntLiteral = 0;

        [GalaxyLiteralType("string_base")]
        public static string StringLiteral = "";
    }
}
