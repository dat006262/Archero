




using System.Collections.Generic;

namespace Dxx
{
	public static class ListPool<T>
	{
		private static readonly ObjectPool<List<T>> m_ListPool = new ObjectPool<List<T>>(null, delegate(List<T> l)
		{
			l.Clear();
		});

		public static List<T> Get()
		{
			return m_ListPool.Get();
		}

		public static void Release(List<T> toRelease)
		{
			m_ListPool.Release(toRelease);
		}
	}
}
