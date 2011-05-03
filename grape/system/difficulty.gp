/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class difficulty:
	static difficulty very_easy = new difficulty(0)
	static difficulty easy      = new difficulty(1)
	static difficulty medium    = new difficulty(2)
	static difficulty hard      = new difficulty(3)
	static difficulty very_hard = new difficulty(4)

	int index

	text get_name():
		return DifficultyName(index)

	text get_campaign_name():
		return DifficultyNameCampaign(index)

	bool is_enabled():
		return DifficultyEnabled(index)

	int get_apm():
		return DifficultyAPM(index)

	ctor difficulty(int index):
		this.index = index