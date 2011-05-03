/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class marker_base:
	static marker create(string link):
		return Marker(link)

	static marker create_with_casting_player(string link, player p):
		return MarkerCastingPlayer(link, p.index)

	static marker create_with_casting_unit(string link, unit u):
		return MarkerCastingUnit(link, u)

	void set_casting_player(player p):
		MarkerSetCastingPlayer(this, p.index)

	int get_casting_player():
		return MarkerGetCastingPlayer(this)

	void set_casting_unit(unit u):
		MarkerSetCastingUnit(this, u)

	unit get_casting_unit():
		return MarkerGetCastingUnit(this)

	void set_match_flag(player p, bool b):
		MarkerSetMatchFlag(this, p.index, b)

	bool get_match_flag(player p):
		return MarkerGetMatchFlag(this, p.index)

	void set_mismatch_flag(player p, bool b):
		MarkerSetMismatchFlag(this, p.index, b)

	bool get_mismatch_flag(player p):
		return MarkerGetMismatchFlag(this, p.index)

	private ctor marker_base():