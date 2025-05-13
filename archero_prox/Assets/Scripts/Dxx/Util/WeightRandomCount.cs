




using System;
using System.Collections.Generic;

namespace Dxx.Util
{
	public class WeightRandomCount
	{
		private List<WeightRandomCountData> list = new List<WeightRandomCountData>();

		private int allweight;

		private int maxcontinuecount;

		private int ran;

		private int randomindex;

		public WeightRandomCount(int maxcontinuecount)
		{
			this.maxcontinuecount = maxcontinuecount;
		}

		public WeightRandomCount(int maxcontinuecount, int maxcount)
		{
			this.maxcontinuecount = maxcontinuecount;
			for (int i = 0; i < maxcount; i++)
			{
				Add(i, 1);
			}
		}

		public void Add(int id, int weight)
		{
			WeightRandomCountData weightRandomCountData = new WeightRandomCountData(id);
			weightRandomCountData.weight = weight;
			list.Add(weightRandomCountData);
			allweight += weight;
		}

		public int GetRandom()
		{
			ran = GameLogic.Random(0, allweight);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				WeightRandomCountData weightRandomCountData = list[i];
				if (ran < weightRandomCountData.weight)
				{
					if (weightRandomCountData.GetCanRandom(randomindex, maxcontinuecount))
					{
						weightRandomCountData.RandomSelf(++randomindex);
						return weightRandomCountData.id;
					}
					return GetRandom();
				}
				ran -= weightRandomCountData.weight;
			}
			throw new Exception(Utils.FormatString("WeightRandom.GetRandom Weight Random Error!!!"));
		}
	}
}
