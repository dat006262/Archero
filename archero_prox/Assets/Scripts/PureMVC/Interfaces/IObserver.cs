




namespace PureMVC.Interfaces
{
	public interface IObserver
	{
		string NotifyMethod
		{
			set;
		}

		object NotifyContext
		{
			set;
		}

		void NotifyObserver(INotification notification);

		bool CompareNotifyContext(object obj);
	}
}
