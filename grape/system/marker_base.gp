/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class marker_base
	static marker create(string link)
		return Marker(link)
	end

	static marker create_with_casting_player(string link, player p)
		return MarkerCastingPlayer(link, p.index)
	end

	static marker create_with_casting_unit(string link, unit u)
		return MarkerCastingUnit(link, u)
	end

	void set_casting_player(player p)
		MarkerSetCastingPlayer(this, p.index)
	end

	int get_casting_player()
		return MarkerGetCastingPlayer(this)
	end

	void set_casting_unit(unit u)
		MarkerSetCastingUnit(this, u)
	end

	unit get_casting_unit()
		return MarkerGetCastingUnit(this)
	end

	void set_match_flag(player p, bool b)
		MarkerSetMatchFlag(this, p.index, b)
	end

	bool get_match_flag(player p)
		return MarkerGetMatchFlag(this, p.index)
	end

	void set_mismatch_flag(player p, bool b)
		MarkerSetMismatchFlag(this, p.index, b)
	end

	bool get_mismatch_flag(player p)
		return MarkerGetMismatchFlag(this, p.index)
	end

	private ctor marker_base()
	end
 end