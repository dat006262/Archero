




using System;

namespace Dxx.Util
{
	public class TimeRepeat
	{
		private float updatetime;

		private float starttime;

		private float delaytime;

		private bool firstdo;

		private Action mCallback;

		private string name;

		public TimeRepeat(string name, float updatetime, Action callback, bool firstdo, float delaytime)
		{
			this.name = name;
			Init(updatetime, callback, firstdo, delaytime);
		}

		private void Init(float updatetime, Action callback, bool firstdo, float delaytime)
		{
			this.delaytime = delaytime;
			this.updatetime = updatetime;
			mCallback = callback;
			this.firstdo = firstdo;
			if (this.firstdo)
			{
				starttime = Updater.AliveTime + delaytime - updatetime;
			}
			else
			{
				starttime = Updater.AliveTime + delaytime;
			}
			Register();
		}

		private void Register()
		{
			Updater.AddUpdate(Utils.FormatString("TimeRepeat.{0}", name), Update);
		}

		public void UnRegister()
		{
			Updater.RemoveUpdate(Utils.FormatString("TimeRepeat.{0}", name), Update);
		}

		private void Update(float delta)
		{
			if (Updater.AliveTime - starttime >= updatetime)
			{
				starttime += updatetime;
				if (mCallback != null)
				{
					mCallback();
				}
			}
		}
	}
}
