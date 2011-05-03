/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

static class game:
	static class game_speeds:
		static int slower = c_gameSpeedSlower
		static int slow = c_gameSpeedSlow
		static int normal = c_gameSpeedNormal
		static int fast = c_gameSpeedFast
		static int faster = c_gameSpeedFaster

	static class game_cheats:
		static int any = c_gameCheatAny
		static int cooldown = c_gameCheatCooldown
		static int defeat = c_gameCheatDefeat
		static int fast_build = c_gameCheatFastBuild
		static int fast_heal = c_gameCheatFastHeal
		static int food = c_gameCheatFood
		static int free = c_gameCheatFree
		static int gimme = c_gameCheatGimme
		static int god = c_gameCheatGod
		static int minerals = c_gameCheatMinerals
		static int no_defeat = c_gameCheatNoDefeat
		static int no_victory = c_gameCheatNoVictory
		static int resource_custom = c_gameCheatResourceCustom
		static int show_map = c_gameCheatShowmap
		static int tech_tree = c_gameCheatTechTree
		static int terrazine = c_gameCheatTerrazine
		static int time_of_day = c_gameCheatTimeOfDay
		static int upgrade = c_gameCheatUpgrade
		static int vespene = c_gameCheatVespene
		static int victory = c_gameCheatVictory
		static int progress = c_gameCheatProgress
		static int scene = c_gameCheatScene
		static int tv = c_gameCheatTV
		static int credits = c_gameCheatCredits
		static int research = c_gameCheatResearch

	static class game_cheat_categories:
		static int category_public = c_gameCheatCategoryPublic
		static int category_development = c_gameCheatCategoryDevelopment

	static class game_over_types:
		static int victory = c_gameOverVictory
		static int defeat = c_gameOverDefeat
		static int tie = c_gameOverTie

	// ==========================================================
	// Utils
	// ==========================================================

	static void display(string text):
		UIDisplayMessage(playergroup.all_players, c_messageAreaAll, StringToText(text))

	// ==========================================================
	// Generic
	// ==========================================================

	static text map_name():
		return GameMapName()

	static text map_description():
		return GameMapDescription()

	static bool map_is_blizzard():
		return GameMapIsBlizzard()

	static void set_mission_time_paused(bool paused):
		GameSetMissionTimePaused(paused)

	static bool get_is_mission_time_paused():
		return GameIsMissionTimePaused()

	static fixed get_mission_time():
		return GameGetMissionTime()

	// ==========================================================
	// Game speed
	// ==========================================================

	static fixed get_speed():
		return GameGetSpeed()

	static void set_speed_value(int speed):
		GameSetSpeedValue(speed)

	static int get_speed_value():
		return GameGetSpeedValue()

	static void set_speed_value_minimum(int speed):
		GameSetSpeedValueMinimum(speed)

	static int get_speed_value_minimum():
		return GameGetSpeedValueMinimum()

	static void set_speed_locked(bool locked):
		GameSetSpeedLocked(locked)

	static bool get_is_speed_locked():
		return GameIsSpeedLocked()

	// ==========================================================
	// Game manipulating methods (restart, game over)
	// ==========================================================

	void game_over(player p, int type, bool show_dialog, bool show_score):
		GameOver(p.index, type, show_dialog, show_score)

	void restart_game(playergroup players):
		RestartGame(players)