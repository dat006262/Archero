




namespace U2
{
	public class NameGenerator
	{
		protected string _szPrefix;

		protected ulong _ulNext;

		protected object _Lock = new object();

		public NameGenerator(string prefix)
		{
			_szPrefix = prefix;
			_ulNext = 1uL;
		}

		public string Generate()
		{
			lock (_Lock)
			{
				return _szPrefix + _ulNext++;
			}
		}

		public void Reset()
		{
			lock (_Lock)
			{
				_ulNext = 0uL;
			}
		}

		public void SetNext(ulong val)
		{
			lock (_Lock)
			{
				_ulNext = val;
			}
		}

		public ulong GetNext()
		{
			lock (_Lock)
			{
				return _ulNext;
			}
		}
	}
}
