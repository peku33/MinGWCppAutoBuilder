class Compiler
{
	const System.String EnvVariableName = "MinGW_bin";
	
	const System.String CompilerExecutable = "g++.exe";
	private System.String ToolchainBinDirectory;
	public Compiler(System.String ToolchainBinDirectory)
	{
		if(ToolchainBinDirectory.Length == 0)
		{
			//Znajdź ścieżkę do kompilatora MinGW
			System.String FromEnv = System.Environment.GetEnvironmentVariable(EnvVariableName);
			if(FromEnv == null)
				throw new System.Exception(System.String.Format("Env variable {0} not found", EnvVariableName));
			else
				ToolchainBinDirectory = FromEnv;
		}
		else
		{
			this.ToolchainBinDirectory = ToolchainBinDirectory;
		}
	}
	
	public bool Compile(System.String SourcePath, System.String ObjectPath)
	{
		if(!ShouldCompile(SourcePath, ObjectPath))
			return true;
		
		System.Console.WriteLine("[COMPILER] {0}", System.IO.Path.GetFileName(ObjectPath));
		return Exec(ToolchainBinDirectory, CompilerExecutable, System.String.Format("-Wall -Wextra -std=c++11 -O2 -ffunction-sections -fdata-sections -c -o \"{0}\" \"{1}\"", ObjectPath, SourcePath));
	}
	private bool ShouldCompile(System.String SourcePath, System.String ObjectPath)
	{
		if(!System.IO.File.Exists(ObjectPath))
			return true;
		
		System.IO.FileInfo SourcePathInfo = new System.IO.FileInfo(SourcePath);
		System.IO.FileInfo ObjectPathInfo = new System.IO.FileInfo(ObjectPath);
		if(SourcePathInfo.LastWriteTime > ObjectPathInfo.LastWriteTime)
			return true;
		
		System.String HeaderPath = System.IO.Path.ChangeExtension(SourcePath, ".hpp");
		if(System.IO.File.Exists(HeaderPath))
		{
			System.IO.FileInfo HeaderPathInfo = new System.IO.FileInfo(HeaderPath);
			if(HeaderPathInfo.LastWriteTime > ObjectPathInfo.LastWriteTime)
				return true;
		}
		return false;
	}
	
	
	public bool Link(System.String[] ObjectPaths, System.String BinaryPath)
	{
		if(ObjectPaths.Length == 0)
			return false;
		
		if(!ShouldLink(ObjectPaths, BinaryPath))
			return true;
		
		System.String ObjectPathsAsString = System.String.Empty;
		foreach(System.String ObjectPath in ObjectPaths)
		{
			if(ObjectPathsAsString.Length != 0)
				ObjectPathsAsString += " ";
			
			ObjectPathsAsString += System.String.Format("\"{0}\"", ObjectPath);
		}
		System.Console.WriteLine("[LINKER] {0}", System.IO.Path.GetFileName(BinaryPath));
		return Exec(ToolchainBinDirectory, CompilerExecutable, System.String.Format("-static -Wl,--gc-sections -s -o \"{0}\" {1}", BinaryPath, ObjectPathsAsString));
	}
	private bool ShouldLink(System.String[] ObjectPaths, System.String BinaryPath)
	{
		if(!System.IO.File.Exists(BinaryPath))
			return true;
		
		System.IO.FileInfo BinaryPathInfo = new System.IO.FileInfo(BinaryPath);
		
		foreach(System.String ObjectPath in ObjectPaths)
		{
			System.IO.FileInfo ObjectPathInfo = new System.IO.FileInfo(ObjectPath);
			if(ObjectPathInfo.LastWriteTime > BinaryPathInfo.LastWriteTime)
				return true;
		}
		
		return false;
	}
	private static bool Exec(System.String WorkingDirectory, System.String FileName, System.String Arguments)
	{
		System.Diagnostics.Process P = new System.Diagnostics.Process();
		P.StartInfo.WorkingDirectory = WorkingDirectory;
		P.StartInfo.FileName = WorkingDirectory + System.IO.Path.DirectorySeparatorChar + FileName;
		P.StartInfo.Arguments = Arguments;
		P.StartInfo.UseShellExecute = false;
		P.Start();
		P.WaitForExit();

		return P.ExitCode == 0;
	}
}