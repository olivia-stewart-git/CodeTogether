namespace CodeTogether.Runner.Scaffolds;

public class Scaffold
{
	public string ScaffoldCode { get; }
	public Type[] ReferencedTypes { get; }

	public Scaffold(string scaffoldCode, Type[] referencedTypes)
	{
		ScaffoldCode = scaffoldCode;
		ReferencedTypes = referencedTypes;
	}
}