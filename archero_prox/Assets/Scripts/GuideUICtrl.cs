




using PureMVC.Interfaces;

public class GuideUICtrl : MediatorCtrlBase
{
	protected override void OnInit()
	{
	}

	protected override void OnOpen()
	{
	}

	protected override void OnClose()
	{
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
		base.OnHandleNotification(notification);
	}

	public override void OnLanguageChange()
	{
	}
}
