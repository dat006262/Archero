




using System;
using UnityEngine;

namespace Umeng
{
	public class GA : Analytics
	{
		public enum Gender
		{
			Unknown,
			Male,
			Female
		}

		public enum PaySource
		{
			AppStore = 1,
			支付宝,
			网银,
			财付通,
			移动,
			联通,
			电信,
			Paypal,
			Source9,
			Source10,
			Source11,
			Source12,
			Source13,
			Source14,
			Source15,
			Source16,
			Source17,
			Source18,
			Source19,
			Source20,
			Source21,
			Source22,
			Source23,
			Source24,
			Source25,
			Source26,
			Source27,
			Source28,
			Source29,
			Source30
		}

		public enum BonusSource
		{
			玩家赠送 = 1,
			Source2,
			Source3,
			Source4,
			Source5,
			Source6,
			Source7,
			Source8,
			Source9,
			Source10
		}

		public static void SetUserLevel(int level)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("setPlayerLevel", level);
#endif
		}

		[Obsolete("SetUserLevel(string level) 已弃用, 请使用 SetUserLevel(int level)")]
		public static void SetUserLevel(string level)
		{
			UnityEngine.Debug.LogWarning("SetUserLevel(string level) 已弃用, 请使用 SetUserLevel(int level)");
		}

		[Obsolete("SetUserInfo已弃用, 请使用ProfileSignIn")]
		public static void SetUserInfo(string userId, Gender gender, int age, string platform)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("setPlayerInfo", userId, age, (int)gender, platform);
#endif
		}

		public static void StartLevel(string level)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("startLevel", level);
#endif
		}

		public static void FinishLevel(string level)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("finishLevel", level);
#endif
		}

		public static void FailLevel(string level)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("failLevel", level);
#endif
		}

		public static void Pay(double cash, PaySource source, double coin)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("pay", cash, coin, (int)source);
#endif
		}

		public static void Pay(double cash, int source, double coin)
		{
			if (source < 1 || source > 100)
			{
				throw new ArgumentException();
			}
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("pay", cash, coin, source);
#endif
		}

		public static void Pay(double cash, PaySource source, string item, int amount, double price)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("pay", cash, item, amount, price, (int)source);
#endif
		}

		public static void Buy(string item, int amount, double price)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("buy", item, amount, price);
#endif
		}

		public static void Use(string item, int amount, double price)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("use", item, amount, price);
#endif
		}

		public static void Bonus(double coin, BonusSource source)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("bonus", coin, (int)source);
#endif
		}

		public static void Bonus(string item, int amount, double price, BonusSource source)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("bonus", item, amount, price, (int)source);
#endif
		}

		public static void ProfileSignIn(string userId)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("onProfileSignIn", userId);
#endif
		}

		public static void ProfileSignIn(string userId, string provider)
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("onProfileSignIn", provider, userId);
#endif
		}

		public static void ProfileSignOff()
		{
#if ENABLE_UMENG
			Analytics.Agent.CallStatic("onProfileSignOff");
#endif
		}
	}
}
