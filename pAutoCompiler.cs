class pAutoCompiler
{
	const System.String DefaultToolchainBinPath = @"C:\Program Files (x86)\mingw-w64\i686-4.9.2-posix-dwarf-rt_v3-rev1\mingw32\bin";
	public static void Main(System.String[] Args)
	{
		System.String LookupDirectory = System.Environment.CurrentDirectory;
		System.String ToolchainBinDirectory = (Args.Length == 1) ? Args[0] : DefaultToolchainBinPath;
		
		System.Console.WriteLine("Lookup directory: {0}", LookupDirectory);
		System.Console.WriteLine("Toolchain directory: {0}", ToolchainBinDirectory);
		System.Console.WriteLine();
		
		Compiler C = new Compiler(ToolchainBinDirectory);
		bool DoExit = false;
		do
		{
			System.Console.WriteLine("== BUILD START ==");
			Project.Build(LookupDirectory, C);
			System.Console.WriteLine("== BUILD END ==");
			
			System.Console.Write("Press any key to recompile, q to exit: ");
			DoExit = System.Console.ReadLine() == "q";
		}
		while(!DoExit);
	}
}