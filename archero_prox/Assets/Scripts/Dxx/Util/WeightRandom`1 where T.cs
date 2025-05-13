




using System;
using System.Collections.Generic;

namespace Dxx.Util
{
	public class WeightRandom<T> where T : WeightRandomDataBase
	{
		private List<T> list = new List<T>();

		private int allweight;

		private int ran;

		public void Add(T t, int weight)
		{
			t.weight = weight;
			list.Add(t);
			allweight += weight;
		}

		public int GetAllWeight()
		{
			return allweight;
		}

		public T GetRandom()
		{
			ran = GameLogic.Random(0, allweight);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				T val = list[i];
				if (ran < val.weight)
				{
					return val;
				}
				ran -= val.weight;
			}
			throw new Exception(Utils.FormatString("WeightRandom<{0}>.GetRandom Weight Random Error!!!", GetType().ToString()));
		}
	}
	public class WeightRandom
	{
		private List<WeightRandomDataBase> list = new List<WeightRandomDataBase>();

		private int allweight;

		private int ran;

		public void Add(int id, int weight)
		{
			WeightRandomDataBase weightRandomDataBase = new WeightRandomDataBase(id);
			weightRandomDataBase.weight = weight;
			list.Add(weightRandomDataBase);
			allweight += weight;
		}

		public int GetRandom()
		{
			ran = GameLogic.Random(0, allweight);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				WeightRandomDataBase weightRandomDataBase = list[i];
				if (ran < weightRandomDataBase.weight)
				{
					return weightRandomDataBase.id;
				}
				ran -= weightRandomDataBase.weight;
			}
			throw new Exception(Utils.FormatString("WeightRandom.GetRandom Weight Random Error!!!"));
		}

		public override string ToString()
		{
			string text = string.Empty;
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				text += Utils.FormatString("{0}:{1} ", list[i].id, list[i].weight);
			}
			return text;
		}
	}
}
