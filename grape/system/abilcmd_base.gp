/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class abilcmd_base:
	string get_ability():
		return AbilityCommandGetAbility(this)

	int get_command():
		return AbilityCommandGetCommand(this)

	int get_action():
		return AbilityCommandGetAction(this)

	private ctor abilcmd_base():