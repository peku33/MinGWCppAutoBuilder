# MinGWCppAutoBuilder
Simple program written in C# for automated building of less complicated projects

# How to build?
Just run Make.bat which will produce pAutoCompiler.exe

# How to use?
Create BuildProject.bat (or any other name) in directory containing yout project files. Put there <"Path to pAutoCompiler.exe" "Path to MinGW\bin directory">. See BuildProject.sample.bat for details.
Run

# How does it work?
1. Looks for all .cpp files in directory with .bat - those are expected to have definition of main() inside.
2. Looks for all .cpp files recursively from this directory (excluding files from point 1, those are expected to be objects).
3. Compiles all files from points 1 and 2 to objects in Bin directory. Compiles file only if object is not present or source file (or corresponding header with .hpp extension) is newer then object.
4. For each file from point 1 creates executable and links all objectst from point 2 with it. Produces exe in Bin directory with the same name as 'Main File'.