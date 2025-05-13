




using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using UnityEngine.UI;

public class ServerAssertUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	private ServerAssertProxy.Transfer mTransfer;

	protected override void OnInit()
	{
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("ServerAssertProxy");
		if (proxy == null || proxy.Data == null)
		{
			SdkManager.Bugly_Report("ServerAssertUICtrl", "proxy is null.");
		}
		if (!(proxy.Data is ServerAssertProxy.Transfer))
		{
			SdkManager.Bugly_Report("ServerAssertUICtrl", "proxy.Data is not a ServerAssertProxy.Transfer.");
		}
		mTransfer = (proxy.Data as ServerAssertProxy.Transfer);
		if (mTransfer == null || mTransfer.assertendtime == 0)
		{
			SdkManager.Bugly_Report("ServerAssertUICtrl", "mTransfer is invalid.");
		}
		InitUI();
	}

	private void InitUI()
	{
		string empty = string.Empty;
		DateTime d = new DateTime(mTransfer.assertendtime);
		DateTime now = DateTime.Now;
		TimeSpan timeSpan = d - now;
		if (timeSpan.Days >= 1)
		{
			empty = GameLogic.Hold.Language.GetLanguageByTID("time_day", timeSpan.Days);
		}
		else if (timeSpan.Hours >= 1)
		{
			empty = GameLogic.Hold.Language.GetLanguageByTID("time_hour", timeSpan.Hours);
		}
		else
		{
			int num = MathDxx.Clamp(timeSpan.Minutes, 10, timeSpan.Minutes);
			empty = GameLogic.Hold.Language.GetLanguageByTID("time_hour", num);
		}
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("serverassert_content", empty);
		Text_Content.text = languageByTID;
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("serverassert_title");
	}
}
