




using UnityEngine;

namespace Umeng
{
	public class DplusAgent
	{
#if ENABLE_UMENG
		private static AndroidJavaOb#if ENABLE_UMENGject _DplusAgent = new AndroidJavaClass("com.umeng.analytics.dplus.UMADplus");
#endif

		private static AndroidJavaObject Context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

#if ENABLE_UMENG
		private static AndroidJavaClass UnityUtil = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
#endif

		public static void track(string eventName)
		{
#if ENABLE_UMENG
			_DplusAgent.CallStatic("track", Context, eventName);
#endif
		}

		public static void track(string eventName, JSONObject jsonObject)
		{
#if ENABLE_UMENG
			UnityUtil.CallStatic("track", Context, eventName, jsonObject.ToString());
#endif
		}

		public static void registerSuperProperty(JSONObject jsonObject)
		{
#if ENABLE_UMENG
			UnityUtil.CallStatic("registerSuperPropertyAll", Context, jsonObject.ToString());
#endif
		}

		public static void unregisterSuperProperty(string propertyName)
		{
#if ENABLE_UMENG
			_DplusAgent.CallStatic("unregisterSuperProperty", Context, propertyName);
#endif
		}

		public static JSONNode getSuperProperty(string propertyName)
		{
#if ENABLE_UMENG
			string aJSON = UnityUtil.CallStatic<string>("getSuperProperty", new object[2]
			{
				Context,
				propertyName
			});
			return JSON.Parse(aJSON)["__umeng_internal_data_"];
#endif
            return default(JSONNode);
		}

		public static JSONObject getSuperProperties()
		{
#if ENABLE_UMENG
			string aJSON = _DplusAgent.CallStatic<string>("getSuperProperties", new object[1]
			{
				Context
			});
			return (JSONObject)JSON.Parse(aJSON);
#endif
            return new JSONObject();
		}

		public static void clearSuperProperties()
		{
#if ENABLE_UMENG
			_DplusAgent.CallStatic("clearSuperProperties", Context);
#endif
		}

		public static void setFirstLaunchEvent(string[] trackID)
		{
#if ENABLE_UMENG
			UnityUtil.CallStatic("setFirstLaunchEvent", Context, string.Join(";=umengUnity=;", trackID));
#endif
		}
	}
}
