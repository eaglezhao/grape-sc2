using System.Globalization;

using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	[Terminal("DecimalLiteral")]
	internal class GrapeIntLiteralExpression: GrapeLiteralExpression<int> {
        public GrapeIntLiteralExpression(string value) : this(value, int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture)) {}
        protected GrapeIntLiteralExpression(string stringValue, int parsedValue) : base(stringValue, parsedValue) {}

		public override GrapeLiteralExpressionType Type {
			get {
				return GrapeLiteralExpressionType.Int;
			}
		}
	}
}
