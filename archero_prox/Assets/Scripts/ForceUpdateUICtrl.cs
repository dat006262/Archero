




using PureMVC.Interfaces;
using UnityEngine.UI;

public class ForceUpdateUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Sure;

	public ButtonCtrl Button_Sure;

	private ChangeAccountProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		Button_Sure.onClick = delegate
		{
			RateUrlManager.OpenAppUrl();
		};
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
		Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗确定");
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("title_warning");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("forceupdate_content");
	}
}
