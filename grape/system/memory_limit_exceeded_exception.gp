/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class memory_limit_exceeded_exception inherits exception
	ctor memory_limit_exceeded_exception(string message)
		init base(message)
	end

	ctor memory_limit_exceeded_exception(string message, string stack_trace)
		init base(message, stack_trace)
	end
 end