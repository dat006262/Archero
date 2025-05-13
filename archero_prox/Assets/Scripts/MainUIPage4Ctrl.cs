




using Dxx.Util;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPage4Ctrl : MediatorCtrlBase
{
	public GameObject window;

	public RectTransform titleparent;

	public Text Text_Setting;

	public Text Text_UserID;

	public Text Text_Version;

	public SettingMusicCtrl mMusicCtrl;

	public SettingSoundCtrl mSoundCtrl;

	public SettingLanguageCtrl mLanguageCtrl;

	public SettingQualityCtrl mQualityCtrl;

	public SettingProducterCtrl mProducterCtrl;

	public SettingReportCtrl mReportCtrl;

	private bool bOpened;

	private bool userid_showlong = true;

	protected override void OnInit()
	{
		float fringeHeight = PlatformHelper.GetFringeHeight();
		RectTransform rectTransform = titleparent;
		Vector2 anchoredPosition = titleparent.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y + fringeHeight);
		window.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		bOpened = true;
		window.SetActive(value: true);
		InitUI();
	}

	private void InitUI()
	{
		OnLanguageChange();
	}

	protected override void OnClose()
	{
		bOpened = false;
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	private void update_userid()
	{
		if (LocalSave.Instance.GetServerUserID() != 0)
		{
			string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("设置_玩家ID");
			Text_UserID.text = Utils.FormatString("{0}: {1:D4}", languageByTID, LocalSave.Instance.GetServerUserIDSub());
		}
		else
		{
			Text_UserID.text = string.Empty;
		}
	}

	public override void OnLanguageChange()
	{
		Text_Setting.text = GameLogic.Hold.Language.GetLanguageByTID("设置_标题");
		mMusicCtrl.UpdateLanguage();
		mSoundCtrl.UpdateLanguage();
		mLanguageCtrl.UpdateLanguage();
		mQualityCtrl.UpdateLanguage();
		mProducterCtrl.UpdateLanguage();
		mReportCtrl.UpdateLanguage();
		update_userid();
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("AppVersion");
		string text = Utils.FormatString("{0}: {1}", languageByTID, PlatformHelper.GetAppVersionName().ToString());
		Text_Version.text = text;
	}
}
