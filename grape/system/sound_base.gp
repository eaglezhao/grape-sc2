/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class sound_base
	void stop(bool fade)
		SoundStop(this, fade)
	end

	static void stop_all_model_sounds()
		SoundStopAllModelSounds()
	end

	static void stop_all_trigger_sounds(bool fade)
		SoundStopAllTriggerSounds(fade)
	end

	void set_volume(fixed volume)
		SoundSetVolume(this, volume)
	end

	void set_position(point position, fixed height)
		SoundSetPosition(this, position, height)
	end

	void set_offset(fixed time)
		fixed value = time
		int offset = c_soundOffsetStart
		if time < 0.0
			value = -time
			offset = c_soundOffsetEnd
		end

		SoundSetOffset(this, value, offset)
	end

	void wait(fixed time)
		fixed value = time
		int offset = c_soundOffsetStart
		if time < 0.0
			value = -time
			offset = c_soundOffsetEnd
		end

		SoundWait(this, value, offset)
	end

	void wait()
		SoundWait(this, 0.0, c_soundOffsetEnd)
	end

	void attach_unit(unit u, fixed height)
		SoundAttachUnit(this, u, height)
	end

	static void set_global_factors(fixed distance, fixed doppler, fixed rolloff)
		SoundSetFactors(distance, doppler, rolloff)
	end

	static void set_reverb(string reverb, fixed duration, bool ambient, bool global)
		SoundSetReverb(reverb, duration, ambient, global)
	end

	private ctor sound_base()
	end
 end