/*
 * Grape stdlib - system package - contains base classes for interop with SC2
 * Copyright (c) 2011 Grape Team. All rights reserved.
 * 
 * The Grape programming language and stdlib are released under the BSD license.
 */

 package system

 static class game
	static void display(string text)
		UIDisplayMessage(playergroup.all_players, c_messageAreaAll, StringToText(text))
	end
 end