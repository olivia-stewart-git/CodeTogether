using System.Reflection;
using CodeTogether.Runner.Engine;

namespace CodeTogether.Runner.Adaptors;

public interface ISubmissionExecutor
{
	SubmissionResultModel Execute(Assembly targetAssembly);
	//IEnumerable<Type> GetAddTypes(); // TODO: why
}