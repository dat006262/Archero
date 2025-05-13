




using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ThinkingGame
{
	public class TGIOSAnalytics : AnalyticsInterface
	{
		public void init(string appId, string serverURL)
		{
			UnityEngine.Debug.Log("init appId:" + appId + " serverURL:" + serverURL);
#if ENABLE_TGA
			ThinkingAnalyticsInit(appId, serverURL);
#endif
		}

		public void track(string eventName, Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			TGA_track(eventName, TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void track(string eventName)
		{
#if ENABLE_TGA
			TGA_track(eventName, null);
#endif
		}

		public void timeEvent(string eventName)
		{
#if ENABLE_TGA
			TGA_timeEvent(eventName);
#endif
		}

		public void logout()
		{
#if ENABLE_TGA
			TGA_logout();
#endif
		}

		public void login(string name)
		{
#if ENABLE_TGA
			TGA_login(name);
#endif
		}

		public void identify(string identifyId)
		{
#if ENABLE_TGA
			TGA_identify(identifyId);
#endif
		}

		public void user_set(Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			TGA_user_set(TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void user_setOnce(Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			TGA_user_setOnce(TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void user_add(Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			TGA_user_add(TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void user_add(string propertyKey, double propertyValue)
		{
		}

		public void user_delete()
		{
#if ENABLE_TGA
			TGA_user_delete();
#endif
		}

		public void setSuperProperties(Dictionary<string, object> superProperties)
		{
#if ENABLE_TGA
			TGA_setSuperProperties(TGAUtils.DictionaryToJson(superProperties));
#endif
		}

		public void unsetSuperProperty(string superPropertyName)
		{
#if ENABLE_TGA
			TGA_unsetSuperProperty(superPropertyName);
#endif
		}

		public void clearSuperProperties()
		{
#if ENABLE_TGA
			TGA_clearSuperProperties();
#endif
		}

		public Dictionary<string, object> getSuperProperties()
		{
			return null;
		}

		public string getDeviceID()
		{
#if ENABLE_TGA
			return TGA_getDeviceId();
#endif
            return SystemInfo.deviceUniqueIdentifier;
		}

		public string getDistinctID()
		{
#if ENABLE_TGA
			return TGA_getDistinctId();
#endif
            return string.Empty;
		}

#if ENABLE_TGA
		[DllImport("__Internal")]
		private static extern void TGA_track(string eventName, string properties);

		[DllImport("__Internal")]
		private static extern void ThinkingAnalyticsInit(string appId, string serverURL);

		[DllImport("__Internal")]
		private static extern void TGA_timeEvent(string eventName);

		[DllImport("__Internal")]
		private static extern void TGA_user_set(string property);

		[DllImport("__Internal")]
		private static extern void TGA_user_setOnce(string properties);

		[DllImport("__Internal")]
		private static extern void TGA_user_add(string properties);

		[DllImport("__Internal")]
		private static extern void TGA_user_delete();

		[DllImport("__Internal")]
		private static extern void TGA_setSuperProperties(string properties);

		[DllImport("__Internal")]
		private static extern void TGA_unsetSuperProperty(string property);

		[DllImport("__Internal")]
		private static extern void TGA_clearSuperProperties();

		[DllImport("__Internal")]
		private static extern void TGA_identify(string distinctId);

		[DllImport("__Internal")]
		private static extern void TGA_login(string accountId);

		[DllImport("__Internal")]
		private static extern void TGA_logout();

		[DllImport("__Internal")]
		private static extern string TGA_getDeviceId();

		[DllImport("__Internal")]
		private static extern string TGA_getDistinctId();
#endif
	}
}
