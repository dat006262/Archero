




using Dxx.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TableTool
{
	public class Drop_DropModel : LocalModel<Drop_Drop, int>
	{
		[Serializable]
		public class DropData
		{
			public PropType type;

			public int id;

			public int count;

			public ulong uniqueid;

			[JsonIgnore]
			public Action OnClose;

			[JsonIgnore]
			public bool is_base_currency
			{
				get
				{
					if (type == PropType.eCurrency && (id == 1 || id == 2 || id == 3 || id == 4 || id == 21 || id == 22))
					{
						return true;
					}
					return false;
				}
			}

			[JsonIgnore]
			public bool can_fly
			{
				get
				{
					if (type == PropType.eCurrency && (id == 1 || id == 2 || id == 3))
					{
						return true;
					}
					return false;
				}
			}

			[JsonIgnore]
			public bool is_equipexp
			{
				get
				{
					if (type == PropType.eEquip && id >= 30101 && id <= 30104)
					{
						return true;
					}
					return false;
				}
			}

			public DropData()
			{
			}

			public DropData(PropType type, int id, int count)
			{
				this.type = type;
				this.id = id;
				this.count = count;
			}

			public bool Equals(DropData data)
			{
				return data != null && type == data.type && id == data.id;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override string ToString()
			{
				return Utils.FormatString("{0}:{1}!", id, count);
			}
		}

		public class DropSaveOneData
		{
			public int type;

			public int id;

			public int min;

			public int max;

			public int weight;

			public int count
			{
				get
				{
					if (min == max)
					{
						return min;
					}
					return RandomCount();
				}
			}

			public void Init(string value)
			{
				string[] array = value.Split(',');
				if (array.Length == 5)
				{
					int.TryParse(array[0], out type);
					int.TryParse(array[1], out id);
					int.TryParse(array[2], out min);
					int.TryParse(array[3], out max);
					int.TryParse(array[4], out weight);
				}
				else if (array.Length == 3)
				{
					int.TryParse(array[0], out type);
					int.TryParse(array[1], out id);
					int.TryParse(array[2], out min);
					max = min;
				}
			}

			public int RandomCount()
			{
				if (type == 1 && id == 2001)
				{
					float goldDropPercent = LocalModelManager.Instance.Drop_Drop.GetGoldDropPercent();
					int num = (int)((float)min * goldDropPercent);
					int num2 = (int)((float)max * goldDropPercent);
					return GameLogic.Random(num, num2 + 1);
				}
				return GameLogic.Random(min, max + 1);
			}
		}

		private class DropOneIDData
		{
			private Drop_Drop mDropData;

			private DropRandOne mFixedData = new DropRandOne();

			private List<DropRandOne> list = new List<DropRandOne>();

			public int DropType => mDropData.DropType;

			public int DropID => mDropData.DropID;

			public DropOneIDData(Drop_Drop data)
			{
				mDropData = data;
				if (DropType == 1)
				{
					int num = mDropData.Prob.Length;
					if (num >= 1)
					{
						DropRandOne dropRandOne = new DropRandOne();
						dropRandOne.RandomPercent = GetPercent(mDropData.Prob[0]);
						dropRandOne.AddOne(mDropData.Rand1);
						list.Add(dropRandOne);
					}
					if (num >= 2)
					{
						DropRandOne dropRandOne2 = new DropRandOne();
						dropRandOne2.RandomPercent = GetPercent(mDropData.Prob[1]);
						dropRandOne2.AddOne(mDropData.Rand2);
						list.Add(dropRandOne2);
					}
					if (num >= 3)
					{
						DropRandOne dropRandOne3 = new DropRandOne();
						dropRandOne3.RandomPercent = GetPercent(mDropData.Prob[2]);
						dropRandOne3.AddOne(mDropData.Rand3);
						list.Add(dropRandOne3);
					}
					if (num >= 4)
					{
						DropRandOne dropRandOne4 = new DropRandOne();
						dropRandOne4.RandomPercent = GetPercent(mDropData.Prob[3]);
						dropRandOne4.AddOne(mDropData.Rand4);
						list.Add(dropRandOne4);
					}
					if (num >= 5)
					{
						DropRandOne dropRandOne5 = new DropRandOne();
						dropRandOne5.RandomPercent = GetPercent(mDropData.Prob[4]);
						dropRandOne5.AddOne(mDropData.Rand5);
						list.Add(dropRandOne5);
					}
				}
				else if (DropType == 2)
				{
					mFixedData.AddOne(mDropData.Fixed);
				}
			}

			public List<DropData> GetRandomDrop()
			{
				if (DropType == 1)
				{
					List<DropData> list = new List<DropData>();
					int i = 0;
					for (int count = this.list.Count; i < count; i++)
					{
						DropData randomDrop = this.list[i].GetRandomDrop();
						if (randomDrop != null)
						{
							list.Add(randomDrop);
						}
					}
					return list;
				}
				if (DropType == 2)
				{
					return mFixedData.GetAllDrop();
				}
				SdkManager.Bugly_Report("Drop_DropModel_Extra.cs", Utils.FormatString("DropOneIDData.GetRandomDrop DropType:{0} is invalid!", DropType));
				return new List<DropData>();
			}

			private int GetPercent(string value)
			{
				if (!int.TryParse(value, out int result) && value.Contains("%"))
				{
					value = value.Substring(0, value.Length - 1);
					int.TryParse(value, out result);
				}
				return result;
			}
		}

		private class DropRandOne
		{
			public int RandomPercent;

			private int weight;

			private List<DropSaveOneData> list = new List<DropSaveOneData>();

			public void AddOne(string[] value)
			{
				int i = 0;
				for (int num = value.Length; i < num; i++)
				{
					DropSaveOneData dropSaveOneData = new DropSaveOneData();
					dropSaveOneData.Init(value[i]);
					weight += dropSaveOneData.weight;
					list.Add(dropSaveOneData);
				}
			}

			private bool IsDrop()
			{
				return GameLogic.Random(0, 10000) < RandomPercent;
			}

			public DropData GetRandomDrop()
			{
				if (!IsDrop())
				{
					return null;
				}
				int num = GameLogic.Random(0, weight);
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					DropSaveOneData dropSaveOneData = list[i];
					if (num < dropSaveOneData.weight)
					{
						return new DropData((PropType)dropSaveOneData.type, dropSaveOneData.id, dropSaveOneData.RandomCount());
					}
					num -= dropSaveOneData.weight;
				}
				return null;
			}

			public List<DropData> GetAllDrop()
			{
				List<DropData> list = new List<DropData>();
				int i = 0;
				for (int count = this.list.Count; i < count; i++)
				{
					DropSaveOneData dropSaveOneData = this.list[i];
					DropData item = new DropData((PropType)dropSaveOneData.type, dropSaveOneData.id, dropSaveOneData.RandomCount());
					list.Add(item);
				}
				return list;
			}
		}

		private const string _Filename = "Drop_Drop";

		private Dictionary<int, DropOneIDData> list = new Dictionary<int, DropOneIDData>();

		private int golddroproom = -1;

		private float golddroppercent = 1f;

		protected override string Filename => "Drop_Drop";

		protected override int GetBeanKey(Drop_Drop bean)
		{
			return bean.DropID;
		}

		public static DropSaveOneData GetDropOne(string str)
		{
			DropSaveOneData dropSaveOneData = new DropSaveOneData();
			dropSaveOneData.Init(str);
			return dropSaveOneData;
		}

		public static DropData GetDropData(string str)
		{
			DropData dropData = new DropData();
			string[] array = str.Split(',');
			int.TryParse(array[0], out int result);
			dropData.type = (PropType)result;
			int.TryParse(array[1], out dropData.id);
			int.TryParse(array[2], out dropData.count);
			return dropData;
		}

		public static List<DropData> GetDropDatas(string[] strs)
		{
			List<DropData> list = new List<DropData>();
			int i = 0;
			for (int num = strs.Length; i < num; i++)
			{
				list.Add(GetDropData(strs[i]));
			}
			return list;
		}

		public List<DropData> GetDropList(int dropid)
		{
			if (!list.TryGetValue(dropid, out DropOneIDData value))
			{
				value = new DropOneIDData(GetBeanById(dropid));
			}
			return value.GetRandomDrop();
		}

		public float GetGoldDropPercent()
		{
			if (GameLogic.Release != null && GameLogic.Release.Mode != null && GameLogic.Release.Mode.RoomGenerate != null)
			{
				int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
				if (golddroproom < currentRoomID)
				{
					golddroproom = currentRoomID;
					golddroppercent = LocalModelManager.Instance.Stage_Level_stagechapter.GetGoldDropPercent(currentRoomID);
				}
			}
			else
			{
				golddroppercent = 1f;
			}
			return golddroppercent;
		}

		public void ClearGoldDrop()
		{
			golddroproom = -1;
		}

		public int GetDropGold(List<DropData> list)
		{
			int num = 0;
			if (list == null)
			{
				return num;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eCurrency && list[i].id == 1)
				{
					num += list[i].count;
				}
			}
			return num;
		}

		public int GetDropDiamond(List<DropData> list)
		{
			int num = 0;
			if (list == null)
			{
				return num;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eCurrency && list[i].id == 2)
				{
					num += list[i].count;
				}
			}
			return num;
		}

		public int GetDropKey(List<DropData> list)
		{
			int num = 0;
			if (list == null)
			{
				return num;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eCurrency && list[i].id == 3)
				{
					num += list[i].count;
				}
			}
			return num;
		}

		public int GetDropExp(List<DropData> list)
		{
			int num = 0;
			if (list == null)
			{
				return num;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eCurrency && list[i].id == 1001)
				{
					num += list[i].count;
				}
			}
			return num;
		}

		public int GetDropDiamondBoxNormal(List<DropData> list)
		{
			int num = 0;
			if (list == null)
			{
				return num;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eCurrency && list[i].id == 21)
				{
					num += list[i].count;
				}
			}
			return num;
		}

		public int GetDropDiamondBoxLarge(List<DropData> list)
		{
			int num = 0;
			if (list == null)
			{
				return num;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eCurrency && list[i].id == 22)
				{
					num += list[i].count;
				}
			}
			return num;
		}

		public List<DropData> GetDropEquips(List<DropData> list)
		{
			List<DropData> list2 = new List<DropData>();
			if (list == null)
			{
				return list2;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].type == PropType.eEquip)
				{
					list2.Add(list[i]);
				}
			}
			return list2;
		}

		public List<DropData> GetDiamondBoxNormal()
		{
			bool flag = true;
			string key = Utils.FormatString("GetDiamondBox1_FirstGet_{0}", LocalSave.Instance.GetServerUserID());
			if (PlayerPrefsEncrypt.HasKey(key))
			{
				flag = false;
			}
			else
			{
				PlayerPrefsEncrypt.SetInt(key, 0);
			}
			int key2 = LocalSave.Instance.Stage_GetStage();
			Box_SilverNormalBox beanById = LocalModelManager.Instance.Box_SilverNormalBox.GetBeanById(key2);
			List<DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
			int position = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position;
			while (flag && position == 1)
			{
				dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
				position = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position;
			}
			return dropList;
		}

		public List<DropData> GetDiamondBoxLarge()
		{
			bool flag = true;
			string key = Utils.FormatString("GetDiamondBox1_FirstGet_{0}", LocalSave.Instance.GetServerUserID());
			if (PlayerPrefsEncrypt.HasKey(key))
			{
				flag = false;
			}
			else
			{
				PlayerPrefsEncrypt.SetInt(key, 0);
			}
			int key2 = LocalSave.Instance.Stage_GetStage();
			Box_SilverBox beanById = LocalModelManager.Instance.Box_SilverBox.GetBeanById(key2);
			List<DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
			int position = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position;
			while (flag && position == 1)
			{
				dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
				position = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position;
			}
			return dropList;
		}

		private void GetDiamondBox_ExcuteOne(List<DropData> list, int singleid, int giftid)
		{
			List<DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(singleid);
			int i = 0;
			for (int count = dropList.Count; i < count; i++)
			{
				list.Add(dropList[i]);
			}
			List<DropData> dropList2 = LocalModelManager.Instance.Drop_Drop.GetDropList(giftid);
			GetDiamondBox_ExcuteHave(list, dropList2);
		}

		private void GetDiamondBox_ExcuteHave(List<DropData> list, List<DropData> giftlist)
		{
			int i = 0;
			for (int count = giftlist.Count; i < count; i++)
			{
				bool flag = false;
				int j = 0;
				for (int count2 = list.Count; j < count2; j++)
				{
					if (list[j].Equals(giftlist[i]))
					{
						list[j].count += giftlist[i].count;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(giftlist[i]);
				}
			}
		}

		private void RandomList(List<DropData> list)
		{
			List<DropData> list2 = new List<DropData>();
			int num = list.Count - 1;
			while (num >= 0 && num < list.Count)
			{
				if (list[num].type != PropType.eEquip)
				{
					list2.Add(list[num]);
					list.RemoveAt(num);
				}
				num--;
			}
			list.RandomSort();
			int i = 0;
			for (int count = list2.Count; i < count; i++)
			{
				list.Add(list2[i]);
			}
		}
	}
}
