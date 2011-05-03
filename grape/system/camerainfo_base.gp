/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class camerainfo_base:
	fixed get_field_of_view():
		return CameraInfoGetValue(this, c_cameraValueFieldOfView)

	void set_field_of_view(fixed value):
		CameraInfoSetValue(this, c_cameraValueFieldOfView, value)

	fixed get_near_clip():
		return CameraInfoGetValue(this, c_cameraValueNearClip)

	void set_near_clip(fixed value):
		CameraInfoSetValue(this, c_cameraValueNearClip, value)

	fixed get_far_clip():
		return CameraInfoGetValue(this, c_cameraValueFarClip)

	void set_far_clip(fixed value):
		CameraInfoSetValue(this, c_cameraValueFarClip, value)

	fixed get_shadow_clip():
		return CameraInfoGetValue(this, c_cameraValueShadowClip)

	void set_shadow_clip(fixed value):
		CameraInfoSetValue(this, c_cameraValueShadowClip, value)

	fixed get_distance():
		return CameraInfoGetValue(this, c_cameraValueDistance)

	void set_distance(fixed value):
		CameraInfoSetValue(this, c_cameraValueDistance, value)

	fixed get_pitch():
		return CameraInfoGetValue(this, c_cameraValuePitch)

	void set_pitch(fixed value):
		CameraInfoSetValue(this, c_cameraValuePitch, value)

	fixed get_yaw():
		return CameraInfoGetValue(this, c_cameraValueYaw)

	void set_yaw(fixed value):
		CameraInfoSetValue(this, c_cameraValueYaw, value)

	fixed get_roll():
		return CameraInfoGetValue(this, c_cameraValueRoll)

	void set_roll(fixed value):
		CameraInfoSetValue(this, c_cameraValueRoll, value)

	fixed get_height_offset():
		return CameraInfoGetValue(this, c_cameraValueHeightOffset)

	void set_height_offset(fixed value):
		CameraInfoSetValue(this, c_cameraValueHeightOffset, value)

	fixed get_depth_of_field():
		return CameraInfoGetValue(this, c_cameraValueDepthOfField)

	void set_depth_of_field(fixed value):
		CameraInfoSetValue(this, c_cameraValueDepthOfField, value)

	fixed get_focal_depth():
		return CameraInfoGetValue(this, c_cameraValueFocalDepth)

	void set_focal_depth(fixed value):
		CameraInfoSetValue(this, c_cameraValueFocalDepth, value)

	fixed get_fall_off_start():
		return CameraInfoGetValue(this, c_cameraValueFalloffStart)

	void set_fall_off_start(fixed value):
		CameraInfoSetValue(this, c_cameraValueFalloffStart, value)

	fixed get_fall_off_end():
		return CameraInfoGetValue(this, c_cameraValueFalloffEnd)

	void set_fall_off_end(fixed value):
		CameraInfoSetValue(this, c_cameraValueFalloffEnd, value)

	point get_target():
		return CameraInfoGetTarget(this)

	void set_target(point value):
		CameraInfoSetTarget(this, value)

	private ctor camerainfo_base():