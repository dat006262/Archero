




using System.Text;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static object _lock = new object();

	private static StringBuilder strTemp = new StringBuilder();

	private static bool applicationIsQuitting = false;

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				return (T)null;
			}
			lock (_lock)
			{
				if ((Object)_instance == (Object)null)
				{
					_instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						return _instance;
					}
					if ((Object)_instance == (Object)null)
					{
						GameObject gameObject = new GameObject();
						_instance = gameObject.AddComponent<T>();
						strTemp.Clear();
						strTemp.AppendFormat("(singleton) {0}", typeof(T).ToString());
						gameObject.name = strTemp.ToString();
						Object.DontDestroyOnLoad(gameObject);
					}
				}
				return _instance;
			}
		}
	}

	public void OnDestroy()
	{
		_instance = (T)null;
		applicationIsQuitting = true;
	}
}
