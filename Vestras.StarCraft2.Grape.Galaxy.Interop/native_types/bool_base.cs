using System;

namespace Vestras.StarCraft2.Grape.Galaxy.Interop.NativeTypes {
    [GalaxyType("bool_base", "bool")]
    [GalaxyTypeDefaultValue("false")]
    internal class BoolBase {
        [GalaxyLiteral("true", "true")]
        [GalaxyLiteralType("bool_base")]
        public const bool TrueValue = true;

        [GalaxyLiteral("false", "false")]
        [GalaxyLiteralType("bool_base")]
        public const bool FalseValue = false;
    }
}
