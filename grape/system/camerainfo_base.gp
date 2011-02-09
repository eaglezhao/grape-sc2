/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class camerainfo_base
	fixed get_field_of_view()
		return CameraInfoGetValue(this, c_cameraValueFieldOfView)
	end

	void set_field_of_view(fixed value)
		CameraInfoSetValue(this, c_cameraValueFieldOfView, value)
	end

	fixed get_near_clip()
		return CameraInfoGetValue(this, c_cameraValueNearClip)
	end

	void set_near_clip(fixed value)
		CameraInfoSetValue(this, c_cameraValueNearClip, value)
	end

	fixed get_far_clip()
		return CameraInfoGetValue(this, c_cameraValueFarClip)
	end

	void set_far_clip(fixed value)
		CameraInfoSetValue(this, c_cameraValueFarClip, value)
	end

	fixed get_shadow_clip()
		return CameraInfoGetValue(this, c_cameraValueShadowClip)
	end

	void set_shadow_clip(fixed value)
		CameraInfoSetValue(this, c_cameraValueShadowClip, value)
	end

	fixed get_distance()
		return CameraInfoGetValue(this, c_cameraValueDistance)
	end

	void set_distance(fixed value)
		CameraInfoSetValue(this, c_cameraValueDistance, value)
	end

	fixed get_pitch()
		return CameraInfoGetValue(this, c_cameraValuePitch)
	end

	void set_pitch(fixed value)
		CameraInfoSetValue(this, c_cameraValuePitch, value)
	end

	fixed get_yaw()
		return CameraInfoGetValue(this, c_cameraValueYaw)
	end

	void set_yaw(fixed value)
		CameraInfoSetValue(this, c_cameraValueYaw, value)
	end

	fixed get_roll()
		return CameraInfoGetValue(this, c_cameraValueRoll)
	end

	void set_roll(fixed value)
		CameraInfoSetValue(this, c_cameraValueRoll, value)
	end

	fixed get_height_offset()
		return CameraInfoGetValue(this, c_cameraValueHeightOffset)
	end

	void set_height_offset(fixed value)
		CameraInfoSetValue(this, c_cameraValueHeightOffset, value)
	end

	fixed get_depth_of_field()
		return CameraInfoGetValue(this, c_cameraValueDepthOfField)
	end

	void set_depth_of_field(fixed value)
		CameraInfoSetValue(this, c_cameraValueDepthOfField, value)
	end

	fixed get_focal_depth()
		return CameraInfoGetValue(this, c_cameraValueFocalDepth)
	end

	void set_focal_depth(fixed value)
		CameraInfoSetValue(this, c_cameraValueFocalDepth, value)
	end

	fixed get_fall_off_start()
		return CameraInfoGetValue(this, c_cameraValueFalloffStart)
	end

	void set_fall_off_start(fixed value)
		CameraInfoSetValue(this, c_cameraValueFalloffStart, value)
	end

	fixed get_fall_off_end()
		return CameraInfoGetValue(this, c_cameraValueFalloffEnd)
	end

	void set_fall_off_end(fixed value)
		CameraInfoSetValue(this, c_cameraValueFalloffEnd, value)
	end

	point get_target()
		return CameraInfoGetTarget(this)
	end

	void set_target(point value)
		CameraInfoSetTarget(this, value)
	end

	private ctor camerainfo_base()
	end
 end