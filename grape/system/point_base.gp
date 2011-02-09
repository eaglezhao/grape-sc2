/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class point_base
	static point origin_point = point.create(0, 0)

	static point create(fixed x, fixed y)
		return Point(x, y)
	end

	static point get_from_id(int id)
		return PointFromId(id)
	end

	fixed get_world_height(int heightmap)
		return WorldHeight(heightmap, this)
	end

	fixed get_world_height()
		return WorldHeight(c_heightMapGround, this)
	end

	fixed get_x()
		return PointGetX(this)
	end

	void set_x(fixed value)
		point new_point = create(value, get_y())
		PointSet(this, new_point)
	end

	fixed get_y()
		return PointGetY(this)
	end

	void set_y(fixed value)
		point new_point = create(get_x(), value)
		PointSet(this, new_point)
	end

	fixed get_height()
		return PointGetHeight(this)
	end

	void set_height(fixed value)
		PointSetHeight(this, value)
	end

	fixed get_distance()
		return DistanceBetweenPoints(origin_point, this)
	end

	void set_distance(fixed value)
		fixed old_angle = get_angle()
		point new_point = PointWithOffsetPolar(origin_point, value, old_angle)
		PointSet(this, new_point)
	end

	fixed get_angle()
		return AngleBetweenPoints(origin_point, this)
	end

	void set_angle(fixed value)
		fixed old_angle = get_angle()
		point new_point = PointWithOffsetPolar(origin_point, old_angle, value)
		PointSet(this, new_point)
	end

	bool is_point_in_range(point other, fixed range)
		if range < 0
			return false
		end

		return DistanceBetweenPoints(this, other) <= range
	end

	static fixed angle_between_points(point a, point b)
		return AngleBetweenPoints(a, b)
	end

	fixed angle_between_points(point other)
		return angle_between_points(this, other)
	end

	static fixed distance_between_points(point a, point b)
		return DistanceBetweenPoints(a, b)
	end

	fixed distance_between_points(point other)
		return distance_between_points(this, other)
	end

	point translate(fixed x, fixed y)
		return PointWithOffset(this, x, y)
	end

	point translate_polar(fixed radius, fixed angle)
		return PointWithOffsetPolar(this, radius, angle)
	end

	bool is_passable()
		return PointPathingPassable(this)
	end

	bool is_connected_to(point other)
		return PointPathingIsConnected(this, other)
	end

	int point_pathing_cost(point other)
		return PointPathingCost(this, other)
	end

	fixed get_cliff_level()
		return PointPathingCliffLevel(this)
	end

	override bool equals(object other)
		return this == other
	end

	override int get_hash_code()
		return FixedToInt(get_x()) ^ FixedToInt(get_y())
	end

	override string to_string()
		return "point_base { get_x() = " + get_x().to_string() + ", get_y() = " + get_y().to_string() + " }"
	end

	private ctor point_base()
	end
 end