




using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine.UI;

public class PopWindowUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Sure;

	public Text Text_Refuse;

	public ButtonCtrl Button_Sure;

	public ButtonCtrl Button_Refuse;

	private PopWindowProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		Button_Refuse.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_PopWindow);
		};
		Button_Sure.onClick = delegate
		{
			Button_Refuse.onClick();
			if (mTransfer != null && mTransfer.callback != null)
			{
				mTransfer.callback();
			}
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("PopWindowProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen PopWindowProxy is null");
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("ChangeAccountUICtrl", "OnOpen PopWindowProxy.Data is null");
			return;
		}
		mTransfer = (proxy.Data as PopWindowProxy.Transfer);
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
		Text_Title.text = mTransfer.title;
		Text_Content.text = mTransfer.content;
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
		Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("popwindow_sure");
		Text_Refuse.text = GameLogic.Hold.Language.GetLanguageByTID("popwindow_cancel");
	}
}
