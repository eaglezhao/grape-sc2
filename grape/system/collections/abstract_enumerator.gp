/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system.collections

// summary: Supports a simple iteration over a generic collection.
abstract class abstract_enumerator:
	// summary: Gets the current item in the abstract_enumerator.
	abstract object get_current():

	// summary: Moves to the next item in the abstract_enumerator.
	abstract bool move_next():

	// summary: Resets the abstract_enumerator.
	abstract void reset():