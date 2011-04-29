/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class doodad_base
	static doodad get_from_id(int id)
		return DoodadFromId(id)
	end

	actor get_actor()
		return ActorFromDoodad(this)
	end

	private ctor doodad_base()
	end
 end