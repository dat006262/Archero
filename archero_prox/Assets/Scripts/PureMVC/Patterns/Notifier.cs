




using PureMVC.Interfaces;
using System;

namespace PureMVC.Patterns
{
	public class Notifier : INotifier
	{
		protected const string MULTITON_MSG = "Multiton key for this Notifier not yet initialized!";

		public string MultitonKey
		{
			get;
			protected set;
		}

		protected IFacade Facade
		{
			get
			{
				if (MultitonKey == null)
				{
					throw new Exception("Multiton key for this Notifier not yet initialized!");
				}
				return PureMVC.Patterns.Facade.GetInstance(MultitonKey);
			}
		}

		public virtual void SendNotification(string notificationName)
		{
			Facade.SendNotification(notificationName);
		}

		public virtual void SendNotification(string notificationName, object body)
		{
			Facade.SendNotification(notificationName, body);
		}

		public virtual void SendNotification(string notificationName, object body, string type)
		{
			Facade.SendNotification(notificationName, body, type);
		}

		public void InitializeNotifier(string key)
		{
			MultitonKey = key;
		}
	}
}
