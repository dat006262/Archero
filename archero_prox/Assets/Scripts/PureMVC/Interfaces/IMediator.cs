




using System.Collections.Generic;

namespace PureMVC.Interfaces
{
	public interface IMediator : INotifier
	{
		string MediatorName
		{
			get;
		}

		object ViewComponent
		{
			get;
			set;
		}

		IEnumerable<string> ListNotificationInterests
		{
			get;
		}

		void HandleNotification(INotification notification);

		void OnRegister();

		void OnRemove();

		object GetEvent(string eventName);
	}
}
