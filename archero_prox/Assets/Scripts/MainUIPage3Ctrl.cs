




using PureMVC.Interfaces;
using UnityEngine.UI;

public class MainUIPage3Ctrl : MediatorCtrlBase
{
	public Text Text_Soon;

	protected override void OnInit()
	{
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
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
	}

	public override void OnLanguageChange()
	{
		Text_Soon.text = GameLogic.Hold.Language.GetLanguageByTID("Main_CommingSoon");
	}
}
