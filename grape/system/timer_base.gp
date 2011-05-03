/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class timer_base:
	static int game_time = 0
	static int real_time = 1
	static int ai_time = 2
	static fixed duration_infinite = -1.0

	static timer create():
		return TimerCreate()

	void destroy():
		TimerPause(this, true)
		// TimerDestroy does not exist?

	void start(fixed duration, bool periodic, int time_type):
		TimerStart(this, duration, periodic, time_type)

	void start_periodic(fixed duration, int time_type):
		TimerStart(this, duration, true, time_type)

	void start_once(fixed duration, int time_type):
		TimerStart(this, duration, false, time_type)

	fixed get_remaining():
		return TimerGetRemaining(this)

	fixed get_duration():
		return TimerGetDuration(this)

	fixed get_elapsed():
		return TimerGetElapsed(this)

	bool is_paused():
		return TimerIsPaused(this)

	void set_is_paused(bool value):
		TimerPause(this, value)

	void pause():
		TimerPause(this, true)

	void unpause():
		TimerPause(this, false)

	void restart():
		TimerRestart(this)

	private ctor timer_base():