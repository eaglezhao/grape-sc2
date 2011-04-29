/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents the base class for all classes in grape - it is the ultimate grape base class.
class object
	// summary: Gets this object's unique hash code.
	int get_hash_code()
		return 0
	end

	// summary: Returns a value indicating whether this object instance is equal to the given object.
	bool equals(object other)
		return other == this
	end

	// summary: Returns a string that represents this object instance.
	string to_string()
		return "object"
	end
end