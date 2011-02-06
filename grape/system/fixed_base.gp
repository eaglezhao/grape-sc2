/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

package system

// summary: Represents a decimal value.
class fixed_base
	static int any_precision = c_fixedPrecisionAny

	fixed_base abs()
		return AbsF(this)
	end

	fixed_base pow(fixed_base exponent)
		return Pow(this, exponent)
	end

	fixed_base max(fixed_base value)
		return MaxF(this, value)
	end

	fixed_base min(fixed_base value)
		return MinF(this, value)
	end

	fixed_base sqrt()
		return SquareRoot(this)
	end
	
	fixed_base mod(fixed_base divisor)
		return ModF(this, divisor)
	end	

	fixed_base sin()
		return Sin(this)
	end

	fixed_base cos()
		return Cos(this)
	end

	fixed_base tan()
		return Tan(this)
	end

	fixed_base asin()
		return ASin(this)
	end

	fixed_base acos()
		return ACos(this)
	end

	fixed_base atan()
		return ATan(this)
	end

	static fixed_base random(fixed_base min, fixed_base max)
		return RandomFixed(min, max)
	end

	override bool equals(object value)
		fixed f = value as fixed
		return this == f
	end

	override int get_hash_code()
		return this as int
	end

	override string to_string()
		return FixedToString(this, any_precision)
	end

	string to_string(int_base precision)
		return FixedToString(this, precision)
	end

	private ctor fixed_base()

	end
end