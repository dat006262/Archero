




using PureMVC.Interfaces;
using System;

namespace PureMVC.Patterns
{
	public class DelegateCommand : Notifier, ICommand, INotifier
	{
		private readonly Action<INotification> m_action;

		public DelegateCommand(Action<INotification> action)
		{
			m_action = action;
		}

		public virtual void Execute(INotification notification)
		{
			m_action(notification);
		}
	}
}
