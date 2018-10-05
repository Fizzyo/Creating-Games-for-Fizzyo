import cx_Freeze

executables = [cx_Freeze.Executable("main.py")]

cx_Freeze.setup(
	name="Monty Hall",
	options={"build_exe":{"packages":["pygame"],"include_files":["door1.jpg","door2.jpg","door3.jpg","opencar.jpg","opengoat.jpg"]}},
	description = "Monty Hall",
	executables = executables
	)