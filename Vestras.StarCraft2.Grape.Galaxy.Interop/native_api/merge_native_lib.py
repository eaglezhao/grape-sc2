# merges all the .galaxy files in the native_lib folder into one file, NativeLib.galaxy

import os
import sys

# the folder that contains all the native files
NATIVE_LIB_FOLDER = "native_lib"

# the output file
NATIVE_LIB_OUTPUT_FILE = "NativeLib.galaxy"

full_folder_path = os.getcwd() + "\\" + NATIVE_LIB_FOLDER
all_content = ""
for p in os.listdir(full_folder_path):
	f = open(full_folder_path + "\\" + p)
	content = f.read()
	f.close()
	all_content += content + "\r\n"

file = open(os.getcwd() + "\\" + NATIVE_LIB_OUTPUT_FILE, 'w')
file.write(all_content)
file.close()