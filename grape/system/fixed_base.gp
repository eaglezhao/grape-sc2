/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents a decimal value.
class fixed_base:
	static int any_precision = c_fixedPrecisionAny

	fixed_base abs():
		return AbsF(this)

	fixed_base pow(fixed_base exponent):
		return Pow(this, exponent)

	fixed_base max(fixed_base value):
		return MaxF(this, value)

	fixed_base min(fixed_base value):
		return MinF(this, value)

	fixed_base sqrt():
		return SquareRoot(this)

	fixed_base mod(fixed_base divisor):
		return ModF(this, divisor)

	fixed_base sin():
		return Sin(this)

	fixed_base cos():
		return Cos(this)

	fixed_base tan():
		return Tan(this)

	fixed_base asin():
		return ASin(this)

	fixed_base acos():
		return ACos(this)

	fixed_base atan():
		return ATan(this)

	static fixed_base random(fixed_base min, fixed_base max):
		return RandomFixed(min, max)

	override bool equals(object value):
		fixed f = value as fixed
		return this == f

	override int get_hash_code():
		return this as int

	override string to_string():
		return FixedToString(this, any_precision)

	string to_string(int_base precision):
		return FixedToString(this, precision)

	private ctor fixed_base():