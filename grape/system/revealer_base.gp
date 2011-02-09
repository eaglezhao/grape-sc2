/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class revealer_base
	void destroy()
		VisRevealerDestroy(this)
	end

	void enable(bool value)
		VisRevealerEnable(this, value)
	end

	void update()
		VisRevealerUpdate(this)
	end

	private ctor revealer_base()
	end
 end