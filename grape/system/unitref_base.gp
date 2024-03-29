﻿/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class unitref_base:
	static unitref create(unit u):
		return UnitRefFromUnit(u)

	static unitref create(string v):
		return UnitRefFromVariable(v)

	unit to_unit():
		return UnitRefToUnit(this)

	override bool equals(object other):
		return this == other

	override int get_hash_code():
		return to_unit().get_hash_code()

	override string to_string():
		return "unitref { to_unit() = " + to_unit().to_string() + " }"

	private ctor unitref_base():