




using Dxx.Net;
using Dxx.Util;
#if ENABLE_FB_SDK
using Facebook.Unity;
#endif
#if ENABLE_GG_GAME_SERVICES
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using System;
using System.Collections.Generic;
using ThinkingGame;
using Umeng;
using UnityEngine;

public class SdkManager
{
	public class LoginData
	{
		public bool bSuccess;

		public string userid;

		public string username;
	}

	public class GAEventType
	{
		public static string DEAD_LAYER = "dead_layer";
	}

	public const string app_start = "app_start";

	public const string app_end = "app_end";

	//private static bool bLogEnable = true;

	private const string BuglyAppID = "8c142b9f46";

	private static void Log(string value)
	{
        UnityEngine.Debug.Log(Utils.FormatString("ShuShu TGAnalysis : {0}", value));
    }

    public static void InitSdks()
	{
		ShuShu_Init();
		Bugly_Init();
		Umeng_Init();
		Facebook_Init();
	}

	public static void set_first_setup_time()
	{
		if (!PlayerPrefsEncrypt.HasKey("set_first_setup_time"))
		{
			PlayerPrefsEncrypt.SetLong("set_first_setup_time", DateTime.Now.ToUniversalTime().Ticks);
		}
	}

	public static long get_first_setup_time()
	{
		return PlayerPrefsEncrypt.GetLong("set_first_setup_time", 0L);
	}

	private static int GetBeforePurchaseHour()
	{
		if (!PlayerPrefsEncrypt.HasKey("before_purchase_time"))
		{
			PlayerPrefsEncrypt.SetLong("before_purchase_time", PlayerPrefsEncrypt.GetLong("set_first_setup_time", 0L));
		}
		long @long = PlayerPrefsEncrypt.GetLong("before_purchase_time", 0L);
		DateTime d = new DateTime(@long);
		DateTime d2 = DateTime.Now.ToUniversalTime();
		TimeSpan timeSpan = d2 - d;
		PlayerPrefsEncrypt.SetLong("before_purchase_time", d2.Ticks);
		return (int)timeSpan.TotalHours;
	}

	public static void send_deadlayer(int layer)
	{
	}

	public static void send_event(string eventId)
	{
		Log(Utils.FormatString("shushu send_event: eventId:{0}", eventId));
		TGAnalytics.TG.track(eventId);
	}

