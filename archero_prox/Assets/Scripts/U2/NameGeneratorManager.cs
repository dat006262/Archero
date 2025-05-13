




using System.Collections.Generic;

namespace U2
{
	public class NameGeneratorManager
	{
		public static string EquipKey = "Equipkey";

		private static volatile NameGeneratorManager _Instance;

		private Dictionary<string, NameGenerator> _NameGeneratorMap = new Dictionary<string, NameGenerator>();

		private object _Lock = new object();

		public static NameGeneratorManager Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new NameGeneratorManager();
				}
				return _Instance;
			}
		}

		private NameGeneratorManager()
		{
		}

		public void RegisterNameGenerator(string prefix)
		{
			lock (_Lock)
			{
				if (!_NameGeneratorMap.ContainsKey(prefix))
				{
					_NameGeneratorMap.Add(prefix, new NameGenerator(prefix));
				}
			}
		}

		public void UnregisterNameGenerator(string prefix)
		{
			lock (_Lock)
			{
				if (_NameGeneratorMap.ContainsKey(prefix))
				{
					_NameGeneratorMap.Remove(prefix);
				}
			}
		}

		public string Generator(string prefix)
		{
			lock (_Lock)
			{
				if (_NameGeneratorMap.TryGetValue(prefix, out NameGenerator value))
				{
					return value.Generate();
				}
				return prefix;
			}
		}

		public NameGenerator GetNameGenerator(string prefix)
		{
			lock (_Lock)
			{
				NameGenerator value = null;
				_NameGeneratorMap.TryGetValue(prefix, out value);
				return value;
			}
		}
	}
}
