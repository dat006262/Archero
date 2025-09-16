




namespace Dxx
{
	public class Singleton<T> : Singletonable where T : Singletonable, new()
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new T();
				}
				_instance.OnInstanceCreate();
				return _instance;
			}
		}
	}
}
