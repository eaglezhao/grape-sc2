/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents a value that is either true or false.
class bool_base:
	bool and(bool b):
		return this && b

	bool or(bool b):
		return this || b

	bool negate():
		return !this

	bool xor(bool b):
		return this ^ b

	int to_int():
		return BoolToInt(this)

	override int get_hash_code():
		return to_int().get_hash_code()

	override bool equals(bool b):
		return this == b

	override string to_string():
		if this:
			return "true"

		return "false"

	private ctor bool_base():