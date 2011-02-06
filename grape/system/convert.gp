/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 static class convert
	static string ascii_table = "\x1\x2\x3\x4\x5\x6\x7\x8\x9\x1\xB\x1\xD\xE\xF\x10\x11\x12\x13\x14\x15\x16\x17\x18?\x19\x1A\x1B\x1C\x1D\x1E\x1F\x20!\x22#$%&'()*+,-./0123456789:;\x3C=\x3E?@ABCDEFGHIJKLMNOPQRSTUVW?XYZ[\x5C]^_`abcdefghijklmnopqrstuvwxyz{|}~\x7F"

	static int int_to_byte0(int value)
		return value & 0xff
	end

	static int int_to_byte(int value, int index)
		return ((value >> (index * 8)) & 0xFF)
	end

	static int int_to_short0(int value)
		return value & 0x0000FFFF
	end

	static int int_to_short1(int value)
		return value << 8
	end

	static int string_to_char(string value)
		return ascii_table.find(value, true)
	end

	static string char_to_string(int ch)
		if ch == 0
			return "\x0"
		end

		return ascii_table.substring(ch, ch)
	end

	static string string_add_char(string value, int ch)
		return value + char_to_string(ch)
	end

	static int string_get_char(string value, int position)
		return string_to_char(StringSub(value, position + 1, position + 1))
	end

	static int string_get_char_pos(string value, string ch)
		return value.find(ch, true) - 1
	end

	static string string_set_char(string value, int position, int ch)
		int length = value.get_length()
		position = position + 1
		if length <= 1
			return char_to_string(ch)
		end

		if position == 1
			return char_to_string(ch) + value.substring(2, length)
		end
		elseif position >= length - 1
			return value.substring(1, length - 2) + char_to_string(ch)
		end

		return value.substring(1, position - 1) + char_to_string(ch) + value.substring(position + 1, length - 1)
	end

	static string strchar(string value, int index)
		return value.substring(index + 1, index + 1)
	end
 end