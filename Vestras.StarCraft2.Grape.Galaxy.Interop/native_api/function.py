# represents a function read by the native galaxy code reader

class Function:
	def __init__(self):
		self.name = ""
		self.type = ""
		self.params = []

	def set_name(self, newName):
		self.name = newName

	def get_name(self):
		return self.name

	def set_type(self, newType):
		self.type = newType

	def get_type(self):
		return self.type

	def set_params(self, newParams):
		self.params = newParams

	def get_params(self):
		return self.params
