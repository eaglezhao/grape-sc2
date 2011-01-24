/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 // summary: Represents a value that is either true or false.
 class bool_base
	bool and(bool b)
		return this && b
	end

	bool or(bool b)
		return this || b
	end

	bool negate()
		return !this
	end

	bool xor(bool b)
		return this ^ b
	end

	int to_int()
		return BoolToInt(this)
	end

	override int get_hash_code()
		return to_int().get_hash_code()
	end

	override bool equals(bool b)
		return this == b
	end

	override string to_string()
		if this
			return "true"
		end

		return "false"
	end
 end