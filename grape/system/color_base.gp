/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

class color_base:
	static color from_rgba(fixed r, fixed g, fixed b, fixed a):
		return ColorWithAlpha(r, g, b, a)

	static color from_rgb(fixed r, fixed g, fixed b):
		return Color(r, g, b)

	static color get_diffuse_color_for_team_color(int team_index):
		return ColorFromIndex(team_index, c_teamColorDiffuse)

	static color get_emissive_color_for_team_color(int team_index):
		return ColorFromIndex(team_index, c_teamColorEmissive)

	fixed get_red():
		return ColorGetComponent(this, c_colorComponentRed)

	fixed get_green():
		return ColorGetComponent(this, c_colorComponentGreen)

	fixed get_blue():
		return ColorGetComponent(this, c_colorComponentBlue)

	fixed get_alpha():
		return ColorGetComponent(this, c_colorComponentAlpha)

	color set_red(fixed value):
		return from_rgba(value, get_green(), get_blue(), get_alpha())

	color set_green(fixed value):
		return from_rgba(get_red(), value, get_blue(), get_alpha())

	color set_blue(fixed value):
		return from_rgba(get_red(), get_green(), value, get_alpha())

	color set_alpha(fixed value):
		return from_rgba(get_red(), get_green(), get_blue(), value)

	private ctor color_base():