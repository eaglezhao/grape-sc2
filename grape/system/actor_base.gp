/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class actor_base:
	static actor create(actorscope scope, string name, string content1, string content2, string content3):
		return ActorCreate(scope, name, content1, content2, content3)

	static actor get_from_name(string name):
		return ActorFrom(name)

	actorscope get_scope():
		return ActorScopeFromActor(this)

	actor get_child(string name):
		return ActorFromActor(this, name)

	text get_debug_text():
		return ActorGetText(this)

	actor get_reference(string name):
		return ActorRefGet(this, name)

	void set_reference(string name, actor value):
		ActorRefSet(this, name, value)

	void send(string message):
		ActorSend(this, message)

	void send_to(string name, string message):
		ActorSendTo(this, name, message)

	void look_at(string group, int weight, fixed time, actor target):
		ActorLookAtStart(this, group, weight, time, target)

	void look_at(string type, actor target):
		ActorLookAtTypeStart(this, type, target)

	void stop_looking(string group, int weight, fixed time):
		ActorLookAtStop(this, group, weight, time)

	void stop_looking(string type):
		ActorLookAtTypeStop(this, type)

	static void kill_all_particles():
		ActorWorldParticleFXDestroy()

	static void apply_global_texture_group(string texture_properties):
		ActorTextureGroupApplyGlobal(texture_properties)

	static void remove_global_texture_group(string texture_properties):
		ActorTextureGroupRemoveGlobal(texture_properties)

	static actor get_last_created():
		return ActorFrom("::LastCreated")

	void destory():
		send("Destroy")

	void set_facing(fixed facing):
		send("SetFacing " + facing.to_string())

	void set_position(fixed x, fixed y, fixed z):
		send("SetPosition " + x.to_string() + "," + y.to_string() + "," + z.to_string())

	void set_position_2d(fixed x, fixed y):
		send("SetPosition2D " + x.to_string() + "," + y.to_string())

	void set_position_2d_preserve_height(fixed x, fixed y):
		send("SetPosition2DH " + x.to_string() + "," + y.to_string())

	void set_position_2d_adjust_height(fixed x, fixed y, fixed height):
		send("SetPositionH " + x.to_string() + "," + y.to_string() + " " + height.to_string())

	void set_height(fixed height):
		send("SetHeight " + height.to_string())

	void set_z(fixed z):
		send("SetZ " + z.to_string())

	private ctor actor_base():