




using System.Collections.Generic;

namespace TableTool
{
	public class Stage_Level_chapter1Model : LocalModel<Stage_Level_chapter1, string>
	{
		public class Stage_LevelDataOne
		{
			public string[] Attriutes;

			public string[] MapAttriutes;

			public long StandardDefence;

			public string[] RoomIds;

			public string[] RoomIds1;

			public string[] GetRoomIds(int count)
			{
				if (count == 0 && RoomIds1.Length > 0)
				{
					return RoomIds1;
				}
				return RoomIds;
			}
		}

		public class Stage_LevelData
		{
			private List<Stage_LevelDataOne> mList = new List<Stage_LevelDataOne>();

			public int Stage
			{
				get;
				private set;
			}

			public int Count
			{
				get;
				private set;
			}

			public Stage_LevelData(int stage, int count)
			{
				Stage = stage;
				Count = count;
				mList.Add(null);
			}

			public void AddAttributes(string[] value, string[] mapsatt, long defence, string[] roomids, string[] roomids1)
			{
				int i = 0;
				for (int num = roomids.Length; i < num; i++)
				{
					roomids[i] = roomids[i].Replace("\n", string.Empty);
					roomids[i] = roomids[i].Replace(" ", string.Empty);
				}
				mList.Add(new Stage_LevelDataOne
				{
					Attriutes = value,
					MapAttriutes = mapsatt,
					StandardDefence = defence,
					RoomIds = roomids,
					RoomIds1 = roomids1
				});
			}

			public string[] GetAttributes(int level)
			{
				if (level < 1 || level > Count)
				{
					return new string[0];
				}
				return mList[level].Attriutes;
			}

			public string[] GetMapAttributes(int level)
			{
				if (level < 1 || level > Count)
				{
					return new string[0];
				}
				return mList[level].MapAttriutes;
			}

			public long GetDefence(int level)
			{
				if (level < 1 || level > Count)
				{
					return 100L;
				}
				return mList[level].StandardDefence;
			}

			public string[] GetRoomIds(int level, int count)
			{
				return mList[level].GetRoomIds(count);
			}
		}

		private const string _Filename = "Stage_Level_chapter1";

		private List<Stage_LevelData> list = new List<Stage_LevelData>();

		private bool bInit;

		protected override string Filename => "Stage_Level_chapter1";

		protected override string GetBeanKey(Stage_Level_chapter1 bean)
		{
			return bean.RoomID;
		}

		public Stage_LevelData GetStageLevel(int stage)
		{
			return list[stage];
		}

		public string[] GetStageLevel_Attributes(int stage, int level)
		{
			return list[stage].GetAttributes(level);
		}

		public string[] GetStageLevel_MapAttributes(int stage, int level)
		{
			return list[stage].GetMapAttributes(level);
		}

		public long GetStageLevel_Defence(int stage, int level)
		{
			return list[stage].GetDefence(level);
		}

		public int GetStageLevel_MaxLevel(int stage)
		{
			return GetStageLevel(stage).Count;
		}

		public bool IsMaxLevel(int stage, int level)
		{
			return GetStageLevel_MaxLevel(stage) <= level;
		}

		public string[] GetStageLevel_RoomIds(int stage, int level, int count)
		{
			return list[stage].GetRoomIds(level, count);
		}

		public int GetMaxStage()
		{
			return list.Count - 1;
		}