	public static void send_event_http(string step)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("http", properties);
	}

	public static void send_app_end()
	{
		int num = (int)Time.realtimeSinceStartup;
		int num2 = num - CInstance<PlayerPrefsMgr>.Instance.apptime.get_value();
		CInstance<PlayerPrefsMgr>.Instance.apptime.set_value(num);
		Log(Utils.FormatString("shushu send_app_end: duration:{0}", num2));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("duration", num2);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("app_end", properties);
	}

	public static void send_event_page_show(WindowID windowid, string step)
	{
		Log(Utils.FormatString("shushu send_event_page_show: page_name:{0} step:{1}", windowid, step));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("page_name", windowid.ToString().ToUpper());
		dictionary.Add("step", step);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("page", properties);
	}

	public static void send_event_revival(string result, int stage, int layer, int gems)
	{
		Log(Utils.FormatString("shushu send_event_revival: result:{0} stage:{1} layer:{2} gems:{3}", result, stage, layer, gems));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("result", result);
		dictionary.Add("stage", stage);
		dictionary.Add("layer", layer);
		dictionary.Add("gems", gems);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("revival", properties);
	}

	public static void send_event_game_start(BattleSource source, int chapter)
	{
		Log(Utils.FormatString("shushu send_event_game_start: source:{0} chapter:{1}", source, chapter));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("chapter", chapter);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("game_start", properties);
	}

	public static void send_event_game_end(int survive_times, BattleSource source, BattleEndType end_type, int coins, int equipment, int end_stage, int end_layer, int newbest, int exp, int levelupcount, int level)
	{
		int value = CInstance<PlayerPrefsMgr>.Instance.gametime.get_value();
		Log(Utils.FormatString("shushu : game_end survive_times:{0} source:{1} end_type:{2} coin:{3} equipment_number:{4}, last_level:{5} newbest:{6} duration:{7}", survive_times, source, end_type, coins, equipment, end_stage, newbest, value));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("survive_times", survive_times);
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("end_type", end_type.ToString().ToUpper());
		dictionary.Add("coins", coins);
		dictionary.Add("equipment", equipment);
		dictionary.Add("stage", end_stage);
		dictionary.Add("layer", end_layer);
		dictionary.Add("attempts_from_last_new_best", newbest);
		dictionary.Add("exp", exp);
		dictionary.Add("levelupcount", levelupcount);
		dictionary.Add("level", level);
		dictionary.Add("duration", value);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("game_end", properties);
	}

	public static void send_event_equipment(string step, int equipment, int count, int target_level, EquipSource source, int coins)
	{
		Log(Utils.FormatString("shushu send_event_equipment: step:{0} equipment:{1} count:{2} target_level:{3} source:{4} coins:{5}", step, equipment, count, target_level, source, coins));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("equipment", equipment);
		dictionary.Add("count", count);
		dictionary.Add("target_level", target_level);
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("coins", coins);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("equipment", properties);
	}

	public static void send_event_talent(string step, int talent, int target_level, int coins)
	{
		Log(Utils.FormatString("shushu send_event_talent: step:{0} talentid:{1} target_level:{2} coins:{3}", step, talent, target_level, coins));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("talent", talent);
		dictionary.Add("target_level", target_level);
		dictionary.Add("coins", coins);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("talent", properties);
	}

	public static void send_event_mysteries(string step, int shoptype, int index, int equipment, int equipment1, int equipment2, int equipment3, int equipment4, int equipment5, int equipment6, int equipment7, int equipment8, int coins, int gems, string result, string reason)
	{
		if (equipment > 0)
		{
			switch (index)
			{
			case 0:
				equipment1 = equipment;
				break;
			case 1:
				equipment2 = equipment;
				break;
			case 2:
				equipment3 = equipment;
				break;
			case 3:
				equipment4 = equipment;
				break;
			case 4:
				equipment5 = equipment;
				break;
			case 5:
				equipment6 = equipment;
				break;
			case 6:
				equipment7 = equipment;
				break;
			case 7:
				equipment8 = equipment;
				break;
			}
			equipment = 0;
		}
		Log(Utils.FormatString("shushu send_event_mysteries: step:{0} shoptype:{14} equipment:{1} equipment1:{2} equipment2:{3} equipment3:{4} equipment4:{5} equipment5:{6} equipment6:{7} equipment7:{8} equipment8:{9} coins:{10} gems:{11} result:{12} reason:{13}", step, equipment, equipment1, equipment2, equipment3, equipment4, equipment5, equipment6, equipment7, equipment8, coins, gems, result, reason, shoptype));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("shoptype", shoptype);
		dictionary.Add("equipment", equipment);
		dictionary.Add("equipment_1", equipment1);
		dictionary.Add("equipment_2", equipment2);
		dictionary.Add("equipment_3", equipment3);
		dictionary.Add("equipment_4", equipment4);
		dictionary.Add("equipment_5", equipment5);
		dictionary.Add("equipment_6", equipment6);
		dictionary.Add("equipment_7", equipment7);
		dictionary.Add("equipment_8", equipment8);
		dictionary.Add("coins", coins);
		dictionary.Add("gems", gems);
		dictionary.Add("result", result);
		dictionary.Add("reason", reason);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("mysteries", properties);
	}

	public static void send_event_shop(string purchase, int coins, int gems, int equipment, int continue_count)
	{
		int beforePurchaseHour = GetBeforePurchaseHour();
		Log(Utils.FormatString("shushu send_event_shop: purchase:{0} hour:{1} coins:{2} gems:{3} equipment:{4}", purchase, beforePurchaseHour, coins, gems, equipment));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("purchase", purchase.ToUpper());
		dictionary.Add("time_since_last_purchase", beforePurchaseHour);
		dictionary.Add("coins", coins);
		dictionary.Add("gems", gems);
		dictionary.Add("equipment", equipment);
		dictionary.Add("count", continue_count);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("shop", properties);
	}

	public static void send_event_exchange(CoinExchangeSource source, int coins, int gems)
	{
		Log(Utils.FormatString("shushu send_event_exchange: source:{0} coins:{1} gems:{2}", source, coins, gems));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("coins", coins);
		dictionary.Add("gems", gems);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("exchange", properties);
	}

	public static void send_event_strength(string step, KeyBuySource source, string result, string reason, int gems)
	{
		Log(Utils.FormatString("shushu send_event_strength: step:{0} source:{1} result:{2} reason:{3} gems:{4}", step, source, result, reason, gems));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("result", result);
		dictionary.Add("reason", reason);
		dictionary.Add("gems", gems);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("strength", properties);
	}

	public static void send_event_iap(string step, ShopOpenSource source, string product, string result, string reason)
	{
		Log(Utils.FormatString("shushu send_event_iap: step:{0} source:{1} product:{2} result:{3} reason:{4}", step, source, product, result, reason));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("product", product.ToUpper());
		dictionary.Add("result", result);
		dictionary.Add("reason", reason);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("iap", properties);
	}

	public static void send_event_ad(ADSource source, string step, int coins, int gems, string result, string reason)
	{
		Log(Utils.FormatString("shushu send_event_ad_key: source:{0} step:{1} coins:{2} gems:{3} result:{4} reason:{5}", source, step, coins, gems, result, reason));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("source", source.ToString().ToUpper());
		dictionary.Add("step", step.ToUpper());
		dictionary.Add("coins", coins);
		dictionary.Add("gems", gems);
		dictionary.Add("result", result);
		dictionary.Add("reason", reason);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("ad", properties);
	}

	public static void send_event_equip_combine(string step, int equipment, string result, string reason)
	{
		Log(Utils.FormatString("shushu send_event_equip_combine: step:{0} equipment:{1} result:{2} reason:{3}", step, equipment, result, reason));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("equipment", equipment);
		dictionary.Add("result", result);
		dictionary.Add("reason", reason);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("equip_combine", properties);
	}

	public static void send_event_harvest(string step, string result, string reason, int coins, int gems)
	{
		Log(Utils.FormatString("shushu send_event_harvest: step:{0} result:{1} reason:{2} coins:{3} gems:{4}", step, result, reason, coins, gems));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("step", step);
		dictionary.Add("result", result);
		dictionary.Add("reason", reason);
		dictionary.Add("coins", coins);
		dictionary.Add("gems", gems);
		Dictionary<string, object> properties = dictionary;
		TGAnalytics.TG.track("harvest", properties);
	}

	public static void OnLogin()
	{
		WindowUI.CloseAllWindows();
		WindowUI.ShowWindow(WindowID.WindowID_Login);
	}

	public static void Login(Action<LoginData> callback)
	{
		Google_Login(callback);
	}

	public static void TryLogin(Action<LoginType, LoginData> callback)
	{
		Google_TryLogin(callback);
	}

	public static int GetLoginType()
	{
		return 1;
	}

	private static void Bugly_Init()
	{
#if ENABLE_BUGLY
        BuglyAgent.ConfigDebugMode(enable: false);
		BuglyAgent.ConfigDefault(null, null, null, 0L);
		BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogError);
		BuglyAgent.ConfigAutoQuitApplication(autoQuit: false);
		BuglyAgent.RegisterLogCallback(null);
		BuglyAgent.InitWithAppId("8c142b9f46");
		BuglyAgent.EnableExceptionHandler();
