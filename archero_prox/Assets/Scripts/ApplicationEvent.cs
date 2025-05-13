




using Dxx.Net;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationEvent : MonoBehaviour
{
	public static ApplicationEvent Instance;

	private bool isPause = true;

	private bool bFirstInGame = true;

	public static bool bQuit;

	private bool bCheckOnlyMain;

	private bool bOnlyMain;

	public static Action<SdkManager.LoginData> login_callback;

	private int gametime;

	private int currentgametime;

	private int lastgametime;

	public static event Action<bool> OnAppPause;

	public static event Action OnOnlyMain;

	private void Awake()
	{
		Instance = this;
		WindowUI.OnInGameWindowClose += OnWindowClose;
		WindowUI.OnInGameWindowOpen += OnWindowOpen;
	}

	private void OnApplicationFocus(bool value)
	{
	}

	public void on_gamecenter_change(string message)
	{
		Debugger.Log("授权 on_gamecenter_change = " + message);
		if (message == "true" || SdkManager.GameCenter_get_login_count() == 0)
		{
			WindowUI.TryLogin();
		}
		else
		{
			SdkManager.GameCenter_add_login_count();
		}
	}

	public void on_login_callback(string gamecenterid)
	{
		Debugger.Log("授权 on_login_callback = " + gamecenterid);
		if (login_callback != null)
		{
			Debugger.Log("授权 on_login_callback success!!!");
		}
	}

	private void OnApplicationPause(bool value)
	{
		if (bFirstInGame)
		{
			bFirstInGame = false;
			return;
		}
		isPause = value;
		if (ApplicationEvent.OnAppPause != null)
		{
			ApplicationEvent.OnAppPause(isPause);
		}
		if (value)
		{
			CInstance<PlayerPrefsMgr>.Instance.gametime.flush();
			SdkManager.send_app_end();
			return;
		}
		if (GameLogic.InGame && GameLogic.Hold.BattleData.GetMode() != GameMode.eMatchDefenceTime && !GameLogic.Paused && !WindowUI.IsWindowOpened(WindowID.WindowID_Pause))
		{
			WindowUI.ShowWindow(WindowID.WindowID_Pause);
		}
		if (GameLogic.InGame || bOnlyMain)
		{
		}
		SdkManager.send_event("app_start");
	}

	private bool CheckNotice()
	{
		DateTime d = new DateTime(SdkManager.get_first_setup_time());
		DateTime d2 = new DateTime(DateTime.Now.ToUniversalTime().Ticks);
		if ((d2 - d).TotalDays >= 2.0 && !PlayerPrefsEncrypt.HasKey("first_test_notice"))
		{
			PlayerPrefsEncrypt.SetInt("first_test_notice", 0);
			WindowUI.ShowWindow(WindowID.WindowID_TestNotice);
			return true;
		}
		return false;
	}

	private void LateUpdate()
	{
		if (bCheckOnlyMain)
		{
			check_only_main();
		}
		if (GameLogic.InGame)
		{
			gametime = CInstance<PlayerPrefsMgr>.Instance.gametime.get_value();
			currentgametime = (int)Time.realtimeSinceStartup;
			if (currentgametime != lastgametime)
			{
				gametime++;
				lastgametime = currentgametime;
				CInstance<PlayerPrefsMgr>.Instance.gametime.set_value(gametime);
			}
		}
	}

	private void OnWindowOpen(WindowID openID, List<WindowID> holdlist)
	{
		if (openID != WindowID.WindowID_Main)
		{
			bCheckOnlyMain = false;
			bOnlyMain = false;
		}
	}

	private void OnWindowClose(WindowID closeID, List<WindowID> holdlist)
	{
		if (holdlist.Count == 1 && holdlist[0] == WindowID.WindowID_Main)
		{
			bCheckOnlyMain = true;
			bOnlyMain = true;
		}
	}

	private void check_only_main()
	{
		bCheckOnlyMain = false;
		bool flag = false;
		if (!flag)
		{
			flag = GameLogic.Hold.Guide.mCard.CheckGuide();
		}
		if (!flag)
		{
			flag = GameLogic.Hold.Guide.mEquip.CheckGuide();
		}
		if (!flag && ApplicationEvent.OnOnlyMain != null)
		{
			ApplicationEvent.OnOnlyMain();
		}
		Facade.Instance.SendNotification("MainUI_UpdatePage");
	}

	public bool check_app_start()
	{
		bool flag = false;
		LocalSave.Instance.BattleIn_CheckInit();
		if (LocalSave.Instance.BattleIn_GetIn())
		{
			flag = true;
			WindowUI.ShowWindow(WindowID.WindowID_CheckBattleIn);
		}
		if (!flag)
		{
			flag = LocalSave.Instance.Mail.CheckMainPop();
		}
		if (!flag && NetManager.NetTime > 0 && LocalSave.Instance.mHarvest.get_can_reward() && LocalSave.Instance.Card_GetHarvestAvailable())
		{
			WindowUI.ShowWindow(WindowID.WindowID_AdHarvest);
			flag = true;
		}
		return flag;
	}

	private void OnApplicationQuit()
	{
		CInstance<PlayerPrefsMgr>.Instance.gametime.flush();
		SdkManager.send_app_end();
		bQuit = true;
	}
}
