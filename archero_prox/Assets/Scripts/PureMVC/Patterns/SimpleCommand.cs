




using PureMVC.Interfaces;

namespace PureMVC.Patterns
{
	public class SimpleCommand : Notifier, ICommand, INotifier
	{
		public virtual void Execute(INotification notification)
		{
		}
	}
}
