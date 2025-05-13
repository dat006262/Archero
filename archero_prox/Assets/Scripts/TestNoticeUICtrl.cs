




using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class TestNoticeUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Sure;

	public ScrollRectBase mScrolRect;

	public ButtonCtrl Button_Sure;

	protected override void OnInit()
	{
		Button_Sure.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_TestNotice);
		};
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("common_notice");
		Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("popwindow_sure");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("mail_first_test");
		RectTransform content = mScrolRect.content;
		Vector2 sizeDelta = mScrolRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta.x, Text_Content.preferredHeight);
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
	}
}
