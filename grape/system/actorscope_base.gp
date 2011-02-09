/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class actorscope_base
	static actorscope create(string name)
		return ActorScopeCreate(name)
	end

	static actorscope get_from_name(string name)
		return ActorScopeFrom(name)
	end
	
	void kill()
		ActorScopeKill(this)
	end

	void orphan()
		ActorScopeOrphan(this)
	end

	actor get_child(string name)
		return ActorFromScope(this, name)
	end

	actor get_reference(string name)
		return ActorScopeRefGet(this, name)
	end

	void set_reference(string name, actor value)
		ActorScopeRefSet(this, name, value)
	end

	void send(string message)
		ActorScopeSend(this, message)
	end

	text get_debug_text()
		return ActorScopeGetText(this)
	end

	private ctor actorscope_base()
	end
 end