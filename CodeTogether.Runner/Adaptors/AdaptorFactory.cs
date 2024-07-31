using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Runner.Adaptors;

public class AdaptorFactory : IAdaptorFactory
{
	public bool TryGetAdaptor(string key, out IAdaptor adaptor)
	{
		throw new NotImplementedException();
	}
}