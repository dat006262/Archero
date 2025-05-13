




using PureMVC.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public LanguageInfinity mInfinity;

	public GameObject copyitems;

	private List<string> mList;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Language);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		mInfinity.Init(1);
		mInfinity.updatecallback = UpdateChildCallBack;
		mList = SettingLanguageCtrl.GetLanguageList();
		copyitems.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
		InitUI();
	}

	private void InitUI()
	{
		mInfinity.SetItemCount(mList.Count);
		mInfinity.Refresh();
	}

	private void UpdateChildCallBack(int index, LanguageOneCtrl one)
	{
		one.Init(index, mList[index]);
		one.OnClickButton = OnClickLanguage;
	}

	private void OnClickLanguage(LanguageOneCtrl one)
	{
		GameLogic.Hold.Language.ChangeLanguage(one.mLanguage);
		mInfinity.Refresh();
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("设置_语言");
	}
}
