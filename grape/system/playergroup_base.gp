/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class playergroup_base:
	static playergroup all_players = PlayerGroupAll()
	static playergroup active_players = PlayerGroupActive()
	static playergroup empty = PlayerGroupEmpty()

	playergroup copy():
		return PlayerGroupCopy(this)

	void clear():
		PlayerGroupClear(this)

	int get_count():
		return PlayerGroupCount(this)

	player get_player(int index):
		return new player(this, index)

	void add(player p):
		PlayerGroupAdd(this, p.index)

	void add_range(playergroup group):
		int index = 1
		while index <= group.get_count():
			add(group.get_player(index))
			index = index + 1

	void remove(player p):
		PlayerGroupRemove(this, p.index)

	void remove_range(playergroup group):
		int index = 1
		while index <= group.get_count():
			remove(group.get_player(index))
			index = index + 1

	bool contains_player(player p):
		return PlayerGroupHasPlayer(this, p.index)

	void set_volume(sound_channel channel, fixed volume, fixed duration):
		SoundChannelSetVolume(this, channel.index, volume, duration)

	void set_muted(sound_channel channel, bool muted):
		SoundChannelMute(this, channel.index, muted)

	void set_paused(sound_channel channel, bool paused):
		SoundChannelPause(this, channel.index, paused)

	void stop_sounds(sound_channel channel):
		SoundChannelStop(this, channel.index)

	void set_achievement_panel_visible(bool value):
		AchievementPanelSetVisible(this, value)

	private ctor playergroup_base():