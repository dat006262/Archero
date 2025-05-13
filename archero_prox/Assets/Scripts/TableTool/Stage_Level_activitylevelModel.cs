




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Stage_Level_activitylevelModel : LocalModel<Stage_Level_activitylevel, string>
	{
		public class ActivityData
		{
			public int activityid;

			public int maxLayer;

			private string stagelevel;

			public ActivityData(int activityid)
			{
				stagelevel = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(activityid).StageLevel;
				this.activityid = activityid;
				maxLayer = 1;
				int num = 0;
				while (true)
				{
					if (num < 999)
					{
						if (LocalModelManager.Instance.Stage_Level_activitylevel.GetBeanById(GetID(maxLayer)) == null)
						{
							break;
						}
						maxLayer++;
						num++;
						continue;
					}
					return;
				}
				maxLayer--;
			}

			private string GetID(int id)
			{
				return Utils.FormatString("{0}{1:D3}", stagelevel, id);
			}
		}

		private const string _Filename = "Stage_Level_activitylevel";

		private Dictionary<int, ActivityData> mList = new Dictionary<int, ActivityData>();

		protected override string Filename => "Stage_Level_activitylevel";

		protected override string GetBeanKey(Stage_Level_activitylevel bean)
		{
			return bean.RoomID;
		}

		public void Init()
		{
		}

		private void InitActivityData(int activityid)
		{
			if (!mList.ContainsKey(activityid))
			{
				ActivityData value = new ActivityData(activityid);
				mList.Add(activityid, value);
			}
		}

		public int GetMaxLayer()
		{
			return GetMaxLayer(GameLogic.Hold.BattleData.ActiveID);
		}

		public int GetMaxLayer(int activityid)
		{
			InitActivityData(activityid);
			return mList[activityid].maxLayer;
		}
	}
}
