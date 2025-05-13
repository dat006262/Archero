




namespace com.F4A.MobileThird
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EventsManager : SingletonMono<EventsManager>
    {
        public void LogEvent(string nameEvent, Dictionary<string, string> events)
        {
            try
            {
                Debug.Log("@LOG EventsManager LogEvent nameEvent:" + nameEvent);
#if DEFINE_FIREBASE_ANALYTIC
                FirebaseManager.Instance.LogEvent(nameEvent, events);
#endif
                AppsFlyerManager.Instance.EventCustom(nameEvent, events);
                AnalyticManager.Instance.LogFacebookEvents(nameEvent, parameters: events);
            }
            catch(Exception ex)
            {
                Debug.Log("@LOG EventsManager LogEvent ex:" + ex.Message);
            }
        }

        public void LogEvent(string nameEvent, Dictionary<string, object> events)
        {
            try
            {
                Debug.Log("@LOG EventsManager LogEvent nameEvent:" + nameEvent);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                if(events != null)
                {
                    foreach(var pair in events)
                    {
                        parameters[pair.Key] = pair.Value.ToString();
                    }
                }
                LogEvent(nameEvent, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log("@LOG EventsManager LogEvent ex:" + ex.Message);
            }
        }

        public void LogEvent(string nameEvent)
        {
            LogEvent(nameEvent, new Dictionary<string, string>());
        }
    }
}
