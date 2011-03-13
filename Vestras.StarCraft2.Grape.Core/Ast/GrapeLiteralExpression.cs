using System;
using System.Globalization;

namespace Vestras.StarCraft2.Grape.Core.Ast {
	public abstract class GrapeLiteralExpression: GrapeExpression {
		public enum GrapeLiteralExpressionType {
			Hexadecimal,
			Int,
			Real,
			String,
			True,
			False,
			Null
		}

		private readonly string valueAsString;

		protected GrapeLiteralExpression(string valueAsString) {
			this.valueAsString = valueAsString;
		}

		public abstract GrapeLiteralExpressionType Type {
			get;
		}

		public string ValueAsString {
			get {
				return valueAsString;
			}
		}
	}

	public abstract class GrapeLiteralExpression<T>: GrapeLiteralExpression where T: IConvertible {
		private readonly T value;

		protected GrapeLiteralExpression(string value): base(value) {
			this.value = (T)((IConvertible)ValueAsString).ToType(typeof(T), CultureInfo.InvariantCulture);
		}

		public T Value {
			get {
				return value;
			}
		}
	}
}
