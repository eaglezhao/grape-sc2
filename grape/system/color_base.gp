/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 class color_base
	static color from_rgba(fixed r, fixed g, fixed b, fixed a)
		return ColorWithAlpha(r, g, b, a)
	end

	static color from_rgb(fixed r, fixed g, fixed b)
		return Color(r, g, b)
	end

	static color get_diffuse_color_for_team_color(int team_index)
		return ColorFromIndex(team_index, c_teamColorDiffuse)
	end

	static color get_emissive_color_for_team_color(int team_index)
		return ColorFromIndex(team_index, c_teamColorEmissive)
	end

	fixed get_red()
		return ColorGetComponent(this, c_colorComponentRed)
	end

	fixed get_green()
		return ColorGetComponent(this, c_colorComponentGreen)
	end

	fixed get_blue()
		return ColorGetComponent(this, c_colorComponentBlue)
	end

	fixed get_alpha()
		return ColorGetComponent(this, c_colorComponentAlpha)
	end

	color set_red(fixed value)
		return from_rgba(value, get_green(), get_blue(), get_alpha())
	end

	color set_green(fixed value)
		return from_rgba(get_red(), value, get_blue(), get_alpha())
	end

	color set_blue(fixed value)
		return from_rgba(get_red(), get_green(), value, get_alpha())
	end

	color set_alpha(fixed value)
		return from_rgba(get_red(), get_green(), get_blue(), value)
	end

	private ctor color_base()
	end
 end