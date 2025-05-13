




using PureMVC.Interfaces;
using System.Collections.Generic;

public class MaskModuleMediator : WindowMediator, IMediator, INotifier
{
	public override List<string> OnListNotificationInterests => new List<string>();

	public MaskModuleMediator()
		: base("MaskUIPanel")
	{
	}

	protected override void OnRegisterOnce()
	{
	}

	protected override void OnRegisterEvery()
	{
		_MonoView.transform.SetAsLastSibling();
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
