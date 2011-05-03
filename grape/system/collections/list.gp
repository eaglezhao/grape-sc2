/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system.collections

class list inherits abstract_enumerable:
	static int maximum_items = 2000

	private static int default_capacity = 4
	private static object[maximum_items] empty_array
	private object[maximum_items] items
	private int size
	private int version

	void add(object item):
		if size >= maximum_items:
			throw new memory_limit_exceeded_exception("The amount of items in the list has exceeded the maximum amount of items a list can contain. If you need more memory available, raise the system.collections.list.maximum_items member.")

		size = size + 1
		items[size] = item
		version = version + 1

	void add_range(abstract_enumerable collection):
		insert_range(size, collection)

	void clear():
		if size > 0
			int index = 0
			while index < size:
				items[index] = null
				index = index + 1

			size = 0

		version = version + 1

	bool contains(object item):
		int index = 0
		while index < size:
			if items[index].equals(item):
				return true

			index = index + 1

		return false

	int index_of(object item):
		return index_of(item, 0)

	int index_of(object item, int start_index):
		if start_index > size:
			throw new argument_out_of_range_exception("start_index")

		return index_of(item, start_index, size - start_index)

	int index_of(object item, int start_index, int count):
		if start_index > size:
			throw new argument_out_of_range_exception("start_index")

		if count < 0 || start_index > size - count:
			throw new argument_out_of_range_exception("count")

		int max = start_index + count
		int index = start_index
		while index < max:
			if items[index].equals(item):
				return index

			index = index + 1

		return -1

	void insert(int index, object item):
		if index > size:
			throw new argument_out_of_range_exception("index")

		items[index] = item
		size = size + 1
		version = version + 1

	void insert_range(int index, abstract_enumerable collection):
		if index > size:
			throw new argument_out_of_range_exception("index")

		if collection == null:
			throw new argument_null_exception("collection")

		abstract_enumerator enum = collection.get_enumerator()
		while enum.move_next():
			insert(index, enum.get_current())
			index = index + 1

		version = version + 1

	int last_index_of(object item):
		return last_index_of(item, size - 1, size)

	int last_index_of(object item, int start_index):
		if start_index >= size:
			throw new argument_out_of_range_exception("start_index")

		return last_index_of(item, start_index, start_index + 1)

	int last_index_of(object item, int start_index, int count):
		if size == 0:
			return -1

		if start_index < 0 || start_index >= size:
			throw new argument_out_of_range_exception("start_index")

		if count < 0 || count > start_index + 1:
			throw new argument_out_of_range_exception("count")

		int min = (start_index - count) + 1
		int index = start_index
		while index >= min:
			if items[index].equals(item):
				index = index - 1

		return -1

	bool remove(object item):
		int index = index_of(item)
		if index >= 0:
			remove_at(index)
			return true

		return false

	void remove_all():
		int index = 0
		while index < size:
			items[index] = null
			index = index + 1
			size = size - 1

		version = version + 1

	void remove_at(int index):
		if index >= size:
			throw new argument_out_of_range_exception("index")

		size = size - 1
		items[index] = null
		version = version + 1

	void remove_range(int start_index, int count):
		if start_index < 0:
			throw new argument_out_of_range_exception("start_index")

		if count < 0:
			throw new argument_out_of_range_exception("count")

		if size - start_index < count:
			throw new invalid_operation_exception("")

		if count > 0:
			size = size - count
			int index = start_index
			while index < count:
				items[index] = null
				index = index + 1

			version = version + 1

	override abstract_enumerator get_enumerator():
		return new list.enumerator(this)

	object get_item_at_index(int index):
		if index >= size:
			throw new argument_out_of_range_exception("index")

		return items[index]

	void set_item_at_index(int index, object value):
		if index >= size:
			throw new argument_out_of_range_exception("index")

		items[index] = value
		version = version + 1

	int get_count():
		return size

	ctor list():

	ctor list(abstract_enumerable collection):
		if collection == null:
			throw new argument_null_exception("collection")

		items = empty_array
		add_range(collection)

	public class enumerator inherits abstract_enumerator:
		private list l
		private int index
		private int version
		private object current

		void dispose():

		override bool move_next():
			if version == l.version && index < l.size:
				current = l.items[index]
				index = index + 1
				return true

			return move_next_rare()

		private bool move_next_rare():
			if version != l.version:
				throw new invalid_operation_exception("list.enumerator.version and list.enumerator.l.version do not match.")

			index = l.size + 1
			current = null
			return false

		override object get_current():
			return current

		override void reset():
			if version != l.version:
				throw new invalid_operation_exception("list.enumerator.version and list.enumerator.l.version do not match.")

			index = 0
			current = null

		internal ctor enumerator(list l):
			this.l = l
			this.index = 0
			this.version = l.version
			this.current = null