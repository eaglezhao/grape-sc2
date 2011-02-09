/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class sound_channel
	static sound_channel me = new sound_channel(c_soundCategoryME)
	static sound_channel movie = new sound_channel(c_soundCategoryMovie)
	static sound_channel tv = new sound_channel(c_soundCategoryTV)
	static sound_channel dialogue = new sound_channel(c_soundCategoryDialogue)
	static sound_channel mission = new sound_channel(c_soundCategoryMission)
	static sound_channel music = new sound_channel(c_soundCategoryMusic)
	static sound_channel ambient = new sound_channel(c_soundCategoryAmbient)
	static sound_channel s_ambient = new sound_channel(c_soundCategorySAmbient)
	static sound_channel alert = new sound_channel(c_soundCategoryAlert)
	static sound_channel death = new sound_channel(c_soundCategoryDeath)
	static sound_channel ready = new sound_channel(c_soundCategoryReady)
	static sound_channel spell = new sound_channel(c_soundCategorySpell)
	static sound_channel combat = new sound_channel(c_soundCategoryCombat)
	static sound_channel voice = new sound_channel(c_soundCategoryVoice)
	static sound_channel ui = new sound_channel(c_soundCategoryUI)
	static sound_channel flames = new sound_channel(c_soundCategoryFlames)
	static sound_channel build = new sound_channel(c_soundCategoryBuild)
	static sound_channel gather = new sound_channel(c_soundCategoryGather)
	static sound_channel doodad = new sound_channel(c_soundCategoryDoodad)
	static sound_channel emitters = new sound_channel(c_soundCategorySEmitters)
	static sound_channel pieces = new sound_channel(c_soundCategorySPieces)
	static sound_channel foley = new sound_channel(c_soundCategoryFoley)
	static sound_channel movement = new sound_channel(c_soundCategoryMovement)
	static sound_channel other = new sound_channel(c_soundCategoryOther)

	int index

	ctor sound_channel(int index)
		this.index = index
	end
end