/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class sound_base:
	void stop(bool fade):
		SoundStop(this, fade)

	static void stop_all_model_sounds():
		SoundStopAllModelSounds()

	static void stop_all_trigger_sounds(bool fade):
		SoundStopAllTriggerSounds(fade)

	void set_volume(fixed volume):
		SoundSetVolume(this, volume)

	void set_position(point position, fixed height):
		SoundSetPosition(this, position, height)

	void set_offset(fixed time):
		fixed value = time
		int offset = c_soundOffsetStart
		if time < 0.0:
			value = -time
			offset = c_soundOffsetEnd

		SoundSetOffset(this, value, offset)

	void wait(fixed time):
		fixed value = time
		int offset = c_soundOffsetStart
		if time < 0.0:
			value = -time
			offset = c_soundOffsetEnd

		SoundWait(this, value, offset)

	void wait():
		SoundWait(this, 0.0, c_soundOffsetEnd)

	void attach_unit(unit u, fixed height):
		SoundAttachUnit(this, u, height)

	static void set_global_factors(fixed distance, fixed doppler, fixed rolloff):
		SoundSetFactors(distance, doppler, rolloff)

	static void set_reverb(string reverb, fixed duration, bool ambient, bool global):
		SoundSetReverb(reverb, duration, ambient, global)

	private ctor sound_base():