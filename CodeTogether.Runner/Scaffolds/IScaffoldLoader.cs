namespace CodeTogether.Runner.Scaffolds;

public interface IScaffoldLoader
{
	string LoadScaffold(string scaffoldName);
	List<string> LoadAllScaffolds();
}