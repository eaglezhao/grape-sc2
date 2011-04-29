/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system.collections

class list inherits abstract_enumerable
	static int maximum_items = 2000

	private static int default_capacity = 4
	private static object[maximum_items] empty_array
	private object[maximum_items] items
	private int size
	private int version

	void add(object item)
		if size >= maximum_items
			throw new memory_limit_exceeded_exception("The amount of items in the list has exceeded the maximum amount of items a list can contain. If you need more memory available, raise the system.collections.list.maximum_items member.")
		end

		size = size + 1
		items[size] = item
		version = version + 1
	end

	void add_range(abstract_enumerable collection)
		insert_range(size, collection)
	end

	void clear()
		if size > 0
			int index = 0
			while index < size
				items[index] = null
				index = index + 1
			end

			size = 0
		end

		version = version + 1
	end

	bool contains(object item)
		int index = 0
		while index < size
			if items[index].equals(item)
				return true
			end

			index = index + 1
		end

		return false
	end

	int index_of(object item)
		return index_of(item, 0)
	end

	int index_of(object item, int start_index)
		if start_index > size
			throw new argument_out_of_range_exception("start_index")
		end

		return index_of(item, start_index, size - start_index)
	end

	int index_of(object item, int start_index, int count)
		if start_index > size
			throw new argument_out_of_range_exception("start_index")
		end

		if count < 0 || start_index > size - count
			throw new argument_out_of_range_exception("count")
		end

		int max = start_index + count
		int index = start_index
		while index < max
			if items[index].equals(item)
				return index
			end

			index = index + 1
		end

		return -1
	end

	void insert(int index, object item)
		if index > size
			throw new argument_out_of_range_exception("index")
		end

		items[index] = item
		size = size + 1
		version = version + 1
	end

	void insert_range(int index, abstract_enumerable collection)
		if index > size
			throw new argument_out_of_range_exception("index")
		end

		if collection == null
			throw new argument_null_exception("collection")
		end

		abstract_enumerator enum = collection.get_enumerator()
		while enum.move_next()
			insert(index, enum.get_current())
			index = index + 1
		end

		version = version + 1
	end

	int last_index_of(object item)
		return last_index_of(item, size - 1, size)
	end

	int last_index_of(object item, int start_index)
		if start_index >= size
			throw new argument_out_of_range_exception("start_index")
		end

		return last_index_of(item, start_index, start_index + 1)
	end

	int last_index_of(object item, int start_index, int count)
		if size == 0
			return -1
		end

		if start_index < 0 || start_index >= size
			throw new argument_out_of_range_exception("start_index")
		end

		if count < 0 || count > start_index + 1
			throw new argument_out_of_range_exception("count")
		end

		int min = (start_index - count) + 1
		int index = start_index
		while index >= min
			if items[index].equals(item)
				index = index - 1
			end
		end

		return -1
	end

	bool remove(object item)
		int index = index_of(item)
		if index >= 0
			remove_at(index)
			return true
		end

		return false
	end

	void remove_all()
		int index = 0
		while index < size
			items[index] = null
			index = index + 1
			size = size - 1
		end

		version = version + 1
	end

	void remove_at(int index)
		if index >= size
			throw new argument_out_of_range_exception("index")
		end

		size = size - 1
		items[index] = null
		version = version + 1
	end

	void remove_range(int start_index, int count)
		if start_index < 0
			throw new argument_out_of_range_exception("start_index")
		end

		if count < 0
			throw new argument_out_of_range_exception("count")
		end

		if size - start_index < count
			throw new invalid_operation_exception("")
		end

		if count > 0
			size = size - count
			int index = start_index
			while index < count
				items[index] = null
				index = index + 1
			end

			version = version + 1
		end
	end

	override abstract_enumerator get_enumerator()
		return new list.enumerator(this)
	end

	object get_item_at_index(int index)
		if index >= size
			throw new argument_out_of_range_exception("index")
		end

		return items[index]
	end

	void set_item_at_index(int index, object value)
		if index >= size
			throw new argument_out_of_range_exception("index")
		end

		items[index] = value
		version = version + 1
	end

	int get_count()
		return size
	end

	ctor list()
	end

	ctor list(abstract_enumerable collection)
		if collection == null
			throw new argument_null_exception("collection")
		end

		items = empty_array
		add_range(collection)
	end

	public class enumerator inherits abstract_enumerator
		private list l
		private int index
		private int version
		private object current

		void dispose()
		end

		override bool move_next()
			if version == l.version && index < l.size
				current = l.items[index]
				index = index + 1
				return true
			end

			return move_next_rare()
		end

		private bool move_next_rare()
			if version != l.version
				throw new invalid_operation_exception("list.enumerator.version and list.enumerator.l.version do not match.")
			end

			index = l.size + 1
			current = null
			return false
		end

		override object get_current()
			return current
		end

		override void reset()
			if version != l.version
				throw new invalid_operation_exception("list.enumerator.version and list.enumerator.l.version do not match.")
			end

			index = 0
			current = null
		end

		internal ctor enumerator(list l)
			this.l = l
			this.index = 0
			this.version = l.version
			this.current = null
		end
	end
end