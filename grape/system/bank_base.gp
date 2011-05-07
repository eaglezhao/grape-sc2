/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class bank_base:
	static int type_fixed = c_bankTypeFixed
	static int type_bool = c_bankTypeFlag
	static int type_int = c_bankTypeInt
	static int type_string = c_bankTypeString
	static int type_unit = c_bankTypeUnit
	static int type_point = c_bankTypePoint
	static int type_text = c_bankTypeText

	static bank load(string name, int player):
		return BankLoad(name, player)

	static bool does_bank_exist(string name, int player):
		return BankExists(name, player)

	static unit last_retrieved_unit():
		return BankLastRestoredUnit()

	static bank last_opened():
		return BankLastCreated()

	string get_name():
		return BankName(this)

	int get_owning_player():
		return BankPlayer(this)

	int get_section_count():
		return BankSectionCount(this)

	string get_section_name(int index):
		return BankSectionName(this, index)

	int get_key_count(string section):
		return BankKeyCount(this, section)

	string get_key_name(string section, int index):
		return BankKeyName(this, section, index)

	bool does_section_exist(string name):
		return BankSectionExists(this, name)

	bool does_key_exist(string section, string key):
		return BankKeyExists(this, section, key)

	void remove_section(string name):
		BankSectionRemove(this, name)

	void remove_key(string section, string key):
		BankKeyRemove(this, section, key)

	void remove_from_cache():
		BankRemove(this)

	void save():
		BankSave(this)

	bool is_value_fixed(string section, string key):
		return BankValueIsType(this, section, key, type_fixed)

	bool is_value_bool(string section, string key):
		return BankValueIsType(this, section, key, type_bool)

	bool is_value_int(string section, string key):
		return BankValueIsType(this, section, key, type_int)

	bool is_value_string(string section, string key):
		return BankValueIsType(this, section, key, type_string)

	bool is_value_unit(string section, string key):
		return BankValueIsType(this, section, key, type_unit)

	bool is_value_point(string section, string key):
		return BankValueIsType(this, section, key, type_point)

	bool is_value_text(string section, string key):
		return BankValueIsType(this, section, key, type_text)

	fixed get_value_as_fixed(string section, string key):
		return BankValueGetAsFixed(this, section, key)

	bool get_value_as_bool(string section, string key):
		return BankValueGetAsFlag(this, section, key)

	int get_value_as_int(string section, string key):
		return BankValueGetAsInt(this, section, key)

	string get_value_as_string(string section, string key):
		return BankValueGetAsString(this, section, key)

	unit get_value_as_unit(string section, string key, int player, point position, fixed angle):
		return BankValueGetAsUnit(this, section, key, player, position, angle)

	point get_value_as_point(string section, string key):
		return BankValueGetAsPoint(this, section, key)

	text get_value_as_text(string section, string key):
		return BankValueGetAsText(this, section, key)

	void set_value(string section, string key, fixed value):
		BankValueSetFromFixed(this, section, key, value)

	void set_value(string section, string key, bool value):
		BankValueSetFromFlag(this, section, key, value)

	void set_value(string section, string key, int value):
		BankValueSetFromInt(this, section, key, value)

	void set_value(string section, string key, string value):
		BankValueSetFromString(this, section, key, value)

	void set_value(string section, string key, unit value):
		BankValueSetFromUnit(this, section, key, value)

	void set_value(string section, string key, point value):
		BankValueSetFromPoint(this, section, key, value)

	void set_value(string section, string key, text value):
		BankValueSetFromText(this, section, key, value)

	private ctor bank_base():