




using System;
using System.Collections.Generic;

namespace PureMVC.Interfaces
{
	public interface IModel : IDisposable
	{
		IEnumerable<string> ListProxyNames
		{
			get;
		}

		void RegisterProxy(IProxy proxy);

		IProxy RetrieveProxy(string proxyName);

		IProxy RemoveProxy(string proxyName);

		bool HasProxy(string proxyName);
	}
}
