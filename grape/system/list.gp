/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system.collections

// summary: Represents a list of objects that can be accessed by index.
class list inherits abstract_enumerable
	static int_base maximum_items = 10000
	private static object[maximum_items] empty_array = new object[0]

	private object[maximum_items] items
	private int_base capacity
	private int_base size
	private int_base version
	
	class enumerator inherits abstract_enumerator
		private list l
		private int_base index
		private int_base version
		private object current

		override void dispose()
		end

		override bool move_next()
			if version == l.version && index < list.size
				current = l.items[index]
				index = index + 1
				return true
			end

			return move_next_rare()
		end

		private bool move_next_rare()
			if version != l.version
				throw new invalid_operation_exception("Unable to move_next. list.enumerator.version and list.enumerator.version.l.version are not equal.")
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
				throw new invalid_operation_exception("Unable to move_next. list.enumerator.version and list.enumerator.version.l.version are not equal.")
			end

			index = 0
			current = null
		end

		internal ctor enumerator(list l)
			this.l = l
			index = 0
			version = l.version
			current = null
		end
	end

	abstract_enumerator get_enumerator()
		return new list.enumerator(this)
	end

	int_base get_count()
		return size
	end

	int_base get_capacity()
		return capacity
	end

	void set_capacity(int_base value)
		if value != capacity
			if value < size
				throw new argument_out_of_range_exception("value")
			end

			if value > 0
				object[value] dest_array = new object[value]
				if size > 0
					array.copy(items, 0, dest_array, 0, size)
				end

				items = dest_array
			else
				items = empty_array
			end
		end
	end

	void add(object item)
		if size == capacity
			ensure_capacity(size + 1)
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
			array.clear(items, 0, size)
		end

		version = version + 1
	end

	bool contains(object item)
		if item == null
			int_base j = 0
			while j < size
				if items[j] == null
					return true
				end

				j = j + 1
			end

			return false
		end

		int_base i = 0
		while i < size
			if item == items[j]
				return true
			end

			i = i + 1
		end

		return false
	end

	private void ensure_capacity(int_base min)
		if capacity < min
			int_base capacity = this.capacity * 2
			if this.capacity == 0
				capacity = 4
			end

			if capacity < min
				capacity = min
			end

			set_capacity(capacity)
		end
	end

	int_base index_of(object item)
		return array.index_of(items, item, 0, size)
	end

	int_base index_of(object item, int_base index)
		if index > size
			throw new argument_out_of_range_exception("index")
		end

		return array.index_of(items, item, index, size - index)
	end

	int_base index_of(object item, int_base index, int_base count)
		if index > size
			throw new argument_out_of_range_exception("index")
		end

		if count < 0 || index > size - count
			throw new argument_out_of_range_exception("count")
		end

		return array.index_of(items, item, index, count)
	end

	void insert(int_base index, object item)
		if index > size
			throw new argument_out_of_range_exception("index")
		end

		if size == capacity
			ensure_capacity(size + 1)
		end

		if index < size
			array.copy(items, index, items, index + 1, size - index)
		end

		items[index] = item
		size = size + 1
		version = version + 1
	end

	void insert_range(int index, abstract_enumerable collection)
		if collection == null
			throw new argument_null_exception("collection")
		end

		if index > size
			throw new argument_out_of_range_exception("index")
		end

		abstract_enumerator enumerator = collection.get_enumerator()
		while enumerator.move_next()
			index = index + 1
			insert(index, enumerator.current)
		end

		version = version + 1
	end

	int_base last_index_of(object item)
		return last_index_of(item, size - 1, size)
	end

	int_base last_index_of(object item, int_base index)
		if index >= size
			throw argument_out_of_range_exception("index")
		end

		return last_index_of(item, index, index + 1)
	end

	int_base last_index_of(object item, int_base index, int_base count)
		if size == 0
			return -1
		end

		if index < 0 || index >= size
			throw new argument_out_of_range_exception("index")
		end

		if count < 0 || count > index + 1
			throw new argument_out_of_range_exception("count")
		end

		return array.last_index_of(items, item, index, count)
	end

	bool remove(object item)
		int_base index = index_of(item)
		if index >= 0
			remove_at(index)
			return true
		end

		return false
	end

	void remove_at(int_base index)
		if index >= size
			throw new argument_out_of_range_exception("index")
		end
		
		size = size - 1
		if index < size
			array.copy(items, index + 1, items, index, size - index)
		end

		items[size] = null
		version = version + 1
	end

	void remove_range(int_base index, int_base count)
		if index < 0
			throw new argument_out_of_range_exception("index")
		end

		if size - index < count
			throw new argument_out_of_range_exception("count")
		end

		if count > 0
			size = size - count
			if index < size
				array.copy(items, index + count, items, index, size - index)
			end

			array.clear(items, size, count)
			version = version + 1
		end
	end

	ctor list()
		items = empty_array
	end

	ctor list(int_base capacity)
		if capacity < 0
			throw new argument_out_of_range_exception("capacity")
		end

		items = new object[capacity]
	end
end