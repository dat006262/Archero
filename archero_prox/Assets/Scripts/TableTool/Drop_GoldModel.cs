




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Drop_GoldModel : LocalModel<Drop_Gold, int>
	{
		public class DropGold
		{
			public int Gold;
		}

		private class DropWeight
		{
			private Dictionary<int, DropWeightOne> mList = new Dictionary<int, DropWeightOne>();

			private WeightRandom mRandom = new WeightRandom();

			public void Init(string[] strs)
			{
				int i = 0;
				for (int num = strs.Length; i < num; i++)
				{
					string[] array = strs[i].Split(',');
					DropWeightOne dropWeightOne = new DropWeightOne();
					dropWeightOne.Count = int.Parse(array[0]);
					dropWeightOne.Weight = int.Parse(array[1]);
					dropWeightOne.Min = int.Parse(array[2]);
					dropWeightOne.Max = int.Parse(array[3]);
					Add(i, dropWeightOne);
				}
			}

			private void Add(int id, DropWeightOne one)
			{
				mList.Add(id, one);
				mRandom.Add(id, one.Weight);
			}

			public List<DropGold> GetDrops()
			{
				List<DropGold> list = new List<DropGold>();
				int random = mRandom.GetRandom();
				DropWeightOne dropWeightOne = mList[random];
				for (int i = 0; i < dropWeightOne.Count; i++)
				{
					DropGold dropGold = new DropGold();
					dropGold.Gold = GameLogic.Random(dropWeightOne.Min, dropWeightOne.Max + 1);
					list.Add(dropGold);
				}
				return list;
			}
		}

		private class DropWeightOne
		{
			public int Count;

			public int Weight;

			public int Min;

			public int Max;
		}

		private const string _Filename = "Drop_Gold";

		private Dictionary<int, DropWeight> mList = new Dictionary<int, DropWeight>();

		protected override string Filename => "Drop_Gold";

		protected override int GetBeanKey(Drop_Gold bean)
		{
			return bean.ID;
		}

		public List<DropGold> GetDropList(int dropid)
		{
			DropWeight dropWeight = GetDropWeight(dropid);
			return dropWeight.GetDrops();
		}

		private DropWeight GetDropWeight(int dropid)
		{
			if (mList.TryGetValue(dropid, out DropWeight value))
			{
				return value;
			}
			value = new DropWeight();
			value.Init(GetBeanById(dropid).GoldDropLevel);
			mList.Add(dropid, value);
			return value;
		}
	}
}