#endif
	}

	public static void Bugly_Report(Exception e, string message)
	{
#if ENABLE_BUGLY
		BuglyAgent.ReportException(e, message);
#endif
	}

	public static void Bugly_Report(bool excute, string name, string message)
	{
		if (!excute)
		{
			Bugly_Report(name, message);
		}
	}

	public static void Bugly_Report(string name, string message, string stackTrace)
	{
#if ENABLE_BUGLY
		BuglyAgent.ReportException(name, message, stackTrace);
#endif
		Debug.LogFormat("Bugly_Report name:{0},message:{1},stackTrace:{2}", name, message, stackTrace);
	}

	public static void Bugly_Report(string name, string message)
	{
		Bugly_Report(name, message, string.Empty);
	}

	private static void Facebook_Init()
	{
#if ENABLE_FB_SDK
		FB.Init("250812872464647", "e3b92c19621dc2d71a0ea872d70669c3");
#endif
	}

	public static void login_check()
	{
		UnityEngine.Debug.Log("SdkManager_GameCenter.login_check do nothing...");
	}

	public static void login_check_start()
	{
		UnityEngine.Debug.Log("SdkManager_GameCenter.login_check_start do nothing...");
	}

	public static void GameCenter_Login(Action<LoginData> callback)
	{
		Social.Active.Authenticate(Social.Active.localUser, delegate(bool success, string error_msg)
		{
			string id = Social.Active.localUser.id;
			string userName = Social.Active.localUser.userName;
			id = id.Replace(":", string.Empty);
			Debugger.Log("GameCenter Login Callback " + success + " userid = " + id);
			LoginData loginData = new LoginData
			{
				username = userName
			};
			if (success && !string.IsNullOrEmpty(id))
			{
				string userID = LocalSave.Instance.GetUserID();
				if (!string.IsNullOrEmpty(userID) && id.Equals(userID))
				{
					LocalSave.Instance.SetUserLoginSDK(value: true);
					loginData.bSuccess = true;
					loginData.userid = id;
					loginData.username = userName;
					Debugger.Log("GameCenter Login Callback userid = " + loginData.userid);
					LocalSave.Instance.SetUserID(LoginType.eGameCenter, loginData.userid, loginData.username);
				}
				else
				{
					WindowUI.TryLogin();
				}
			}
			callback(loginData);
		});
	}

	private static void GameCenter_TryLogin(Action<LoginType, LoginData> callback)
	{
		Social.Active.Authenticate(Social.Active.localUser, delegate(bool success, string error_msg)
		{
			LoginData loginData = new LoginData();
			string id = Social.Active.localUser.id;
			string userName = Social.Active.localUser.userName;
			id = id.Replace(":", string.Empty);
			if (success && !string.IsNullOrEmpty(id))
			{
				loginData.userid = id;
				loginData.username = userName;
				LocalSave.Instance.SetUserLoginSDK(value: true);
			}
			callback(LoginType.eGameCenter, loginData);
		});
	}

	public static void GameCenter_add_login_count()
	{
		int num = GameCenter_get_login_count();
		PlayerPrefsEncrypt.SetInt("gamecenter_login_count", num + 1);
	}

	public static int GameCenter_get_login_count()
	{
		return PlayerPrefsEncrypt.GetInt("gamecenter_login_count");
	}

	public static void GameCenter_clear_login_count()
	{
		PlayerPrefsEncrypt.SetInt("gamecenter_login_count", 0);
	}

	public static void Google_Login(Action<LoginData> callback)
	{
		LoginData data = new LoginData();
		data.bSuccess = false;
		data.userid = string.Empty;
		if (!CInstance<Unity2AndroidHelper>.Instance.is_gp_avalible() || !NetManager.IsNetConnect)
		{
			callback(data);
			return;
		}
#if ENABLE_GG_GAME_SERVICES
		PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().RequestEmail().RequestServerAuthCode(forceRefresh: false).RequestIdToken()
			.Build();
#endif
		Debugger.Log("SdkManager_Google.Google_Login start");
#if ENABLE_GG_GAME_SERVICES
		PlayGamesPlatform.InitializeInstance(configuration);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		Social.Active.Authenticate(Social.Active.localUser, delegate(bool success, string error_msg)
		{
			if (success)
			{
				PlayGamesLocalUser playGamesLocalUser = (PlayGamesLocalUser)Social.Active.localUser;
				string email = playGamesLocalUser.Email;
				string idToken = playGamesLocalUser.GetIdToken();
				data.userid = playGamesLocalUser.id;
				data.username = playGamesLocalUser.userName;
				Debugger.Log("SdkManager_Google login google userName:" + playGamesLocalUser.userName);
				Debugger.Log("SdkManager_Google login google id:" + playGamesLocalUser.id);
				Debugger.Log("SdkManager_Google login google 登录成功");
				LocalSave.Instance.SetUserLoginSDK(value: true);
				LocalSave.Instance.SetUserID(LoginType.eGP, data.userid, data.username);
				data.bSuccess = true;
				data.userid = playGamesLocalUser.id;
			}
			else
			{
				UnityEngine.Debug.Log("SdkManager_Google google Init return false.");
			}
			callback(data);
		});
#endif
	}

	public static void Google_TryLogin(Action<LoginType, LoginData> callback)
	{
		LoginData data = new LoginData();
		if (!CInstance<Unity2AndroidHelper>.Instance.is_gp_avalible())
		{
			callback(LoginType.eGP, data);
			return;
		}
#if ENABLE_GG_GAME_SERVICES
		PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().RequestEmail().RequestServerAuthCode(forceRefresh: false).RequestIdToken()
			.Build();
		PlayGamesPlatform.InitializeInstance(configuration);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
#endif
		Debugger.Log("SdkManager_Google googlelogin Google_TryLogin start");
#if ENABLE_GG_GAME_SERVICES
        Social.Active.Authenticate(Social.Active.localUser, delegate(bool success, string error_msg)
		{
			Debugger.Log("SdkManager_Google googlelogin Google_TryLogin start callback " + success);
			if (success)
			{
				Debugger.Log("SdkManager_Google Google_TryLogin success ");
				PlayGamesLocalUser playGamesLocalUser = (PlayGamesLocalUser)Social.Active.localUser;
				data.userid = playGamesLocalUser.id;
				data.username = playGamesLocalUser.userName;
			}
			else
			{
				Debugger.Log("SdkManager_Google Google_TryLogin fail ");
			}
			callback(LoginType.eGP, data);
		});
#endif
	}

	private static void ShuShu_Init()
	{
	}

	public static void ShuShu_Login(string name)
	{
		TGAnalytics.TG.login(name);
	}

	private static void Umeng_Init()
	{
		Analytics.Start();
		Analytics.SetLogEnabled(value: false);
	}
}
