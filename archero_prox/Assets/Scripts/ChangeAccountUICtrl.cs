




using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine.UI;

public class ChangeAccountUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Sure;

	public Text Text_Refuse;

	public ButtonCtrl Button_Sure;

	public ButtonCtrl Button_Refuse;

	private ChangeAccountProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		Button_Sure.onClick = delegate
		{
			if (mTransfer != null && mTransfer.callback_sure != null)
			{
				mTransfer.callback_sure();
			}
		};
		Button_Refuse.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_ChangeAccount);
			if (mTransfer != null && mTransfer.callback_confirm != null)
			{
				mTransfer.callback_confirm();
			}
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("ChangeAccountProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen ChangeAccountProxy is null");
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen ChangeAccountProxy.Data is null");
			return;
		}
		mTransfer = (proxy.Data as ChangeAccountProxy.Transfer);
		if (mTransfer == null)
		{
			SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen ChangeAccountProxy.Data is null");
		}
		else
		{
			InitUI();
		}
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
		Text_Refuse.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗取消");
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("title_warning");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("changeaccount_content", LocalSave.Instance.GetUserName());
	}
}
