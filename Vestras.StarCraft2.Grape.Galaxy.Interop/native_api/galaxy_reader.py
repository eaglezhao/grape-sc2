# provides a galaxy code reader that reads constants and function from galaxy code

from time import time
t1 = time()

import constant
import function
import sys
import re
import sqlite3

constant_elements = []
function_elements = []

E_TYPE_FUNCTION = 1
E_TYPE_NATIVE = 2
E_TYPE_GLOBAL = 3
E_TYPE_CONST = 4

elements = []

code = ''
with open("NativeLib.galaxy") as file:
	code += file.read()

def format_native(data, etype = E_TYPE_NATIVE):
	data['element_type'] = etype
	data['value'] = ''
	elements.append(data)

def format_func(data):
	format_native(data)

def format_global(data, etype = E_TYPE_GLOBAL):
	data['element_type'] = etype
	if data.has_key('value') and data['value'] != None:
        # string values have " in them, so gotta handle those
        #data['value'] = data['value'].replace('"', '&quot;')
		pass
	else:
		data['value'] = ''

	elements.append(data)

def format_const(data):
	return format_global(data, E_TYPE_CONST)

re_comment  = re.compile(r'//.*')
re_include  = re.compile(r'include\s+"[\s\S]*?(?<!\\)"\s*')
re_native   = re.compile(r'\s*native\s+(?P<type>[\w*\[\]\s]+)\s+(?P<name>\w+)\s*\((?P<args>[^)]*)\)\s*;\s*')
re_proto    = re.compile(r'\s*(?P<type>[\w*\[\]\s]+)\s+(?P<name>\w+)\s*\((?P<args>[^)]*)\)\s*;\s*')
re_func     = re.compile(r'\s*(?P<type>[\w*\[\]\s]+)\s+(?P<name>\w+)\s*\((?P<args>[^)]*)\)\s*{')
re_const    = re.compile(r'\s*const\s+(?P<type>[\w*\[\]\s]+)\s+(?P<name>\w+)\s*=\s*(?P<value>("[^"]*"|[^";]+)+);\s*')
re_glob     = re.compile(r'\s*(?P<type>[\w*\[\]\s]+)\s+(?P<name>\w+)\s*(=\s*(?P<value>(\"[\\s\\S]*?(?<!\\)\"|[^";]+)+))?;\s*')
re_type     = re.compile(r'\s*type\s+(?P<name>[\w*\s]+);\s*');
re_space    = re.compile(r'\s+')

# strip includes
code = re_include.sub('', code)

# globals
original_code = re.split(r'\n', code)
current_line = 0
result = ''

# strip comments
code = re_comment.sub('', code) + '\n'

# Need to get the function body for the declaration.
def get_func_line_count(index):
    # Find the ending }. There might be blocks inside
    # the function, so gotta do this weirdo stuff to
    # skip them.
    count = 1
    index += 1
    while count != 0:
        open = code.find('{', index)
        close = code.find('}', index)
        
        if open < close and open != -1:
            count += 1
            index = open + 1
        else:
            count -= 1
            index = close + 1
    
    index = code.find('\n', index) + 1
    
    # And finally return the line count.
    c = code[:index]
    return (c.count('\n'), index)

def handle_if_matches(pattern, format):
    global code, original_code, current_line, result
    m = pattern.match(code)
    
    if m:
        # Get the length for the declaration.
        l = len(m.group(0))
        
        # Get the line count for the declaration.
        if pattern == re_func:
            # Functions will also receive a new length as function body
            # isn't matched by the regular expression.
            (line_count, l) = get_func_line_count(l)
        else:
            line_count = m.group(0).count('\n')
        
        # Clean the data.
        data = m.groupdict()
        data['decl'] = '\n'.join(original_code[current_line:current_line+line_count]).rstrip().lstrip('\n')
        
        if data.has_key('args') and data['args'] != None:
            # Remove all double spaces and whitespace before and after.
            data['args'] = re_space.sub(' ', data['args']).strip()
        else:
            data['args'] = ''
        
        if data.has_key('type'):
            data['type'] = data['type'].strip()
        
        # Format the data and update code and current line.
        format(data)
        code = code[l:]
        current_line += line_count
        return True
    
    # Didn't match!
    return False

while not (len(code) == 0 or code.isspace()):
    if handle_if_matches(re_native, format_native) or \
       handle_if_matches(re_func, format_func)     or \
       handle_if_matches(re_const, format_const)   or \
       handle_if_matches(re_glob, format_global):
        pass
    else:
        m = re_proto.match(code)
        if m:
            code = code[len(m.group(0)):]
            current_line += m.group(0).count('\n')
            continue
        
        print current_line
        print code[:150]
        exit()

for e in elements:
	name = e['name']
	decl = e['decl']
	type = e['type']
	args = e['args']
	value = e['value']
	element_type = e['element_type']
	if element_type == E_TYPE_NATIVE or element_type == E_TYPE_FUNCTION:
		f = function.Function()
		f.set_name(name)
		f.set_type(type)
		f.set_params(args)
		function_elements.append(f)
	else:
		 if element_type == E_TYPE_CONST or element_type == E_TYPE_GLOBAL:
			c = constant.Constant()
			c.set_name(name)
			c.set_type(type)
			constant_elements.append(c)

t2 = time()
print t2 - t1