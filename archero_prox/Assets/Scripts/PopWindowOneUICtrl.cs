




using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine.UI;

public class PopWindowOneUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Sure;

	public ButtonCtrl Button_Sure;

	public ButtonCtrl Button_Close;

	private PopWindowOneProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		Button_Sure.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_PopWindowOne);
			if (mTransfer != null && mTransfer.callback != null)
			{
				mTransfer.callback();
			}
		};
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_PopWindowOne);
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("PopWindowOneProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("PopWindowOneUICtrl", "OnOpen PopWindowOneProxy is null");
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("PopWindowOneUICtrl", "OnOpen PopWindowOneProxy.Data is null");
			return;
		}
		mTransfer = (proxy.Data as PopWindowOneProxy.Transfer);
		if (mTransfer == null)
		{
			SdkManager.Bugly_Report("PopWindowOneUICtrl", "OnOpen PopWindowOneProxy.Data is null");
			return;
		}
		Button_Close.gameObject.SetActive(mTransfer.showclosebutton);
		InitUI();
	}

	private void InitUI()
	{
		Text_Title.text = mTransfer.title;
		Text_Content.text = mTransfer.content;
		Text_Sure.text = mTransfer.sure;
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
