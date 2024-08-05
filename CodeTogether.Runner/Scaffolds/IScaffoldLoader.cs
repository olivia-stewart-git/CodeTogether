namespace CodeTogether.Runner.Scaffolds;

public interface IScaffoldLoader
{
	Scaffold LoadScaffold(string scaffoldName);
	List<Scaffold> LoadAllScaffolds();
}