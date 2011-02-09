# generates all the classes necessary for galaxy interop

# print the merge message

print 'Merging native lib'

# merge the native lib first

import merge_native_lib

# print start message
print 'Starting class generation'

# start constant generation by importing generate_constants.py
import generate_constants

# print separator
print '\n'
print '--------------------------'
print '\n'

# start function generation by importing generate_functions.py
import generate_functions