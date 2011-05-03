/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents a 32-bit integer value.
class int_base:
	static int_base minimum_int_value = 1 << 32
	static int_base maximum_int_value = ~minimum_int_value

	int_base abs():
		return AbsI(this)

	int_base pow(int_base exponent):
		fixed_base f_base = IntToFixed(this)
		fixed_base f_exp = IntToFixed(exponent)
		fixed_base result = Pow(f_base, f_exp)
		return FixedToInt(result)

	int_base max(int_base value):
		return MaxI(this, value)

	int_base min(int_base value):
		return MinI(this, value)

	bool isOdd():
		return ((this % 2) != 0)

	bool isEven():
		return ((this % 2) == 0)

	static int_base random(int_base min, int_base max):
		return RandomInt(min, max)

	static int_base random():
		return RandomInt(minimum_int_value, maximum_int_value)

	override bool equals(object value):
		int i = value as int
		return this == i

	override int_base get_hash_code():
		return this

	override string to_string():
		return this as string

	private ctor int_base():