




using System.Collections.Generic;

namespace ThinkingGame
{
	public interface AnalyticsInterface
	{
		void init(string appId, string serverURL);

		void track(string eventName, Dictionary<string, object> properties);

		void track(string eventName);

		void timeEvent(string eventName);

		void logout();

		void login(string name);

		void identify(string identifyId);

		void user_set(Dictionary<string, object> properties);

		void user_setOnce(Dictionary<string, object> properties);

		void user_add(Dictionary<string, object> properties);

		void user_add(string propertyKey, double propertyValue);

		void user_delete();

		void setSuperProperties(Dictionary<string, object> superProperties);

		void unsetSuperProperty(string superPropertyName);

		void clearSuperProperties();

		Dictionary<string, object> getSuperProperties();

		string getDeviceID();

		string getDistinctID();
	}
}
