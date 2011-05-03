/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class actorscope_base:
	static actorscope create(string name):
		return ActorScopeCreate(name)

	static actorscope get_from_name(string name):
		return ActorScopeFrom(name)

	void kill():
		ActorScopeKill(this)

	void orphan():
		ActorScopeOrphan(this)

	actor get_child(string name):
		return ActorFromScope(this, name)

	actor get_reference(string name):
		return ActorScopeRefGet(this, name)

	void set_reference(string name, actor value):
		ActorScopeRefSet(this, name, value)

	void send(string message):
		ActorScopeSend(this, message)

	text get_debug_text():
		return ActorScopeGetText(this)

	private ctor actorscope_base():