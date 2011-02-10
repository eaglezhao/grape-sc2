/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class argument_exception inherits exception
	private string param_name

	override string get_message()
		string message = base.get_message()
		if param_name != null && param_name.get_length() != 0
			message = message + "\r\nParameter name: " + param_name
		end

		return message
	end

	string get_param_name()
		return param_name
	end

	ctor argument_exception(string message, string param_name)
		init base(message)
		this.param_name = param_name
	end

	ctor argument_exception(string message)
		init base(message)
	end

	ctor argument_exception(string message, string stack_trace, string param_name)
		init base(message, stack_trace)
		this.param_name = param_name
	end
 end