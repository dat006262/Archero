




using System;
using System.Collections.Generic;

namespace Dxx.Util
{
	public class TimeClock
	{
		public class TimeClockOne
		{
			private float time;

			private float delaytime;

			private Action action;

			private string name;

			public TimeClockOne(string name, float delay, Action action)
			{
				this.name = name;
				delaytime = delay;
				this.action = action;
				Updater.AddUpdate(Utils.FormatString("TimeClockOne.{0}", name), OnUpdate);
			}

			private void OnUpdate(float delta)
			{
				time += delta;
				if (time >= delaytime)
				{
					time -= delaytime;
					action();
				}
			}

			public void DeInit()
			{
				Updater.RemoveUpdate(Utils.FormatString("TimeClockOne.{0}", name), OnUpdate);
			}
		}

		private static long mClockIndex = 0L;

		private static Dictionary<long, TimeClockOne> mList = new Dictionary<long, TimeClockOne>();

		public static long Register(string name, float delta, Action action)
		{
			TimeClockOne value = new TimeClockOne(name, delta, action);
			mClockIndex++;
			mList.Add(mClockIndex, value);
			return mClockIndex;
		}

		public static void Unregister(long index)
		{
			if (mList.TryGetValue(index, out TimeClockOne value))
			{
				value.DeInit();
				mList.Remove(index);
			}
		}

		public static void Clear()
		{
			int i = 0;
			for (int count = mList.Count; i < count; i++)
			{
				mList[i].DeInit();
			}
			mList.Clear();
			mClockIndex = 0L;
		}
	}
}
