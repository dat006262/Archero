




using PureMVC.Interfaces;
using System;

namespace PureMVC.Patterns
{
	public class Proxy : Notifier, IProxy, INotifier
	{
		public static string NAME = "Proxy";

		public string ProxyName
		{
			get;
			protected set;
		}

		public object Data
		{
			get;
			set;
		}

		public Action Event_Para0
		{
			get;
			set;
		}

		public Action<object> Event_Para1
		{
			get;
			set;
		}

		public Proxy()
			: this(NAME, null)
		{
		}

		public Proxy(string proxyName)
			: this(proxyName, null)
		{
		}

		public Proxy(string proxyName, object data)
		{
			ProxyName = (proxyName ?? NAME);
			if (data != null)
			{
				Data = data;
			}
		}

		public virtual void OnRegister()
		{
		}

		public virtual void OnRemove()
		{
		}
	}
}
