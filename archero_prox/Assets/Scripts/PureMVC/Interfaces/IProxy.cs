




using System;

namespace PureMVC.Interfaces
{
	public interface IProxy : INotifier
	{
		string ProxyName
		{
			get;
		}

		object Data
		{
			get;
			set;
		}

		Action Event_Para0
		{
			get;
			set;
		}

		Action<object> Event_Para1
		{
			get;
			set;
		}

		void OnRegister();

		void OnRemove();
	}
}
