# MinGWCppAutoBuilder
Simple program written in C# for automated building of less complicated projects.
Projects are expected to be in following format:

	Top Directory
		Some directories containing non-main .cpp and .hpp files
			Maybe more files?
		Even more directories with classes
		Directory with useless files

		Main1.cpp
		Main2.cpp
		MainWithAnotherName.cpp
		Bin directory (will be automaticaly created)
		BuildProject.bat (see below)

After running BuildProject.bat this program will produce:

	Bin/*.o (for all .cpp files found)
	Bin/Main1.o
	Bin/Main2.o
	Bin/MainWithAnotherName.o
	Bin/Main1.exe
	Bin/Main2.exe
	Bin/MainWithAnotherName.exe

# How to make this projetc from source?
Just run Make.bat which will produce pAutoCompiler.exe. If any errors occurs modify the bat and provide valid path to c# compiler (csc.exe)

# How to use?
Create BuildProject.bat (or any other name) in directory containing yout project files. Put there <"Path to pAutoCompiler.exe" "Path to MinGW\bin directory">. See BuildProject.sample.bat for details. Also see top of this README for details.
Run created bat.

# How does it work?
1. Looks for all .cpp files in directory with .bat - those are expected to have definition of main() inside.
2. Looks for all .cpp files recursively from this directory (excluding files from point 1, those are expected to be objects).
3. Compiles all files from points 1 and 2 to objects in Bin directory. Compiles file only if object is not present or source file (or corresponding header with .hpp extension) is newer then object.
4. For each file from point 1 creates executable and links all objectst from point 2 with it. Produces exe in Bin directory with the same name as 'Main File'.