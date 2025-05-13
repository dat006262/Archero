




using PureMVC.Core;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

namespace PureMVC.Patterns
{
	public class Facade : Notifier, IFacade, INotifier, IDisposable
	{
		protected IController m_controller;

		protected IModel m_model;

		protected IView m_view;

		protected static readonly IDictionary<string, IFacade> m_instanceMap;

		public const string DEFAULT_KEY = "PureMVC";

		protected new const string MULTITON_MSG = "Facade instance for this Multiton key already constructed!";

		public static IFacade Instance => GetInstance("PureMVC");

		public static IEnumerable<string> ListCore => m_instanceMap.Keys;

		public Facade(string key)
		{
			InitializeNotifier(key);
			m_instanceMap[key] = this;
			InitializeFacade();
		}

		public Facade()
			: this("PureMVC")
		{
		}

		static Facade()
		{
			m_instanceMap = new Dictionary<string, IFacade>();
		}

		public void RegisterProxy(IProxy proxy)
		{
			m_model.RegisterProxy(proxy);
		}

		public IProxy RetrieveProxy(string proxyName)
		{
			return m_model.RetrieveProxy(proxyName);
		}

		public IProxy RemoveProxy(string proxyName)
		{
			return m_model.RemoveProxy(proxyName);
		}

		public bool HasProxy(string proxyName)
		{
			return m_model.HasProxy(proxyName);
		}

		public void RegisterCommand(string notificationName, Type commandType)
		{
			m_controller.RegisterCommand(notificationName, commandType);
		}

		public void RegisterCommand(string notificationName, ICommand command)
		{
			m_controller.RegisterCommand(notificationName, command);
		}

		public object RemoveCommand(string notificationName)
		{
			return m_controller.RemoveCommand(notificationName);
		}

		public bool HasCommand(string notificationName)
		{
			return m_controller.HasCommand(notificationName);
		}

		public void RegisterMediator(IMediator mediator)
		{
			m_view.RegisterMediator(mediator);
		}

		public IMediator RetrieveMediator(string mediatorName)
		{
			return m_view.RetrieveMediator(mediatorName);
		}

		public IMediator RemoveMediator(string mediatorName)
		{
			return m_view.RemoveMediator(mediatorName);
		}

		public bool HasMediator(string mediatorName)
		{
			return m_view.HasMediator(mediatorName);
		}

		public void NotifyObservers(INotification notification)
		{
			m_view.NotifyObservers(notification);
		}

		public override void SendNotification(string notificationName)
		{
			NotifyObservers(new Notification(notificationName));
		}

		public override void SendNotification(string notificationName, object body)
		{
			NotifyObservers(new Notification(notificationName, body));
		}

		public override void SendNotification(string notificationName, object body, string type)
		{
			NotifyObservers(new Notification(notificationName, body, type));
		}

		public static IFacade GetInstance()
		{
			return GetInstance("PureMVC");
		}

		public static IFacade GetInstance(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IFacade value))
			{
				return value;
			}
			value = new Facade(key);
			m_instanceMap[key] = value;
			return value;
		}

		public static bool HasCore(string key)
		{
			return m_instanceMap.ContainsKey(key);
		}

		public static void RemoveCore(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IFacade value))
			{
				m_instanceMap.Remove(key);
				value.Dispose();
				Model.RemoveModel(key);
				Controller.RemoveController(key);
				View.RemoveView(key);
			}
		}

		public void Dispose()
		{
			m_view = null;
			m_model = null;
			m_controller = null;
			m_instanceMap.Remove(base.MultitonKey);
		}

		public static void BroadcastNotification(INotification notification)
		{
			IEnumerator<KeyValuePair<string, IFacade>> enumerator = m_instanceMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.NotifyObservers(notification);
			}
		}

		public static void BroadcastNotification(string notificationName)
		{
			BroadcastNotification(new Notification(notificationName));
		}

		public static void BroadcastNotification(string notificationName, object body)
		{
			BroadcastNotification(new Notification(notificationName, body));
		}

		public static void BroadcastNotification(string notificationName, object body, string type)
		{
			BroadcastNotification(new Notification(notificationName, body, type));
		}

		protected virtual void InitializeFacade()
		{
			InitializeModel();
			InitializeController();
			InitializeView();
		}

		protected virtual void InitializeController()
		{
			if (m_controller == null)
			{
				m_controller = Controller.GetInstance(base.MultitonKey);
			}
		}

		protected virtual void InitializeModel()
		{
			if (m_model == null)
			{
				m_model = Model.GetInstance(base.MultitonKey);
			}
		}

		protected virtual void InitializeView()
		{
			if (m_view == null)
			{
				m_view = View.GetInstance(base.MultitonKey);
			}
		}
	}
}
