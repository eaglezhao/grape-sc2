/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system.collections

// summary: Represents an enumerable collection of items.
abstract class abstract_enumerable
	// summary: Gets the abstract_enumerator of this abstract_enumerable.
	abstract abstract_enumerator get_enumerator()
	end
end