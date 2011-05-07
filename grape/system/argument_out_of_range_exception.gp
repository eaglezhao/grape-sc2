/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class argument_out_of_range_exception inherits argument_exception:
	override string get_message():
		return base.get_message() + "\r\n" + "Parameter was out of the expected range."

	ctor argument_out_of_range_exception(string message, string param_name):
		init base(message, param_name)

	ctor argument_out_of_range_exception(string message):
		init base(message)

	ctor argument_out_of_range_exception(string message, string stack_trace, string param_name):
		init base(message, stack_trace, param_name)