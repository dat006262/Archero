




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

namespace PureMVC.Core
{
	public class Controller : IController, IDisposable
	{
		protected string m_multitonKey;

		private IView m_view;

		private readonly IDictionary<string, object> m_commandMap;

		protected static readonly IDictionary<string, IController> m_instanceMap;

		public const string DEFAULT_KEY = "PureMVC";

		protected const string MULTITON_MSG = "Controller instance for this Multiton key already constructed!";

		public static IController Instance => GetInstance("PureMVC");

		public IEnumerable<string> ListNotificationNames => m_commandMap.Keys;

		public Controller(string key)
		{
			m_multitonKey = key;
			m_commandMap = new Dictionary<string, object>();
			if (m_instanceMap.ContainsKey(key))
			{
				throw new Exception("Controller instance for this Multiton key already constructed!");
			}
			m_instanceMap[key] = this;
			InitializeController();
		}

		public Controller()
			: this("PureMVC")
		{
		}

		static Controller()
		{
			m_instanceMap = new Dictionary<string, IController>();
		}

		public void ExecuteCommand(INotification notification)
		{
			if (!m_commandMap.ContainsKey(notification.Name))
			{
				return;
			}
			object obj = m_commandMap[notification.Name];
			Type type = obj as Type;
			ICommand command;
			if (type != null)
			{
				object obj2 = Activator.CreateInstance(type);
				command = (obj2 as ICommand);
				if (command == null)
				{
					return;
				}
			}
			else
			{
				command = (obj as ICommand);
				if (command == null)
				{
					return;
				}
			}
			command.InitializeNotifier(m_multitonKey);
			command.Execute(notification);
		}

		public void RegisterCommand(string notificationName, Type commandType)
		{
			if (!m_commandMap.ContainsKey(notificationName))
			{
				m_view.RegisterObserver(notificationName, new Observer("executeCommand", this));
			}
			m_commandMap[notificationName] = commandType;
		}

		public void RegisterCommand(string notificationName, ICommand command)
		{
			if (!m_commandMap.ContainsKey(notificationName))
			{
				m_view.RegisterObserver(notificationName, new Observer("executeCommand", this));
			}
			command.InitializeNotifier(m_multitonKey);
			m_commandMap[notificationName] = command;
		}

		public bool HasCommand(string notificationName)
		{
			return m_commandMap.ContainsKey(notificationName);
		}

		public object RemoveCommand(string notificationName)
		{
			if (!m_commandMap.ContainsKey(notificationName))
			{
				return null;
			}
			m_view.RemoveObserver(notificationName, this);
			object result = m_commandMap[notificationName];
			m_commandMap.Remove(notificationName);
			return result;
		}

		public static IController GetInstance()
		{
			return GetInstance("PureMVC");
		}

		public static IController GetInstance(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IController value))
			{
				return value;
			}
			value = new Controller(key);
			m_instanceMap[key] = value;
			return value;
		}

		private void InitializeController()
		{
			m_view = View.GetInstance(m_multitonKey);
		}

		public void Dispose()
		{
			RemoveController(m_multitonKey);
			m_commandMap.Clear();
		}

		public static void RemoveController(string key)
		{
			if (m_instanceMap.TryGetValue(key, out IController value))
			{
				m_instanceMap.Remove(key);
				value.Dispose();
			}
		}
	}
}
