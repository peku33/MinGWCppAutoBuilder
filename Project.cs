using System.Linq;

class Project
{
	const System.String BinDirectoryName = "Bin";
	public static bool Build(System.String LookupDirectory, Compiler C)
	{
		System.String BinDirectory = LookupDirectory + System.IO.Path.DirectorySeparatorChar + BinDirectoryName;
		if(!System.IO.Directory.Exists(BinDirectory))
			System.IO.Directory.CreateDirectory(BinDirectory);
		
		System.Collections.Generic.List<System.String> MainSourcePaths = new System.Collections.Generic.List<System.String>(System.IO.Directory.GetFiles(LookupDirectory, "*.cpp", System.IO.SearchOption.TopDirectoryOnly));
		foreach(System.String MainSourcePath in MainSourcePaths)
			System.Console.WriteLine("[MainFile]	{0}", MainSourcePath);
		System.Collections.Generic.List<System.String> CommonSourcePaths = (new System.Collections.Generic.List<System.String>(System.IO.Directory.GetFiles(LookupDirectory, "*.cpp", System.IO.SearchOption.AllDirectories))).Except(MainSourcePaths).ToList();
		foreach(System.String CommonSourcePath in CommonSourcePaths)
			System.Console.WriteLine("[CommonFile]	{0}", CommonSourcePath);
		System.Collections.Generic.List<System.String> CommonObjectPaths = new System.Collections.Generic.List<System.String>();
		
		//Skompiluj wszystkie obiekty
		foreach(System.String CommonSourcePath in CommonSourcePaths)
		{
			System.String CommonObjectPath = CreateObjectPath(LookupDirectory, BinDirectory, CommonSourcePath);
			
			if(!C.Compile(CommonSourcePath, CommonObjectPath))
				return false;
			
			CommonObjectPaths.Add(CommonObjectPath);
		}
		
		//Skompiluj i zlinkuj pliki main
		foreach(System.String MainSourcePath in MainSourcePaths)
		{
			System.String MainObjectPath = CreateObjectPath(LookupDirectory, BinDirectory, MainSourcePath);
			
			if(!C.Compile(MainSourcePath, MainObjectPath))
				return false;
			
			System.Collections.Generic.List<System.String> TargetObjects = new System.Collections.Generic.List<System.String>(CommonObjectPaths);
			TargetObjects.Insert(0, MainObjectPath);
			
			System.String ExeName = System.IO.Path.ChangeExtension(MainObjectPath, ".exe");
			
			if(!C.Link(TargetObjects.ToArray(), ExeName))
				return false;
		}
		
		return true;
	}
	private static System.String CreateObjectPath(System.String TopDirectory, System.String BinDirectory, System.String SourcePath)
	{
		bool FinishingSlash = TopDirectory.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString());
		return System.IO.Path.ChangeExtension(BinDirectory + System.IO.Path.DirectorySeparatorChar + ReplaceNonAlNumChars(SourcePath.StartsWith(TopDirectory) ? SourcePath.Substring(TopDirectory.Length + (FinishingSlash ? 0 : 1)) : SourcePath), ".o");
	}
	private static System.String ReplaceNonAlNumChars(System.String Input)
	{
		System.Text.RegularExpressions.Regex R = new System.Text.RegularExpressions.Regex(@"[^a-zA-Z0-9\.]+");
		return R.Replace(Input, "_");
	}
}