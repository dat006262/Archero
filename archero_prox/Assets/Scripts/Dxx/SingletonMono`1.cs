




using UnityEngine;

namespace Dxx
{
	public class SingletonMono<T> : SingletonableMono where T : SingletonableMono
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if ((Object)_instance == (Object)null)
				{
					GameObject gameObject = new GameObject(typeof(T).Name);
					_instance = gameObject.AddComponent<T>();
					_instance.OnInstanceCreate();
					Object.DontDestroyOnLoad(gameObject);
				}
				return _instance;
			}
		}
	}
}
