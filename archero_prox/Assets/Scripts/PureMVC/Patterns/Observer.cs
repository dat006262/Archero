




using PureMVC.Interfaces;
using System;

namespace PureMVC.Patterns
{
	[Serializable]
	public class Observer : IObserver
	{
		public string NotifyMethod
		{
			private get;
			set;
		}

		public object NotifyContext
		{
			private get;
			set;
		}

		public Observer(string notifyMethod, object notifyContext)
		{
			NotifyMethod = notifyMethod;
			NotifyContext = notifyContext;
		}

		public void NotifyObserver(INotification notification)
		{
			object notifyContext;
			lock (this)
			{
				notifyContext = NotifyContext;
			}
			if (notifyContext is IMediator)
			{
				IMediator mediator = notifyContext as IMediator;
				mediator.HandleNotification(notification);
			}
			else if (notifyContext is IController)
			{
				IController controller = notifyContext as IController;
				controller.ExecuteCommand(notification);
			}
		}

		public bool CompareNotifyContext(object obj)
		{
			lock (this)
			{
				return NotifyContext.Equals(obj);
			}
		}
	}
}
