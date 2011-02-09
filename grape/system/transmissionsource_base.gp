/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class transmissionsource_base
	static int invalid_transmission_id = c_invalidTransmissionId
	static fixed transition_duration = c_transmissionTransitionDuration

	class transmission_durations
		static int duration_default = c_transmissionDurationDefault
		static int duration_add = c_transmissionDurationAdd
		static int duration_subtract = c_transmissionDurationSub
		static int duration_set = c_transmissionDurationSet
	end

	static transmissionsource create()
		return TransmissionSource()
	end

	static transmissionsource create_from_unit(unit u, bool flash, bool override_portrait, string animation)
		return TransmissionSourceFromUnit(u, flash, override_portrait, animation)
	end

	static transmissionsource create_from_unit_type(string unit_type, bool override_portrait)
		return TransmissionSourceFromUnitType(unit_type, override_portrait)
	end

	static transmissionsource create_from_model(string model_link)
		return TransmissionSourceFromModel(model_link)
	end

	static transmissionsource create_from_movie(string asset_link, bool subtitles)
		TransmissionSourceFromMovie(asset_link, subtitles)
	end

	int send(playergroup players, int target_portrait, string animation, soundlink sound_link, text speaker, text subtitle, fixed duration, int duration_type, bool wait_until_done)
		return TransmissionSend(players, this, target_portrait, animation, sound_link, speaker, subtitle, duration, duration_type, wait_until_done)
	end

	void clear(int type)
		TransmissionClear(type)
	end

	void clear_all()
		TransmissionClearAll()
	end

	void set_option(int option_index, bool value)
		TransmissionSetOption(option_index, value)
	end

	void wait(int type, fixed offset)
		TransmissionWait(type, offset)
	end

	private ctor transmissionsource_base()
	end
 end