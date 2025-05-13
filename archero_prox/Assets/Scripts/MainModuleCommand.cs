




using PureMVC.Interfaces;
using PureMVC.Patterns;

public class MainModuleCommand : SimpleCommand
{
	public override void Execute(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
	}
}
