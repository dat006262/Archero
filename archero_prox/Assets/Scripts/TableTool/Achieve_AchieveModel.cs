




using System.Collections.Generic;

namespace TableTool
{
	public class Achieve_AchieveModel : LocalModel<Achieve_Achieve, int>
	{
		private const string _Filename = "Achieve_Achieve";

		public Dictionary<int, List<int>> mList = new Dictionary<int, List<int>>();

		public Dictionary<int, List<int>> mLocalList = new Dictionary<int, List<int>>();

		protected override string Filename => "Achieve_Achieve";

		protected override int GetBeanKey(Achieve_Achieve bean)
		{
			return bean.ID;
		}

		public void Init()
		{
			IList<Achieve_Achieve> allBeans = GetAllBeans();
			int i = 0;
			for (int count = allBeans.Count; i < count; i++)
			{
				List<int> value = null;
				List<int> value2 = null;
				Achieve_Achieve achieve_Achieve = allBeans[i];
				if (!mList.TryGetValue(achieve_Achieve.Stage, out value))
				{
					value = new List<int>();
					mList.Add(achieve_Achieve.Stage, value);
				}
				if (!mLocalList.TryGetValue(achieve_Achieve.Stage, out value2))
				{
					value2 = new List<int>();
					mLocalList.Add(achieve_Achieve.Stage, value2);
				}
				if (!achieve_Achieve.IsGlobal)
				{
					value2.Add(achieve_Achieve.ID);
				}
				value.Add(achieve_Achieve.ID);
			}
		}

		public List<int> GetStageList(int stageid, bool haveglobal)
		{
			List<int> value = null;
			if (haveglobal && mList.TryGetValue(stageid, out value))
			{
				return value;
			}
			if (!haveglobal && mLocalList.TryGetValue(stageid, out value))
			{
				return value;
			}
			return new List<int>();
		}

		public int GetStage(int id)
		{
			return GetBeanById(id)?.Stage ?? 0;
		}
	}
}
