/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class order_base:
	class order_target_types:
		static int target_none = c_orderTargetNone
		static int target_point = c_orderTargetPoint
		static int target_unit = c_orderTargetUnit
		static int target_item = c_orderTargetItem

	static order create(abilcmd cmd):
		return Order(cmd)

	static order create_targeting_point(abilcmd cmd, point p):
		return OrderTargetingPoint(cmd, p)

	static order create_targeting_relative_point(abilcmd cmd, point p):
		return OrderTargetingRelativePoint(cmd, p)

	static order create_targeting_unit(abilcmd cmd, unit u):
		return OrderTargetingUnit(cmd, u)

	static order create_targeting_unitgroup(abilcmd cmd, unitgroup grp):
		return OrderTargetingUnitGroup(cmd, grp)

	static order create_targeting_item(abilcmd cmd, unit item):
		return OrderTargetingItem(cmd, item)

	static order create_with_autocast(abilcmd cmd, bool autocast_on):
		return OrderSetAutoCast(cmd, autocast_on)

	void set_ability_command(abilcmd cmd):
		OrderSetAbilityCommand(this, cmd)

	abilcmd get_ability_command():
		return OrderGetAbilityCommand(this)

	void set_player(player p):
		OrderSetPlayer(this, p.index)

	player get_player():
		return new player(playergroup.all_players, OrderGetPlayer(this))

	int get_target_type():
		return OrderGetTargetType(this)

	bool set_target_placement(point target_point, unit placer, string type):
		return OrderSetTargetPlacement(this, target_point, placer, type)

	void set_target_point(point value):
		OrderSetTargetPoint(this, value)

	point get_target_point():
		return OrderGetTargetPoint(this)

	point get_target_position():
		return OrderGetTargetPosition(this)

	void set_target_unit(unit value):
		OrderSetTargetUnit(this, value)

	unit get_target_unit():
		return OrderGetTargetUnit(this)

	void set_target_passenger(unit value):
		OrderSetTargetPassenger(this, value)

	void set_target_item(unit value):
		OrderSetTargetItem(this, value)

	unit get_target_item():
		return OrderGetTargetItem(this)

	void set_flag(int flag, bool value):
		OrderSetFlag(this, flag, value)

	bool get_flag(int flag):
		return OrderGetFlag(this, flag)

	private ctor order_base():