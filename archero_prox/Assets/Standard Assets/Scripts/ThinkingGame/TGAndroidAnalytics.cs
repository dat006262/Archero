




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThinkingGame
{
	public class TGAndroidAnalytics : AnalyticsInterface
	{
		private static class SingletonHolder
		{
#if ENABLE_TGA
			public static AndroidJavaObject instance_mobclick;
			public static AndroidJavaObject instance_context;
#endif
            static SingletonHolder()
			{
#if ENABLE_TGA
                instance_mobclick = new AndroidJavaObject("com.wwgame.game.libtkgame.TGAnalytics");
                using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					instance_context = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
#endif
            }
        }

#if ENABLE_TGA
		protected static AndroidJavaObject Agent => SingletonHolder.instance_mobclick;

		protected static AndroidJavaObject Context => SingletonHolder.instance_context;
#endif
        public void init(string appId, string serverURL)
		{
			agentInit(appId, serverURL);
		}

		public void track(string eventName, Dictionary<string, object> properties)
		{
			string text = TGAUtils.DictionaryToJson(properties);
#if ENABLE_TGA
			Agent.Call("track", eventName, text);
#endif
		}

		public void track(string eventName)
		{
#if ENABLE_TGA
			Agent.Call("track", eventName);
#endif
		}

		public void timeEvent(string eventName)
		{
#if ENABLE_TGA
			Agent.Call("timeEvent", eventName);
#endif
		}

		public void logout()
		{
#if ENABLE_TGA
			Agent.Call("logout");
#endif
		}

		public void login(string name)
		{
#if ENABLE_TGA
			Agent.Call("login", name);
#endif
		}

		public void identify(string identifyId)
		{
#if ENABLE_TGA
			Agent.Call("identify", identifyId);
#endif
		}

		public void user_set(Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			Agent.Call("user_set", TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void user_setOnce(Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			Agent.Call("user_setOnce", TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void user_add(Dictionary<string, object> properties)
		{
#if ENABLE_TGA
			Agent.Call("user_add", TGAUtils.DictionaryToJson(properties));
#endif
		}

		public void user_add(string propertyKey, double propertyValue)
		{
#if ENABLE_TGA
			Agent.Call("user_add", propertyKey, propertyValue);
#endif
		}

		public void user_delete()
		{
#if ENABLE_TGA
			Agent.Call("user_delete");
#endif
		}

		public void setSuperProperties(Dictionary<string, object> superProperties)
		{
#if ENABLE_TGA
			Agent.Call("setSuperProperties", TGAUtils.DictionaryToJson(superProperties));
#endif
		}

		public void unsetSuperProperty(string superPropertyName)
		{
#if ENABLE_TGA
			Agent.Call("unsetSuperProperty", superPropertyName);
#endif
		}

		public void clearSuperProperties()
		{
#if ENABLE_TGA
			Agent.Call("clearSuperProperties");
#endif
		}

		public Dictionary<string, object> getSuperProperties()
		{
#if ENABLE_TGA
			string text = Agent.Call<string>("getSuperProperties", Array.Empty<object>());
			if (text != null)
			{
				return JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
			}
#endif
			return new Dictionary<string, object>();
		}

		public static void agentInit(string AppKey, string url)
		{
#if ENABLE_TGA
			Agent.CallStatic("init", Context, AppKey, url);
#endif
		}

		public string getDeviceID()
		{
#if ENABLE_TGA
			return Agent.Call<string>("getDeviceId", Array.Empty<object>());
#endif
            return string.Empty;
		}

		public string getDistinctID()
		{
#if ENABLE_TGA
			return Agent.Call<string>("getDistinctId", Array.Empty<object>());
#endif
            return string.Empty;
		}

		public void Dispose()
		{
#if ENABLE_TGA
			Agent.Dispose();
			Context.Dispose();
#endif
        }
    }
}
