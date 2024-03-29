﻿/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class unitfilter_base:
	class filter_types:
		static int allowed = c_unitFilterAllowed
		static int required = c_unitFilterRequired
		static int excluded = c_unitFilterExcluded

	static unitfilter create(int required1, int required2, int excluded1, int excluded2):
		return UnitFilter(required1, required2, excluded1, excluded2)

	static unitfilter create(string filter_rules):
		return UnitFilterStr(filter_rules)

	void set_state(int attribute, int value):
		UnitFilterSetState(this, attribute, value)

	int get_state(int attribute):
		return UnitFilterGetState(this, attribute)

	bool match(unit u, player p):
		return UnitFilterMatch(u, p.index, this)

	override bool equals(object other):
		return this == other

	private ctor unitfilter_base():