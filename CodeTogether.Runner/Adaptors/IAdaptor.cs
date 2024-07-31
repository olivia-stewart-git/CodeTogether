using CodeTogether.Runner.Engine;

namespace CodeTogether.Runner.Adaptors;

public interface IAdaptor
{
	ExecutionResult Execute();
	IEnumerable<Type> GetAddTypes();
}