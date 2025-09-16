




using System.Collections.Generic;

namespace Dxx
{
	public static class DictionaryPool<K, V>
	{
		private static readonly ObjectPool<Dictionary<K, V>> m_DictPool = new ObjectPool<Dictionary<K, V>>(null, delegate(Dictionary<K, V> d)
		{
			d.Clear();
		});

		public static Dictionary<K, V> Get()
		{
			return m_DictPool.Get();
		}

		public static void Release(Dictionary<K, V> toRelease)
		{
			m_DictPool.Release(toRelease);
		}
	}
}
