/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class soundlink_base:
	static int asset_index_any = c_soundIndexAny

	static soundlink create(string id, int index):
		return SoundLink(id, index)

	string get_id():
		return SoundLinkId(this)

	int get_asset_index():
		return SoundLinkAsset(this)

	sound play(playergroup group, fixed volume, fixed offset):
		SoundPlay(this, group, volume, offset)
		return SoundLastPlayed()

	sound play_at_point(playergroup group, fixed volume, fixed offset, point location, fixed height):
		SoundPlayAtPoint(this, group, location, height, volume, offset)
		return SoundLastPlayed()

	sound play_on_unit(playergroup group, fixed volume, fixed offset, unit u, fixed height):
		SoundPlayOnUnit(this, group, u, height, volume, offset)
		return SoundLastPlayed()

	sound play_scene(playergroup group, unitgroup units, string animation):
		SoundPlayScene(this, group, units, animation)
		return SoundLastPlayed()

	sound play_scene_file(playergroup group, string file, string camera):
		SoundPlaySceneFile(this, group, file, camera)
		return SoundLastPlayed()

	void query_length():
		SoundLengthQuery(this)

	static void query_wait():
		SoundLengthQueryWait()

	fixed get_length_sync():
		return SoundLengthSync(this)

	text get_subtitle_text():
		return SoundSubtitleText(this)

	override bool equals(object other):
		return this == other

	override int get_hash_code():
		return get_id().get_hash_code() ^ get_asset_index()

	override string to_string():
		return "soundlink_base { get_id() = " + get_id().to_string() + ", get_asset_index() = " + get_asset_index().to_string() + " }"

	private ctor soundlink_base():