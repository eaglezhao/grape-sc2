/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class unitgroup_base:
	static unitgroup empty = UnitGroupEmpty()

	class unit_alliance_types:
		static int any = c_unitAllianceAny
		static int ally = c_unitAllianceAlly
		static int enemy = c_unitAllianceEnemy
		static int ally_exclude_self = c_unitAllianceAllyExcludeSelf
		static int any_excluse_self = c_unitAllianceAnyExcludeSelf

	class unit_counting_types:
		static int all = c_unitCountAll
		static int alive = c_unitCountAlive
		static int dead = c_unitCountDead

	static unitgroup create(string type, player p, region r, unitfilter filter, int max):
		return UnitGroup(type, p.index, r, filter, max)

	static unitgroup create_alliance(player p, int alliance, region r, unitfilter filter, int max):
		return UnitGroupAlliance(p.index, alliance, r, filter, max)

	static unitgroup create_filter(string type, player p, unitgroup g, unitfilter filter, int max):
		return UnitGroupFilter(type, p.index, g, filter, max)

	static unitgroup get_from_id(int id):
		return UnitGroupFromId(id)

	unitgroup copy():
		return UnitGroupCopy(this)

	unitgroup filter_alliance(player p, int alliance, int max):
		return UnitGroupFilterAlliance(this, p.index, alliance, max)

	// Needs some love
	unitgroup filter_player(player p, int max):
		return UnitGroupFilterPlayer(this, p.index, max)

	unitgroup filter_plane(int plane, int max):
		return UnitGroupFilterPlane(this, plane, max)

	unitgroup filter_region(region r, int max):
		return UnitGroupFilterRegion(this, r, max)

	unitgroup filter_threat(unit u, string alternate_type, int max):
		return UnitGroupFilterThreat(this, u, alternate_type, max)

	void clear():
		UnitGroupClear(this)

	void add(unit u):
		UnitGroupAdd(this, u)

	void remove(unit u):
		UnitGroupRemove(this, u)

	bool issue_order(order o, int queue_type):
		return UnitGroupIssueOrder(this, o, queue_type)

	void wait_until_idle(int count, bool idle):
		UnitGroupWaitUntilIdle(this, count, idle)

	int get_count(int type):
		return UnitGroupCount(this, type)

	unit get_unit(int index):
		return UnitGroupUnit(this, index)

	bool contains(unit u):
		return UnitGroupHasUnit(this, u)

	bool test_plane(int plane):
		return UnitGroupTestPlane(this, plane)

	unit get_nearest_unit(point p):
		return UnitGroupNearestUnit(this, p)

	unit get_random_unit(int type):
		return UnitGroupRandomUnit(this, type)