/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents a string of characters.
class string_base
	static int not_found = -1
	static int replace_all = -1

	int get_length()
		return StringLength(this)
	end

	string convert_to_case(bool upper)
		return StringCase(this, upper)
	end

	string to_upper_case()
		return StringCase(this, true)
	end

	string to_lower_case()
		return StringCase(this, false)
	end

	string substring(int start_index, int end_index)
		return StringSub(this, start_index, end_index)
	end

	string substring(int start)
		return StringSub(this, start, get_length())
	end

	string get_char_at(int index)
		return substring(index, index + 1)
	end

	override bool equals(object other)
		string other_str = other as string
		return this == other_str
	end

	bool equals_ignore_case(string str)
		return StringEqual(this, str, false)
	end

	bool equals(string str, bool case_sensitive)
		return StringEqual(this, str, case_sensitive)
	end

	int find(string to_find)
		return StringFind(this, to_find, true)
	end

	int find(string to_find, bool case_sensitive)
		return StringFind(this, to_find, case_sensitive)
	end

	bool starts_with(string value)
		return StringContains(this, value, c_stringBegin, true)
	end

	bool ends_with(string value)
		return StringContains(this, value, c_stringEnd, true)
	end

	bool starts_with(string value, bool case_sensitive)
		return StringContains(this, value, c_stringBegin, case_sensitive)
	end

	bool ends_with(string value, bool case_sensitive)
		return StringContains(this, value, c_stringEnd, case_sensitive)
	end

	bool contains(string value)
		return StringContains(this, value, c_stringAnywhere, true)
	end

	bool contains(string value, bool case_sensitive)
		return StringContains(this, value, c_stringAnywhere, case_sensitive)
	end

	string get_word_at(int index)
		return StringWord(this, index)
	end

	string replace_range(string replacement, int start_index, int end_index)
		return StringReplace(this, replacement, start_index, end_index)
	end

	string replace_all(string search_for, string replace_with)
		return StringReplaceWord(this, search_for, replace_with, c_stringReplaceAll, true)
	end

	string replace_all(string search_for, string replace_with, bool case_sensitive)
		return StringReplaceWord(this, search_for, replace_with, c_stringReplaceAll, case_sensitive)
	end

	string replace_first(string search_for, string replace_with)
		return StringReplaceWord(this, search_for, replace_with, 1, true)
	end

	string replace_first(string search_for, string replace_with, bool case_sensitive)
		return StringReplaceWord(this, search_for, replace_with, 1, case_sensitive)
	end

	string replace(string search_for, string replace_with, int max_replacements)
		return StringReplaceWord(this, search_for, replace_with, max_replacements, true)
	end

	string replace(string search_for, string replace_with, int max_replacements, bool case_sensitive)
		return StringReplaceWord(this, search_for, replace_with, max_replacements, case_sensitive)
	end

	private ctor string_base()

	end
end