/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class bank_base
	static int type_fixed = c_bankTypeFixed
	static int type_bool = c_bankTypeFlag
	static int type_int = c_bankTypeInt
	static int type_string = c_bankTypeString
	static int type_unit = c_bankTypeUnit
	static int type_point = c_bankTypePoint
	static int type_text = c_bankTypeText

	static bank load(string name, int player)
		return BankLoad(name, player)
	end

	static bool does_bank_exist(string name, int player)
		return BankExists(name, player)
	end

	static unit last_retrieved_unit()
		return BankLastRestoredUnit()
	end

	static bank last_opened()
		return BankLastCreated()
	end

	string get_name()
		return BankName(this)
	end

	int get_owning_player()
		return BankPlayer(this)
	end

	int get_section_count()
		return BankSectionCount(this)
	end

	string get_section_name(int index)
		return BankSectionName(this, index)
	end

	int get_key_count(string section)
		return BankKeyCount(this, section)
	end

	string get_key_name(string section, int index)
		return BankKeyName(this, section, index)
	end

	bool does_section_exist(string name)
		return BankSectionExists(this, name)
	end

	bool does_key_exist(string section, string key)
		return BankKeyExists(this, section, key)
	end

	void remove_section(string name)
		BankSectionRemove(this, name)
	end

	void remove_key(string section, string key)
		BankKeyRemove(this, section, key)
	end

	void remove_from_cache()
		BankRemove(this)
	end

	void save()
		BankSave(this)
	end

	bool is_value_fixed(string section, string key)
		return BankValueIsType(this, section, key, type_fixed)
	end

	bool is_value_bool(string section, string key)
		return BankValueIsType(this, section, key, type_bool)
	end

	bool is_value_int(string section, string key)
		return BankValueIsType(this, section, key, type_int)
	end

	bool is_value_string(string section, string key)
		return BankValueIsType(this, section, key, type_string)
	end

	bool is_value_unit(string section, string key)
		return BankValueIsType(this, section, key, type_unit)
	end

	bool is_value_point(string section, string key)
		return BankValueIsType(this, section, key, type_point)
	end

	bool is_value_text(string section, string key)
		return BankValueIsType(this, section, key, type_text)
	end

	fixed get_value_as_fixed(string section, string key)
		return BankValueGetAsFixed(this, section, key)
	end

	bool get_value_as_bool(string section, string key)
		return BankValueGetAsFlag(this, section, key)
	end

	int get_value_as_int(string section, string key)
		return BankValueGetAsInt(this, section, key)
	end

	string get_value_as_string(string section, string key)
		return BankValueGetAsString(this, section, key)
	end

	unit get_value_as_unit(string section, string key, int player, point position, fixed angle)
		return BankValueGetAsUnit(this, section, key, player, position, angle)
	end

	point get_value_as_point(string section, string key)
		return BankValueGetAsPoint(this, section, key)
	end

	text get_value_as_text(string section, string key)
		return BankValueGetAsText(this, section, key)
	end

	void set_value(string section, string key, fixed value)
		BankValueSetFromFixed(this, section, key, value)
	end

	void set_value(string section, string key, bool value)
		BankValueSetFromFlag(this, section, key, value)
	end

	void set_value(string section, string key, int value)
		BankValueSetFromInt(this, section, key, value)
	end

	void set_value(string section, string key, string value)
		BankValueSetFromString(this, section, key, value)
	end

	void set_value(string section, string key, unit value)
		BankValueSetFromUnit(this, section, key, value)
	end

	void set_value(string section, string key, point value)
		BankValueSetFromPoint(this, section, key, value)
	end

	void set_value(string section, string key, text value)
		BankValueSetFromText(this, section, key, value)
	end

	private ctor bank_base()
	end
 end