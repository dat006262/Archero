




using System.Collections.Generic;

namespace TableTool
{
	public class Stage_Level_activityModel : LocalModel<Stage_Level_activity, int>
	{
		public class ActivityTypeData
		{
			public int index;

			public int type;

			public Dictionary<int, int> list = new Dictionary<int, int>();

			public List<int> mIds = new List<int>();

			public ActivityTypeData(int type)
			{
				this.type = type;
			}

			public void Add(Stage_Level_activity value)
			{
				list.Add(value.Difficult - 1, value.ID);
				mIds.Add(value.ID);
				mIds.Sort((int a, int b) => (a < b) ? (-1) : 1);
			}

			public int GetCount(int index)
			{
				return LocalModelManager.Instance.Stage_Level_activity.GetBeanById(mIds[index]).Number;
			}

			public Stage_Level_activity GetData(int index)
			{
				return LocalModelManager.Instance.Stage_Level_activity.GetBeanById(mIds[index]);
			}
		}

		private const string _Filename = "Stage_Level_activity";

		private Dictionary<int, ActivityTypeData> mList = new Dictionary<int, ActivityTypeData>();

		private List<ActivityTypeData> mList2 = new List<ActivityTypeData>();

		private List<Stage_Level_activity> mChallengeList = new List<Stage_Level_activity>();

		protected override string Filename => "Stage_Level_activity";

		protected override int GetBeanKey(Stage_Level_activity bean)
		{
			return bean.ID;
		}

		public void Init()
		{
			InitActive();
			InitChallenge();
		}

		private void InitActive()
		{
			IList<Stage_Level_activity> allBeans = GetAllBeans();
			for (int i = 0; i < allBeans.Count; i++)
			{
				Stage_Level_activity stage_Level_activity = allBeans[i];
				if (stage_Level_activity.ID < 2000)
				{
					if (!mList.ContainsKey(stage_Level_activity.Type))
					{
						ActivityTypeData activityTypeData = new ActivityTypeData(stage_Level_activity.Type);
						activityTypeData.Add(stage_Level_activity);
						mList.Add(stage_Level_activity.Type, activityTypeData);
						mList2.Add(activityTypeData);
					}
					else
					{
						mList[stage_Level_activity.Type].Add(stage_Level_activity);
					}
				}
			}
			mList2.Sort((ActivityTypeData a, ActivityTypeData b) => (a.type < b.type) ? (-1) : 1);
			for (int j = 0; j < mList2.Count; j++)
			{
				mList2[j].index = j;
			}
		}

		public List<ActivityTypeData> GetDifficults()
		{
			return mList2;
		}

		private void InitChallenge()
		{
			int num = 2101;
			for (Stage_Level_activity beanById = GetBeanById(num); beanById != null; beanById = GetBeanById(num))
			{
				mChallengeList.Add(beanById);
				num++;
			}
		}

		public List<Stage_Level_activity> GetChallengeList()
		{
			return mChallengeList;
		}
	}
}
