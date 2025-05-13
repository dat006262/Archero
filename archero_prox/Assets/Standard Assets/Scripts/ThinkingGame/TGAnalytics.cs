




using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThinkingGame
{
	public class TGAnalytics : MonoBehaviour
#if ENABLE_TGA
        , AnalyticsInterface
#endif
    {
		private string serverURL = "http://receiver.habby.mobi";

		private string appId;

		public AnalyticsInterface mAnalytics;

		public static TGAnalytics TG;

		private string adid;

		private string networkName;

		public void Awake()
		{
			if (!(TG != null))
			{
				TG = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				try
				{
					setDateTime(new Dictionary<string, object>());
					mAnalytics = new TGAndroidAnalytics();
					appId = "fb94cc735e874a918951554391ca1db3";
					init(appId, serverURL);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
		}

		public void init(string appId, string serverURL)
		{
			if (mAnalytics != null)
			{
				mAnalytics.init(appId, serverURL);
				attribution();
				if (!PlayerPrefs.HasKey("first_active"))
				{
					PlayerPrefs.SetInt("first_active", 0);
					track("first_active");
				}
			}
		}

		protected void attribution()
		{
		}

		public void clearSuperProperties()
		{
			if (mAnalytics != null)
			{
				mAnalytics.clearSuperProperties();
			}
		}

		protected void setDateTime(Dictionary<string, object> properties)
		{
			DateTime utcNow = DateTime.UtcNow;
			properties.Add("event_time_utc", utcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
			properties.Add("event_time_beijing", utcNow.AddHours(8.0).ToString("yyyy-MM-dd HH:mm:ss.fff"));
			properties.Add("guid", Guid.NewGuid().ToString());
		}

		public Dictionary<string, object> getSuperProperties()
		{
			if (mAnalytics == null)
			{
				return null;
			}
			return mAnalytics.getSuperProperties();
		}

		public void identify(string identifyId)
		{
			if (mAnalytics != null)
			{
				mAnalytics.identify(identifyId);
			}
		}

		public void logout()
		{
			if (mAnalytics != null)
			{
				mAnalytics.logout();
			}
		}

		public void login(string name)
		{
			if (mAnalytics != null)
			{
				mAnalytics.login(name);
			}
		}

		public void setSuperProperties(Dictionary<string, object> superProperties)
		{
			if (mAnalytics != null)
			{
				mAnalytics.setSuperProperties(superProperties);
			}
		}

		public void timeEvent(string eventName)
		{
			if (mAnalytics != null)
			{
				mAnalytics.timeEvent(eventName);
			}
		}

		public void track(string eventName, Dictionary<string, object> properties)
		{
			if (mAnalytics != null)
			{
				setDateTime(properties);
				mAnalytics.track(eventName, properties);
			}
		}

		public void track(string eventName)
		{
			track(eventName, new Dictionary<string, object>());
		}

		public void unsetSuperProperty(string superPropertyName)
		{
			if (mAnalytics != null)
			{
				mAnalytics.unsetSuperProperty(superPropertyName);
			}
		}

		public void user_add(Dictionary<string, object> properties)
		{
			if (mAnalytics != null)
			{
				setDateTime(properties);
				mAnalytics.user_add(properties);
			}
		}

		public void user_add(string propertyKey, double propertyValue)
		{
			if (mAnalytics != null)
			{
				mAnalytics.user_add(propertyKey, propertyValue);
			}
		}

		public void user_delete()
		{
			if (mAnalytics != null)
			{
				mAnalytics.user_delete();
			}
		}

		public void user_set(Dictionary<string, object> properties)
		{
			if (mAnalytics != null)
			{
				setDateTime(properties);
				mAnalytics.user_set(properties);
			}
		}

		public void user_setOnce(Dictionary<string, object> properties)
		{
			if (mAnalytics != null)
			{
				setDateTime(properties);
				mAnalytics.user_setOnce(properties);
			}
		}

		public string getDeviceID()
		{
			return (mAnalytics != null) ? mAnalytics.getDeviceID() : "unknown";
		}

		public string getDistinctID()
		{
			return (mAnalytics != null) ? mAnalytics.getDistinctID() : "unknown";
		}
	}
}
