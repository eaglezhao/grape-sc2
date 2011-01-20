# represents a constant read by the native galaxy code reader

class Constant:
	def __init__(self):
		self.name = ""
		self.type = ""

	def set_name(self, newName):
		self.name = newName

	def get_name(self):
		return self.name

	def set_type(self, newType):
		self.type = newType

	def get_type(self):
		return self.type