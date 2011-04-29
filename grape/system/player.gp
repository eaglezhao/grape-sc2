/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class player
	static player any = new player(playergroup.all_players, -1)
	static int max_players = c_maxPlayers

	playergroup owner_group
	int index

	int get_credits()
		return PlayerGetPropertyInt(index, c_playerPropCredits)
	end

	void set_credits(int value)
		PlayerModifyPropertyInt(index, c_playerPropCredits, c_playerPropOperSetTo, value)
	end

	int get_credits_spent()
		return PlayerGetPropertyInt(index, c_playerPropCreditsSpent)
	end

	int get_current_custom_resource()
		return PlayerGetPropertyInt(index, c_playerPropCustom)
	end

	void set_current_custom_resource(int value)
		PlayerModifyPropertyInt(index, c_playerPropCustom, c_playerPropOperSetTo, value)
	end

	int get_current_custom_resouce_collected()
		return PlayerGetPropertyInt(index, c_playerPropCustomCollected)
	end

	int get_handicap()
		return PlayerGetPropertyInt(index, c_playerPropHandicap)
	end

	void set_handicap(int value)
		PlayerModifyPropertyInt(index, c_playerPropHandicap, c_playerPropOperSetTo, value)
	end

	int get_minerals()
		return PlayerGetPropertyInt(index, c_playerPropMinerals)
	end

	void set_minerals(int value)
		PlayerModifyPropertyInt(index, c_playerPropMinerals, c_playerPropOperSetTo, value)
	end

	int get_minerals_collected()
		return PlayerGetPropertyInt(index, c_playerPropMineralsCollected)
	end

	int get_research_points()
		return PlayerGetPropertyInt(index, c_playerPropResearchPoints)
	end

	void set_research_points(int value)
		PlayerModifyPropertyInt(index, c_playerPropResearchPoints, c_playerPropOperSetTo, value)
	end

	int get_research_points_spent()
		return PlayerGetPropertyInt(index, c_playerPropResearchPointsSpent)
	end

	int get_supply_limit()
		return PlayerGetPropertyInt(index, c_playerPropSuppliesLimit)
	end

	void set_supply_limit(int value)
		PlayerModifyPropertyInt(index, c_playerPropSuppliesLimit, c_playerPropOperSetTo, value)
	end

	int get_supply_made()
		return PlayerGetPropertyInt(index, c_playerPropSuppliesMade)
	end

	int get_supply_used()
		return PlayerGetPropertyInt(index, c_playerPropSuppliesUsed)
	end

	int get_terrazine()
		return PlayerGetPropertyInt(index, c_playerPropTerrazine)
	end

	void set_terrazine(int value)
		PlayerModifyPropertyInt(index, c_playerPropTerrazine, c_playerPropOperSetTo, value)
	end

	int get_terrazine_collected()
		return PlayerGetPropertyInt(index, c_playerPropTerrazineCollected)
	end

	int get_vespene()
		return PlayerGetPropertyInt(index, c_playerPropVespene)
	end

	void set_vespene(int value)
		PlayerModifyPropertyInt(index, c_playerPropVespene, c_playerPropOperSetTo, value)
	end

	int get_vespene_collected()
		return PlayerGetPropertyInt(index, c_playerPropVespeneCollected)
	end

	bool is_unused()
		return PlayerStatus(index) == c_playerStatusUnused
	end

	bool is_absent()
		return PlayerStatus(index) == c_playerStatusLeft
	end

	bool is_active()
		return PlayerStatus(index) == c_playerStatusActive
	end

	bool is_ai_controlled()
		return PlayerType(index) == c_playerTypeComputer
	end

	bool is_hostile_controlled()
		return PlayerType(index) == c_playerTypeHostile
	end

	bool is_neutral_controlled()
		return PlayerType(index) == c_playerTypeNeutral
	end

	bool is_uncontrolled()
		return PlayerType(index) == c_playerTypeNone
	end

	bool is_referee_controlled()
		return PlayerType(index) == c_playerTypeReferee
	end

	bool is_spectator_controlled()
		return PlayerType(index) == c_playerTypeSpectator
	end

	bool is_user_controlled()
		return PlayerType(index) == c_playerTypeUser
	end

	text get_name()
		return PlayerName(index)
	end

	string get_race()
		return PlayerRace(index)
	end

	difficulty get_difficulty()
		return new difficulty(PlayerDifficulty(index))
	end

	void set_difficulty(difficulty value)
		PlayerSetDifficulty(index, value.index)
	end

	point get_start_location()
		return PlayerStartLocation(index)
	end

	color get_color()
		return color.get_diffuse_color_for_team_color(get_color_index())
	end

	color get_default_color()
		return color.get_diffuse_color_for_team_color(get_default_color_index())
	end

	int get_color_index()
		return PlayerGetColorIndex(index, false)
	end

	int get_default_color_index()
		return PlayerGetColorIndex(index, true)
	end

	void set_color(int color_index, bool change_unit_colors)
		PlayerSetColorIndex(index, color_index, change_unit_colors)
	end

	void set_allied_chat(player target, bool shared_chat)
		PlayerSetAlliance(index, c_allianceIdChat, target.index, shared_chat)
	end

	bool get_allied_chat(player target)
		return PlayerGetAlliance(index, c_allianceIdChat, target.index)
	end

	void set_give_help(player target, bool help_allowed)
		PlayerSetAlliance(index, c_allianceIdGiveHelp, target.index, help_allowed)
	end

	bool get_give_help(player target)
		return PlayerGetAlliance(index, c_allianceIdGiveHelp, target.index)
	end

	void set_passive(player target, bool should_units_be_passive)
		PlayerSetAlliance(index, c_allianceIdPassive, target.index, should_units_be_passive)
	end

	bool get_passive(player target)
		return PlayerGetAlliance(index, c_allianceIdPassive, target.index)
	end

	void set_pushable(player target, bool should_units_be_pushable)
		PlayerSetAlliance(index, c_allianceIdPushable, target.index, should_units_be_pushable)
	end

	bool get_pushable(player target)
		return PlayerGetAlliance(index, c_allianceIdPushable, target.index)
	end

	void set_seek_help(player target, bool seek_help_allowed)
		PlayerSetAlliance(index, c_allianceIdSeekHelp, target.index, seek_help_allowed)
	end

	bool get_seek_help(player target)
		return PlayerGetAlliance(index, c_allianceIdSeekHelp, target.index)
	end

	void set_shared_control(player target, bool shared_control)
		PlayerSetAlliance(index, c_allianceIdControl, target.index, shared_control)
	end

	bool get_shared_control(player target)
		return PlayerGetAlliance(index, c_allianceIdControl, target.index)
	end

	void set_shared_defeat(player target, bool shared_defeat)
		PlayerSetAlliance(index, c_allianceIdDefeat, target.index, shared_defeat)
	end

	bool get_shared_defeat(player target)
		return PlayerGetAlliance(index, c_allianceIdDefeat, target.index)
	end

	void set_shared_vision(player target, bool shared_vision)
		PlayerSetAlliance(index, c_allianceIdVision, target.index, shared_vision)
	end

	bool get_shared_vision(player target)
		return PlayerGetAlliance(index, c_allianceIdVision, target.index)
	end

	void set_can_spend_resources(player target, bool allowed_spend_resources)
		PlayerSetAlliance(index, c_allianceIdVision, target.index, allowed_spend_resources)
	end

	bool get_can_spend_resources(player target)
		return PlayerGetAlliance(index, c_allianceIdSpend, target.index)
	end

	void set_trade_resources(player target, bool trade_resources_allowed)
		PlayerSetAlliance(index, c_allianceIdTrade, target.index, trade_resources_allowed)
	end

	bool get_trade_resources(player target)
		return PlayerGetAlliance(index, c_allianceIdTrade, target.index)
	end

	bool get_abort_enabled()
		return PlayerGetState(index, c_playerStateAbortEnabled)
	end

	void set_abort_enabled(bool value)
		PlayerSetState(index, c_playerStateAbortEnabled, value)
	end

	bool get_continue_enabled()
		return PlayerGetState(index, c_playerStateContinueEnabled)
	end

	void set_continue_enabled(bool value)
		PlayerSetState(index, c_playerStateContinueEnabled, value)
	end

	bool get_display_in_leader_panel()
		return PlayerGetState(index, c_playerStateDisplayInLeaderPanel)
	end

	void set_display_in_leader_panel(bool value)
		PlayerSetState(index, c_playerStateDisplayInLeaderPanel, value)
	end

	bool get_display_in_view_menu()
		return PlayerGetState(index, c_playerStateDisplayInViewMenu)
	end

	void set_display_in_view_menu(bool value)
		PlayerSetState(index, c_playerStateDisplayInViewMenu, value)
	end

	bool get_xp_gain_enabled()
		return PlayerGetState(index, c_playerStateXPGain)
	end

	void set_xp_gain_enabled(bool value)
		PlayerSetState(index, c_playerStateXPGain, value)
	end

	bool get_restart_enabled()
		return PlayerGetState(index, c_playerStateRestartEnabled)
	end

	void set_restart_enabled(bool value)
		PlayerSetState(index, c_playerStateRestartEnabled, value)
	end

	bool get_show_score()
		return PlayerGetState(index, c_playerStateShowScore)
	end

	void set_show_score(bool value)
		PlayerSetState(index, c_playerStateShowScore, value)
	end

	bool get_show_world_tip()
		return PlayerGetState(index, c_playerStateShowWorldTip)
	end

	void set_show_world_tip(bool value)
		PlayerSetState(index, c_playerStateShowWorldTip, value)
	end

	bool get_unit_fidgeting_enabled()
		return PlayerGetState(index, c_playerStateFidgetingEnabled)
	end

	void set_unit_fidgeting_enabled(bool value)
		PlayerSetState(index, c_playerStateFidgetingEnabled, value)
	end

	void pause_all_charges(bool pause)
		PlayerPauseAllCharges(index, pause)
	end

	void pause_all_cooldowns(bool pause)
		PlayerPauseAllCooldowns(index, pause)
	end

	void add_charge_regen(string charge, fixed value)
		PlayerAddChargeRegen(index, charge, value)
	end

	fixed get_charge_regen(string charge)
		return PlayerGetChargeRegen(index, charge)
	end

	void add_charge_used(string charge, fixed value)
		PlayerAddChargeUsed(index, charge, value)
	end

	fixed get_charge_used(string charge)
		return PlayerGetChargeUsed(index, charge)
	end

	void add_cooldown(string cooldown, fixed value)
		PlayerAddCooldown(index, cooldown, value)
	end

	fixed get_cooldown(string cooldown)
		return PlayerGetCooldown(index, cooldown)
	end

	playergroup get_allies()
		return PlayerGroupAlliance(c_playerGroupAlly, index)
	end

	playergroup get_enemies()
		return PlayerGroupAlliance(c_playerGroupEnemy, index)
	end

	playergroup to_player_group()
		return PlayerGroupSingle(index)
	end

	void disable_achievements()
		AchievementsDisable(index)
	end

	ctor player(playergroup owner_group, int index)
		this.owner_group = owner_group
		this.index = index
	end
 end