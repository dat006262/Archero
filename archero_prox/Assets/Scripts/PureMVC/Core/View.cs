




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

namespace PureMVC.Core
{
	public class View : IView, IDisposable
	{
		protected string m_multitonKey;

		protected IDictionary<string, IMediator> m_mediatorMap;

		protected IDictionary<string, IList<IObserver>> m_observerMap;

		protected static volatile IView m_instance;

		protected static readonly IDictionary<string, IView> m_instanceMap;

		public const string DEFAULT_KEY = "PureMVC";

		protected const string MULTITON_MSG = "View instance for this Multiton key already constructed!";

		public IEnumerable<string> ListMediatorNames => m_mediatorMap.Keys;

		public static IView Instance => GetInstance("PureMVC");

		protected View(string key)
		{
			m_multitonKey = key;
			m_mediatorMap = new Dictionary<string, IMediator>();
			m_observerMap = new Dictionary<string, IList<IObserver>>();
			if (m_instanceMap.ContainsKey(key))
			{
				throw new Exception("View instance for this Multiton key already constructed!");
			}
			m_instanceMap[key] = this;
			InitializeView();
		}

		protected View()
			: this("PureMVC")
		{
		}

		static View()
		{
			m_instanceMap = new Dictionary<string, IView>();
		}

		public virtual void RegisterObserver(string notificationName, IObserver observer)
		{
			if (!m_observerMap.ContainsKey(notificationName))
			{
				m_observerMap[notificationName] = new List<IObserver>();
			}
			m_observerMap[notificationName].Add(observer);
		}

		public virtual void NotifyObservers(INotification notification)
		{
			IList<IObserver> list = null;
			if (m_observerMap.ContainsKey(notification.Name))
			{
				IList<IObserver> collection = m_observerMap[notification.Name];
				list = new List<IObserver>(collection);
			}
			if (list != null)
			{
				IEnumerator<IObserver> enumerator = list.GetEnumerator();
				while (enumerator.MoveNext())
				{
					IObserver current = enumerator.Current;
					current.NotifyObserver(notification);
				}
			}
		}

		public virtual void RemoveObserver(string notificationName, object notifyContext)
		{
			if (m_observerMap.ContainsKey(notificationName))
			{
				IList<IObserver> list = m_observerMap[notificationName];
				lock (list)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].CompareNotifyContext(notifyContext))
						{
							list.RemoveAt(i);
							break;
						}
					}
					if (list.Count == 0)
					{
						m_observerMap.Remove(notificationName);
					}
				}
			}
		}

		public virtual void RegisterMediator(IMediator mediator)
		{
			lock (m_mediatorMap)
			{
				if (m_mediatorMap.ContainsKey(mediator.MediatorName))
				{
					return;
				}
				mediator.InitializeNotifier(m_multitonKey);
				m_mediatorMap[mediator.MediatorName] = mediator;
				IEnumerable<string> listNotificationInterests = mediator.ListNotificationInterests;
				IObserver observer = new Observer("HandleNotification", mediator);
				IEnumerator<string> enumerator = listNotificationInterests.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					RegisterObserver(current, observer);
				}
				IObserver observer2 = new Observer("PublicNotification", mediator);
				RegisterObserver("PUB_NOTIFICATION", observer2);
			}
			mediator.OnRegister();
		}

		public virtual IMediator RetrieveMediator(string mediatorName)
		{
			if (!m_mediatorMap.ContainsKey(mediatorName))
			{
				return null;
			}
			return m_mediatorMap[mediatorName];
		}

		public virtual IMediator RemoveMediator(string mediatorName)
		{
			lock (m_mediatorMap)
			{
				if (!m_mediatorMap.ContainsKey(mediatorName))
				{
					return null;
				}
				IMediator mediator = m_mediatorMap[mediatorName];
				IEnumerable<string> listNotificationInterests = mediator.ListNotificationInterests;
				IEnumerator<string> enumerator = listNotificationInterests.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					RemoveObserver(current, mediator);
				}
				RemoveObserver("PUB_NOTIFICATION", mediator);
				m_mediatorMap.Remove(mediatorName);
				mediator.OnRemove();
				return mediator;
			}
		}

		public virtual bool HasMediator(string mediatorName)
		{
			return m_mediatorMap.ContainsKey(mediatorName);
		}

		public static void RemoveView(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IView value))
			{
				m_instanceMap.Remove(key);
				value.Dispose();
			}
		}

		public void Dispose()
		{
			RemoveView(m_multitonKey);
			m_observerMap.Clear();
			m_mediatorMap.Clear();
		}

		public static IView GetInstance()
		{
			return GetInstance("PureMVC");
		}

		public static IView GetInstance(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IView value))
			{
				return value;
			}
			value = new View(key);
			m_instanceMap[key] = value;
			return value;
		}

		protected virtual void InitializeView()
		{
		}
	}
}
