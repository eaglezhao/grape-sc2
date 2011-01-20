# generates all the classes necessary for galaxy interop

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