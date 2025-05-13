




namespace PureMVC.Interfaces
{
	public interface INotifier
	{
		string MultitonKey
		{
			get;
		}

		void SendNotification(string notificationName);

		void SendNotification(string notificationName, object body);

		void SendNotification(string notificationName, object body, string type);

		void InitializeNotifier(string key);
	}
}
