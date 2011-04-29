/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class region_base
	static region empty = RegionEmpty()
	static region entire_map = RegionEntireMap()

	static region create_rect(fixed min_x, fixed min_y, fixed max_x, fixed max_y)
		return RegionRect(min_x, min_y, max_x, max_y)
	end

	static region create_circle(point center, fixed radius)
		return RegionCircle(center, radius)
	end

	static region get_from_id(int id)
		return RegionFromId(id)
	end

	void set_as_playable_area()
		RegionPlayableMapSet(this)
	end

	region get_playable_area()
		return RegionPlayableMap()
	end

	void add_rect(bool additive, fixed min_x, fixed min_y, fixed max_x, fixed max_y)
		RegionAddRect(this, additive, min_x, min_y, max_x, max_y)
	end

	void add_circle(bool additive, point center, fixed radius)
		RegionAddCircle(this, additive, center, radius)
	end

	void add_region(region subject)
		RegionAddRegion(this, subject)
	end

	point get_offset()
		return RegionGetOffset(this)
	end

	void set_offset(point value)
		RegionSetOffset(this, value)
	end

	bool contains(point value)
		return RegionContainsPoint(this, value)
	end

	point get_random_point()
		return RegionRandomPoint(this)
	end

	point get_minimum_bound()
		return RegionGetBoundsMin(this)
	end

	point get_maximum_bound()
		return RegionGetBoundsMax(this)
	end

	point get_center()
		return RegionGetCenter(this)
	end

	void set_center(point value)
		RegionSetCenter(this, value)
	end

	void attach_to_unit(unit target, point offset)
		RegionAttachToUnit(this, target, offset)
	end

	void detach_from_unit()
		RegionAttachToUnit(this, null, null)
	end

	unit get_attached_unit()
		return RegionGetAttachUnit(this)
	end

	private ctor region_base()
	end
 end