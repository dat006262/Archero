




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Achieve_Achieve : LocalBean
	{
		private List<Drop_DropModel.DropData> rewardlist = new List<Drop_DropModel.DropData>();

		public int ID
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public int Stage
		{
			get;
			private set;
		}

		public int GlobalType
		{
			get;
			private set;
		}

		public int UnlockType
		{
			get;
			private set;
		}

		public int ShowTypeArgs
		{
			get;
			private set;
		}

		public int CondType
		{
			get;
			private set;
		}

		public string[] CondTypeArgs
		{
			get;
			private set;
		}

		public string[] Rewards
		{
			get;
			private set;
		}

		public bool IsGlobal => GlobalType == 1;

		protected override bool ReadImpl()
		{
			ID = readInt();
			Index = readInt();
			Stage = readInt();
			GlobalType = readInt();
			UnlockType = readInt();
			ShowTypeArgs = readInt();
			CondType = readInt();
			CondTypeArgs = readArraystring();
			Rewards = readArraystring();
			return true;
		}

		public Achieve_Achieve Copy()
		{
			Achieve_Achieve achieve_Achieve = new Achieve_Achieve();
			achieve_Achieve.ID = ID;
			achieve_Achieve.Index = Index;
			achieve_Achieve.Stage = Stage;
			achieve_Achieve.GlobalType = GlobalType;
			achieve_Achieve.UnlockType = UnlockType;
			achieve_Achieve.ShowTypeArgs = ShowTypeArgs;
			achieve_Achieve.CondType = CondType;
			achieve_Achieve.CondTypeArgs = CondTypeArgs;
			achieve_Achieve.Rewards = Rewards;
			return achieve_Achieve;
		}

		public List<Drop_DropModel.DropData> GetRewards()
		{
			if (rewardlist.Count == 0)
			{
				for (int i = 0; i < Rewards.Length; i++)
				{
					string[] array = Rewards[i].Split(':');
					if (array.Length != 2)
					{
						SdkManager.Bugly_Report("LocalSave_Achieve.Achieve_Achieve", Utils.FormatString("achieveid:[{0}] Rewards.Length != 2 !!!", ID));
					}
					if (!int.TryParse(array[0], out int result))
					{
						SdkManager.Bugly_Report("LocalSave_Achieve.Achieve_Achieve", Utils.FormatString("achieveid:[{0}] id is not a int type!!! ", ID));
					}
					if (!int.TryParse(array[1], out int result2))
					{
						SdkManager.Bugly_Report("LocalSave_Achieve.Achieve_Achieve", Utils.FormatString("achieveid:[{0}] count is not a int type!!! ", ID));
					}
					Drop_DropModel.DropData dropData = new Drop_DropModel.DropData();
					dropData.type = PropType.eCurrency;
					dropData.id = result;
					dropData.count = result2;
					rewardlist.Add(dropData);
				}
			}
			return rewardlist;
		}
	}
}
