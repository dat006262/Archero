




using PureMVC.Interfaces;
using UnityEngine.UI;

public class BoxChooseUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_BoxChoose);
		};
		Button_Shadow.onClick = Button_Close.onClick;
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("宝箱_标题");
	}
}
