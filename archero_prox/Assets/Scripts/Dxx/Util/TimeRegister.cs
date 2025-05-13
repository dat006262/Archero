




using System;
using System.Collections.Generic;

namespace Dxx.Util
{
	public static class TimeRegister
	{
		private static Dictionary<int, TimeRepeat> mList = new Dictionary<int, TimeRepeat>();

		private static int mIndex = 0;

		public static int Register(string name, float updatetime, Action callback, bool firstdo = false, float delaytime = 0f)
		{
			mIndex++;
			TimeRepeat value = new TimeRepeat(name, updatetime, callback, firstdo, delaytime);
			mList.Add(mIndex, value);
			return mIndex;
		}

		public static void UnRegister(int index)
		{
			if (mList.TryGetValue(index, out TimeRepeat value))
			{
				value.UnRegister();
				mList.Remove(index);
			}
		}
	}
}
