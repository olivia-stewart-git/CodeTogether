namespace CodeTogether.Runner.Adaptors;

public interface IAdaptorFactory
{
	public bool TryGetAdaptor(string key, out IAdaptor adaptor);
}