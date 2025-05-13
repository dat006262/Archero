




using System;
using System.Collections.Generic;
using UnityEngine;

namespace Umeng
{
	public class Analytics
	{
		private static class SingletonHolder
		{
#if ENABLE_UMENG
			public static AndroidJavaClass instance_mobclick;

			public static AndroidJavaObject instance_context;
#endif

			static SingletonHolder()
			{
#if ENABLE_UMENG
				instance_mobclick = new AndroidJavaClass("com.umeng.analytics.game.UMGameAgent");
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					instance_context = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
#endif
			}
		}

		private static bool hasInit;

		private static string _AppKey;

		private static string _ChannelId;

		private static AndroidJavaClass _UpdateAgent;

		public static string AppKey => _AppKey;

		public static string ChannelId => _ChannelId;

#if ENABLE_UMENG
		protected static AndroidJavaClass Agent => SingletonHolder.instance_mobclick;

		protected static AndroidJavaClass UpdateAgent
		{
			get
			{
				if (_UpdateAgent == null)
				{
					_UpdateAgent = new AndroidJavaClass("com.umeng.update.UmengUpdateAgent");
				}
				return _UpdateAgent;
			}
		}

		protected static AndroidJavaObject Context => SingletonHolder.instance_context;
#endif

		public static void Start()
		{
			UMGameAgentInit();
			if (!hasInit)
			{
				onResume();
				CreateUmengManger();
				hasInit = true;
			}
		}

		public static void SetLogEnabled(bool value)
		{
#if ENABLE_UMENG
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.commonsdk.UMConfigure");
			androidJavaClass.CallStatic("setLogEnabled", value);
#endif
		}

		public static void Event(string eventId)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEvent", Context, eventId);
#endif
		}

		public static void Event(string eventId, string label)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEvent", Context, eventId, label);
#endif
		}

		public static void Event(string eventId, Dictionary<string, string> attributes)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEvent", Context, eventId, ToJavaHashMap(attributes));
#endif
		}

		public static void EventBegin(string eventId)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventBegin", Context, eventId);
#endif
		}

		public static void EventEnd(string eventId)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventEnd", Context, eventId);
#endif
		}

		public static void EventBegin(string eventId, string label)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventBegin", Context, eventId, label);
#endif
		}

		public static void EventEnd(string eventId, string label)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventEnd", Context, eventId, label);
#endif
		}

		public static void EventBeginWithPrimarykeyAndAttributes(string eventId, string primaryKey, Dictionary<string, string> attributes)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onKVEventBegin", Context, eventId, ToJavaHashMap(attributes), primaryKey);
#endif
		}

		public static void EventEndWithPrimarykey(string eventId, string primaryKey)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onKVEventEnd", Context, eventId, primaryKey);
#endif
		}

		public static void EventDuration(string eventId, int milliseconds)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventDuration", Context, eventId, (long)milliseconds);
#endif
		}

		public static void EventDuration(string eventId, string label, int milliseconds)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventDuration", Context, eventId, label, (long)milliseconds);
#endif
		}

		public static void EventDuration(string eventId, Dictionary<string, string> attributes, int milliseconds)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onEventDuration", Context, eventId, ToJavaHashMap(attributes), (long)milliseconds);
#endif
		}

		public static void PageBegin(string pageName)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onPageStart", pageName);
#endif
		}

		public static void PageEnd(string pageName)
		{
#if ENABLE_UMENG
			Agent.CallStatic("onPageEnd", pageName);
#endif
		}

		public static void Event(string eventId, Dictionary<string, string> attributes, int value)
		{
			try
			{
				if (attributes == null)
				{
					attributes = new Dictionary<string, string>();
				}
				if (attributes.ContainsKey("__ct__"))
				{
					attributes["__ct__"] = value.ToString();
					Event(eventId, attributes);
				}
				else
				{
					attributes.Add("__ct__", value.ToString());
					Event(eventId, attributes);
					attributes.Remove("__ct__");
				}
			}
			catch (Exception)
			{
			}
		}

		public static string GetDeviceInfo()
		{
#if ENABLE_UMENG
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
			return androidJavaClass.CallStatic<string>("getDeviceInfo", new object[1]
			{
				Context
			});
#endif
            return string.Format("deviceUniqueIdentifier:{0}, deviceName:{1}, deviceModel:{2}", SystemInfo.deviceUniqueIdentifier,
                SystemInfo.deviceName, SystemInfo.deviceModel);
		}

		public static void SetLogEncryptEnabled(bool value)
		{
#if ENABLE_UMENG
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.commonsdk.UMConfigure");
			androidJavaClass.CallStatic("setEncryptEnabled", value);
#endif
		}

		public static void SetLatency(int value)
		{
#if ENABLE_UMENG
			Agent.CallStatic("setLatencyWindow", (long)value);
#endif
		}

		public static void SetContinueSessionMillis(long milliseconds)
		{
#if ENABLE_UMENG
			Agent.CallStatic("setSessionContinueMillis", milliseconds);
#endif
		}

		[Obsolete("Flush")]
		public static void Flush()
		{
#if ENABLE_UMENG
			Agent.CallStatic("flush", Context);
#endif
		}

		[Obsolete("SetEnableLocation已弃用")]
		public static void SetEnableLocation(bool reportLocation)
		{
#if ENABLE_UMENG
			Agent.CallStatic("setAutoLocation", reportLocation);
#endif
		}

		public static void EnableActivityDurationTrack(bool isTraceActivity)
		{
#if ENABLE_UMENG
			Agent.CallStatic("openActivityDurationTrack", isTraceActivity);
#endif
		}

		public static void SetCheckDevice(bool value)
		{
#if ENABLE_UMENG
			Agent.CallStatic("setCheckDevice", value);
#endif
		}

		private static void CreateUmengManger()
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UmengManager>();
			gameObject.name = "UmengManager";
		}

		public static void onResume()
		{
#if ENABLE_UMENG
			Agent.CallStatic("onResume", Context);
#endif
		}

		public static void onPause()
		{
#if ENABLE_UMENG
			Agent.CallStatic("onPause", Context);
#endif
		}

		public static void onKillProcess()
		{
#if ENABLE_UMENG
			Agent.CallStatic("onKillProcess", Context);
#endif
		}

		private static AndroidJavaObject ToJavaHashMap(Dictionary<string, string> dic)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] array = new object[2];
			foreach (KeyValuePair<string, string> item in dic)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", item.Key))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", item.Value))
					{
						array[0] = androidJavaObject2;
						array[1] = androidJavaObject3;
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			return androidJavaObject;
		}

		public static void UMGameAgentInit()
		{
#if ENABLE_UMENG
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
			androidJavaClass.CallStatic("initUnity", Context, string.Empty, string.Empty);
#endif
		}

		public void Dispose()
		{
#if ENABLE_UMENG
			Agent.Dispose();
			Context.Dispose();
#endif
		}
	}
}