		public void Init()
		{
			list.Add(new Stage_LevelData(0, 0));
			IList<Stage_Level_chapter1> allBeans = LocalModelManager.Instance.Stage_Level_chapter1.GetAllBeans();
			int count = allBeans.Count;
			Stage_LevelData stage_LevelData = new Stage_LevelData(1, count);
			for (int i = 0; i < count; i++)
			{
				stage_LevelData.AddAttributes(allBeans[i].Attributes, allBeans[i].MapAttributes, allBeans[i].StandardDefence, allBeans[i].RoomIDs, allBeans[i].RoomIDs1);
			}
			list.Add(stage_LevelData);
			IList<Stage_Level_chapter2> allBeans2 = LocalModelManager.Instance.Stage_Level_chapter2.GetAllBeans();
			int count2 = allBeans2.Count;
			Stage_LevelData stage_LevelData2 = new Stage_LevelData(2, count2);
			for (int j = 0; j < count2; j++)
			{
				stage_LevelData2.AddAttributes(allBeans2[j].Attributes, allBeans2[j].MapAttributes, allBeans2[j].StandardDefence, allBeans2[j].RoomIDs, allBeans2[j].RoomIDs1);
			}
			list.Add(stage_LevelData2);
			IList<Stage_Level_chapter3> allBeans3 = LocalModelManager.Instance.Stage_Level_chapter3.GetAllBeans();
			int count3 = allBeans3.Count;
			Stage_LevelData stage_LevelData3 = new Stage_LevelData(3, count3);
			for (int k = 0; k < count3; k++)
			{
				stage_LevelData3.AddAttributes(allBeans3[k].Attributes, allBeans3[k].MapAttributes, allBeans3[k].StandardDefence, allBeans3[k].RoomIDs, allBeans3[k].RoomIDs1);
			}
			list.Add(stage_LevelData3);
			IList<Stage_Level_chapter4> allBeans4 = LocalModelManager.Instance.Stage_Level_chapter4.GetAllBeans();
			int count4 = allBeans4.Count;
			Stage_LevelData stage_LevelData4 = new Stage_LevelData(4, count4);
			for (int l = 0; l < count4; l++)
			{
				stage_LevelData4.AddAttributes(allBeans4[l].Attributes, allBeans4[l].MapAttributes, allBeans4[l].StandardDefence, allBeans4[l].RoomIDs, allBeans4[l].RoomIDs1);
			}
			list.Add(stage_LevelData4);
			IList<Stage_Level_chapter5> allBeans5 = LocalModelManager.Instance.Stage_Level_chapter5.GetAllBeans();
			int count5 = allBeans5.Count;
			Stage_LevelData stage_LevelData5 = new Stage_LevelData(5, count5);
			for (int m = 0; m < count5; m++)
			{
				stage_LevelData5.AddAttributes(allBeans5[m].Attributes, allBeans5[m].MapAttributes, allBeans5[m].StandardDefence, allBeans5[m].RoomIDs, allBeans5[m].RoomIDs1);
			}
			list.Add(stage_LevelData5);
			IList<Stage_Level_chapter6> allBeans6 = LocalModelManager.Instance.Stage_Level_chapter6.GetAllBeans();
			int count6 = allBeans6.Count;
			Stage_LevelData stage_LevelData6 = new Stage_LevelData(6, count6);
			for (int n = 0; n < count6; n++)
			{
				stage_LevelData6.AddAttributes(allBeans6[n].Attributes, allBeans6[n].MapAttributes, allBeans6[n].StandardDefence, allBeans6[n].RoomIDs, allBeans6[n].RoomIDs1);
			}
			list.Add(stage_LevelData6);
			IList<Stage_Level_chapter7> allBeans7 = LocalModelManager.Instance.Stage_Level_chapter7.GetAllBeans();
			int count7 = allBeans7.Count;
			Stage_LevelData stage_LevelData7 = new Stage_LevelData(7, count7);
			for (int num = 0; num < count7; num++)
			{
				stage_LevelData7.AddAttributes(allBeans7[num].Attributes, allBeans7[num].MapAttributes, allBeans7[num].StandardDefence, allBeans7[num].RoomIDs, allBeans7[num].RoomIDs1);
			}
			list.Add(stage_LevelData7);
			IList<Stage_Level_chapter8> allBeans8 = LocalModelManager.Instance.Stage_Level_chapter8.GetAllBeans();
			int count8 = allBeans8.Count;
			Stage_LevelData stage_LevelData8 = new Stage_LevelData(8, count8);
			for (int num2 = 0; num2 < count8; num2++)
			{
				stage_LevelData8.AddAttributes(allBeans8[num2].Attributes, allBeans8[num2].MapAttributes, allBeans8[num2].StandardDefence, allBeans8[num2].RoomIDs, allBeans8[num2].RoomIDs1);
			}
			list.Add(stage_LevelData8);
			IList<Stage_Level_chapter9> allBeans9 = LocalModelManager.Instance.Stage_Level_chapter9.GetAllBeans();
			int count9 = allBeans9.Count;
			Stage_LevelData stage_LevelData9 = new Stage_LevelData(9, count9);
			for (int num3 = 0; num3 < count9; num3++)
			{
				stage_LevelData9.AddAttributes(allBeans9[num3].Attributes, allBeans9[num3].MapAttributes, allBeans9[num3].StandardDefence, allBeans9[num3].RoomIDs, allBeans9[num3].RoomIDs1);
			}
			list.Add(stage_LevelData9);
			IList<Stage_Level_chapter10> allBeans10 = LocalModelManager.Instance.Stage_Level_chapter10.GetAllBeans();
			int count10 = allBeans10.Count;
			Stage_LevelData stage_LevelData10 = new Stage_LevelData(10, count10);
			for (int num4 = 0; num4 < count10; num4++)
			{
				stage_LevelData10.AddAttributes(allBeans10[num4].Attributes, allBeans10[num4].MapAttributes, allBeans10[num4].StandardDefence, allBeans10[num4].RoomIDs, allBeans10[num4].RoomIDs1);
			}
			list.Add(stage_LevelData10);
			IList<Stage_Level_chapter11> allBeans11 = LocalModelManager.Instance.Stage_Level_chapter11.GetAllBeans();
			int count11 = allBeans11.Count;
			Stage_LevelData stage_LevelData11 = new Stage_LevelData(11, count11);
			for (int num5 = 0; num5 < count11; num5++)
			{
				stage_LevelData11.AddAttributes(allBeans11[num5].Attributes, allBeans11[num5].MapAttributes, allBeans11[num5].StandardDefence, allBeans11[num5].RoomIDs, allBeans11[num5].RoomIDs1);
			}
			list.Add(stage_LevelData11);
			IList<Stage_Level_chapter12> allBeans12 = LocalModelManager.Instance.Stage_Level_chapter12.GetAllBeans();
			int count12 = allBeans12.Count;
			Stage_LevelData stage_LevelData12 = new Stage_LevelData(12, count12);
			for (int num6 = 0; num6 < count12; num6++)
			{
				stage_LevelData12.AddAttributes(allBeans12[num6].Attributes, allBeans12[num6].MapAttributes, allBeans12[num6].StandardDefence, allBeans12[num6].RoomIDs, allBeans12[num6].RoomIDs1);
			}
			list.Add(stage_LevelData12);
		}
	}
}
