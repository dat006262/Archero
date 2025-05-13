




using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Empty;

	public ButtonCtrl Button_Close;

	public MailInfinity mInfinity;

	public RectTransform window;

	public GameObject copyitems;

	private List<CMailInfo> mList;

	protected override void OnInit()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		RectTransform rectTransform2 = window;
		Vector2 sizeDelta = window.sizeDelta;
		float x = sizeDelta.x;
		Vector2 sizeDelta2 = rectTransform.sizeDelta;
		float y = sizeDelta2.y;
		Vector2 anchoredPosition = window.anchoredPosition;
		rectTransform2.sizeDelta = new Vector2(x, y + anchoredPosition.y);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Mail);
		};
		mInfinity.initDisplayCount = 10;
		mInfinity.Init(1);
		mInfinity.updatecallback = UpdateChildCallBack;
		copyitems.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		UpdateList();
	}

	private void UpdateList()
	{
		mList = LocalSave.Instance.Mail.list;
		int count = mList.Count;
		EmptyShow(count == 0);
		mInfinity.SetItemCount(count);
		mInfinity.Refresh();
	}

	private void UpdateChildCallBack(int index, MailOneCtrl one)
	{
		one.Init(index, mList[index]);
		one.OnClickButton = OnClickOpen;
	}

	private void OnClickOpen(int index, MailOneCtrl one)
	{
		Facade.Instance.RegisterProxy(new MailInfoProxy(new MailInfoProxy.Transfer
		{
			data = mList[index],
			ctrl = one
		}));
		WindowUI.ShowWindow(WindowID.WindowID_MailInfo);
	}

	private void EmptyShow(bool value)
	{
		if ((bool)Text_Empty)
		{
			Text_Empty.gameObject.SetActive(value);
		}
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
		string name = notification.Name;
		object body = notification.Body;
		if (name != null && name == "MailUI_MailUpdate")
		{
			UpdateList();
		}
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("邮件标题");
		Text_Empty.text = GameLogic.Hold.Language.GetLanguageByTID("Main_MailEmpty");
	}
}
