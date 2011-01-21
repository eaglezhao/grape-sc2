package grape.sample

import system
import system.collections
import system.utils

// mod_base is a built-in class that provides all the base functions for a SC2 mod
// for example, the user can override main which is run on map initialization
class mod inherits mod_base
	string args

    override void main(string args)
        string[100] args_array = args.split(" ")

        if args_array.null_or_empty()
            return exit_code_failure
        end

        foreach string arg in args_array
            if arg.null_or_empty()
                break
            end

            game.echo(arg)
        end

		// in grape, there are no for loops - I found while loops to be prettier, and more "scripting"-like
		int index = 0
		while index < 10
			game.echo(index)
			index = index + 1
		end

		bool processing_args = false
		bool debug_mode_on = false
		bool is_unit_test = false
		try
			processing_args = true
			switch args_array[0]
				case "debug_mode_on"
					debug_mode_on = true
				end
				case "is_unit_test"
					is_unit_test = true
				end
				default
					// execute default action here
					// this code is run when none of the above cases were handled
				end
			end
		end
		catch exception e
			game.echo("exception caught: " + e.to_string())
		end
		finally
			processing_args = false
		end
    end

	void before_base_initialize()
	end
	
	void after_base_initialize()
	end

	// Constructors/destructors
	ctor mod()
		init this("")
	end

	ctor mod(string args)
		before_base_initialize()
		init base()
		after_base_initialize()

		this.args = args
	end

	dctor mod()
		args.destroy()
	end
end
