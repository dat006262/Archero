




using System.Collections.Generic;

namespace Dxx
{
	public static class HashSetPool<T>
	{
		private static readonly ObjectPool<HashSet<T>> m_DictPool = new ObjectPool<HashSet<T>>(null, delegate(HashSet<T> d)
		{
			d.Clear();
		});

		public static HashSet<T> Get()
		{
			return m_DictPool.Get();
		}

		public static void Release(HashSet<T> toRelease)
		{
			m_DictPool.Release(toRelease);
		}
	}
}
