using System.Reflection;
using CodeTogether.Runner.Engine;

namespace CodeTogether.Runner.Adaptors;

public interface ISubmissionExecutor
{
	ExecutionResultModel Execute(Assembly targetAssembly);
	IEnumerable<Type> GetAddTypes();
}