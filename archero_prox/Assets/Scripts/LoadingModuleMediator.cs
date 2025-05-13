




using PureMVC.Interfaces;
using System.Collections.Generic;

public class LoadingModuleMediator : WindowMediator, IMediator, INotifier
{
	public override List<string> OnListNotificationInterests => new List<string>();

	public LoadingModuleMediator()
		: base("LoadingUIPanel")
	{
	}

	protected override void OnRegisterOnce()
	{
	}

	protected override void OnRegisterEvery()
	{
	}

	protected override void OnRemoveAfter()
	{
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
		}
	}

	protected override void OnLanguageChange()
	{
	}
}
