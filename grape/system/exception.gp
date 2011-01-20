/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents an exception that has been thrown by the user.
class exception
	private string message
	private string stack_trace

	string get_message()
		return message
	end

	string get_stack_trace()
		return message
	end

	ctor exception(string message)
		init this(message, "")
	end

	ctor exception(string message, string stack_trace)
		this.message = message
		this.stack_trace = stack_trace
	end
end