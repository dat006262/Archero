




using Dxx.Util;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class SettingUICtrl : MediatorCtrlBase
{
	public GameObject loginObj;

	public GameObject logoutObj;

	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public Text Text_Music;

	public Text Text_Sound;

	public Text Text_Language;

	public Text Text_Producter;

	public Text Text_Login;

	public Text Text_ButtonLogin;

	public Text Text_Logout;

	public Text Text_Quality;

	public ButtonCtrl Button_Producter;

	public ButtonCtrl Button_Logout;

	public ButtonCtrl Button_Login;

	public SettingMusicCtrl mMusicCtrl;

	public SettingSoundCtrl mSoundCtrl;

	public SettingLanguageCtrl mLanguageCtrl;

	public SettingQualityCtrl mQualityCtrl;

	public Text Text_Version;

	public Text Text_UserID;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Setting);
		};
		Button_Producter.onClick = delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_Producer);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		Button_Logout.onClick = delegate
		{
			LocalSave.Instance.SetUserID(LoginType.eInvalid, string.Empty, string.Empty);
			SdkManager.OnLogin();
		};
		Button_Login.onClick = delegate
		{
		};
	}

	protected override void OnOpen()
	{
		GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
		InitUI();
	}

	private void InitUI()
	{
		LoginType loginType = LocalSave.Instance.GetLoginType();
		if (string.IsNullOrEmpty(LocalSave.Instance.GetUserID()) || loginType == LoginType.eInvalid)
		{
			loginObj.SetActive(value: true);
			logoutObj.SetActive(value: false);
		}
		else
		{
			loginObj.SetActive(value: false);
			logoutObj.SetActive(value: true);
			string text = string.Empty;
			switch (loginType)
			{
			case LoginType.eGP:
				text = GameLogic.Hold.Language.GetLanguageByTID("设置_登录GP");
				break;
			case LoginType.eWeiXin:
				text = GameLogic.Hold.Language.GetLanguageByTID("设置_登录微信");
				break;
			case LoginType.eGameCenter:
				text = GameLogic.Hold.Language.GetLanguageByTID("设置_登录GameCenter");
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				Text_Login.text = GameLogic.Hold.Language.GetLanguageByTID("设置_已登录", text);
			}
		}
		loginObj.SetActive(value: false);
		logoutObj.SetActive(value: false);
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
		if (name != null && name == "PUB_UI_UPDATE_PING")
		{
		}
	}

	public override void OnLanguageChange()
	{
		ulong serverUserID = LocalSave.Instance.GetServerUserID();
		if (serverUserID != 0)
		{
			Text_UserID.text = Utils.FormatString("{0}:{1}", GameLogic.Hold.Language.GetLanguageByTID("设置_玩家ID"), serverUserID);
		}
		else
		{
			Text_UserID.text = string.Empty;
		}
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("设置_标题");
		Text_Music.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音乐");
		Text_Sound.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音效");
		Text_Language.text = GameLogic.Hold.Language.GetLanguageByTID("设置_语言");
		Text_Producter.text = GameLogic.Hold.Language.GetLanguageByTID("设置_制作人员");
		Text_Logout.text = GameLogic.Hold.Language.GetLanguageByTID("设置_退出登录");
		Text_ButtonLogin.text = GameLogic.Hold.Language.GetLanguageByTID("设置_登录");
		Text_Quality.text = GameLogic.Hold.Language.GetLanguageByTID("设置_画面质量");
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("AppVersion");
		string text = Utils.FormatString("{0}:{1}", languageByTID, PlatformHelper.GetAppVersionName().ToString());
		Text_Version.text = text;
		mMusicCtrl.UpdateLanguage();
		mSoundCtrl.UpdateLanguage();
		mLanguageCtrl.UpdateLanguage();
		mQualityCtrl.UpdateLanguage();
	}
}
