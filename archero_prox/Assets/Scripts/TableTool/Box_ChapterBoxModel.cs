




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Box_ChapterBoxModel : LocalModel<Box_ChapterBox, int>
	{
		private const string _Filename = "Box_ChapterBox";

		private int mMaxID;

		protected override string Filename => "Box_ChapterBox";

		protected override int GetBeanKey(Box_ChapterBox bean)
		{
			return bean.ID;
		}

		public List<Drop_DropModel.DropData> GetDrops(int id)
		{
			List<Drop_DropModel.DropData> result = new List<Drop_DropModel.DropData>();
			Box_ChapterBox beanById = GetBeanById(id);
			if (beanById == null)
			{
				SdkManager.Bugly_Report("Box_ChapterBoxModel_Extra", Utils.FormatString("GetDrops id[{0}] is invalid.", id));
				return result;
			}
			return Drop_DropModel.GetDropDatas(beanById.Reward);
		}

		public int GetNextLevel(int id)
		{
			return GetBeanById(id)?.Chapter ?? int.MaxValue;
		}

		public int GetOpenCount(int currentlayer, int openedcount)
		{
			InitMaxID();
			if (currentlayer < GetBeanById(1).Chapter)
			{
				return 0;
			}
			if (currentlayer >= GetBeanById(mMaxID).Chapter)
			{
				return mMaxID - openedcount;
			}
			for (int i = 1; i < mMaxID - 1; i++)
			{
				if (currentlayer >= GetBeanById(i).Chapter && currentlayer < GetBeanById(i + 1).Chapter)
				{
					return i - openedcount;
				}
			}
			return 0;
		}

		private void InitMaxID()
		{
			if (mMaxID <= 0)
			{
				int i;
				for (i = 1; GetBeanById(i) != null; i++)
				{
				}
				mMaxID = i - 1;
			}
		}

		public List<Box_ChapterBox> GetCurrentList()
		{
			List<Box_ChapterBox> list = new List<Box_ChapterBox>();
			for (int i = 0; i <= mMaxID; i++)
			{
				Box_ChapterBox beanById = GetBeanById(i);
				if (beanById != null)
				{
					list.Add(beanById);
				}
			}
			return list;
		}

		public Box_ChapterBox GetNext(int id)
		{
			return GetBeanById(id + 1);
		}
	}
}
