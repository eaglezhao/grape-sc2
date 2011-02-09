/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package systems

import system

// summary: Represents a base class for mods. Provides mod initialization methods along with other mod utilities.
abstract class mod_base
	abstract void main()
	end

	ctor mod_base()
	end

	ctor mod_base(string args)
	end
end