




using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

namespace PureMVC.Core
{
	public class Model : IModel, IDisposable
	{
		protected string m_multitonKey;

		protected IDictionary<string, IProxy> m_proxyMap;

		protected static volatile IModel m_instance;

		protected static readonly IDictionary<string, IModel> m_instanceMap;

		public const string DEFAULT_KEY = "PureMVC";

		protected const string MULTITON_MSG = "Model instance for this Multiton key already constructed!";

		public IEnumerable<string> ListProxyNames => m_proxyMap.Keys;

		public static IModel Instance => GetInstance("PureMVC");

		public Model(string key)
		{
			m_multitonKey = key;
			m_proxyMap = new Dictionary<string, IProxy>();
			if (m_instanceMap.ContainsKey(key))
			{
				throw new Exception("Model instance for this Multiton key already constructed!");
			}
			m_instanceMap[key] = this;
			InitializeModel();
		}

		public Model()
			: this("PureMVC")
		{
		}

		static Model()
		{
			m_instanceMap = new Dictionary<string, IModel>();
		}

		public virtual void RegisterProxy(IProxy proxy)
		{
			proxy.InitializeNotifier(m_multitonKey);
			m_proxyMap[proxy.ProxyName] = proxy;
			proxy.OnRegister();
		}

		public virtual IProxy RetrieveProxy(string proxyName)
		{
			if (!m_proxyMap.ContainsKey(proxyName))
			{
				return null;
			}
			return m_proxyMap[proxyName];
		}

		public virtual bool HasProxy(string proxyName)
		{
			return m_proxyMap.ContainsKey(proxyName);
		}

		public virtual IProxy RemoveProxy(string proxyName)
		{
			IProxy proxy = null;
			if (m_proxyMap.ContainsKey(proxyName))
			{
				proxy = RetrieveProxy(proxyName);
				m_proxyMap.Remove(proxyName);
			}
			proxy?.OnRemove();
			return proxy;
		}

		public static IModel GetInstance(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IModel value))
			{
				return value;
			}
			value = new Model(key);
			m_instanceMap[key] = value;
			return value;
		}

		protected virtual void InitializeModel()
		{
		}

		public static void RemoveModel(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IModel value))
			{
				m_instanceMap.Remove(key);
				value.Dispose();
			}
		}

		public void Dispose()
		{
			RemoveModel(m_multitonKey);
			m_proxyMap.Clear();
		}
	}
}
