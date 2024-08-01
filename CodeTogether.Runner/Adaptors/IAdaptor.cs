using CodeTogether.Runner.Engine;

namespace CodeTogether.Runner.Adaptors;

public interface IAdaptor
{
	ExecutionResultModel Execute();
	IEnumerable<Type> GetAddTypes();
}