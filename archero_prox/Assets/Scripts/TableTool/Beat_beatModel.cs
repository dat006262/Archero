




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Beat_beatModel : LocalModel<Beat_beat, int>
	{
		private const string _Filename = "Beat_beat";

		protected override string Filename => "Beat_beat";

		protected override int GetBeanKey(Beat_beat bean)
		{
			return bean.ID;
		}

		public string GetBeat(int layer)
		{
			IList<Beat_beat> allBeans = GetAllBeans();
			int count = GetBeanKeyList().Count;
			Beat_beat beatOne = GetBeatOne(layer, 0, count - 1);
			if (beatOne == null)
			{
				return MathDxx.Clamp(layer, 0f, 100f) + "%";
			}
			if (beatOne.ID == 0)
			{
				return "0.1%";
			}
			Beat_beat beanById = GetBeanById(beatOne.ID - 1);
			float num = (float)(layer - beanById.Score) / (float)(beatOne.Score - beanById.Score);
			float num2 = (beatOne.Rate - beanById.Rate) * num + beanById.Rate;
			return Utils.GetFloat2(num2 * 100f) + "%";
		}

		private Beat_beat GetBeatOne(long score, int start, int end)
		{
			int num = (end - start) / 2 + start;
			Beat_beat beanById = GetBeanById(num);
			if (beanById == null)
			{
				return null;
			}
			if (start == end)
			{
				return beanById;
			}
			if (score <= beanById.Score)
			{
				return GetBeatOne(score, start, num);
			}
			return GetBeatOne(score, num + 1, end);
		}
	}
}
