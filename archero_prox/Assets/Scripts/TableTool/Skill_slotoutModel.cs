




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Skill_slotoutModel : LocalModel<Skill_slotout, int>
	{
		private const string _Filename = "Skill_slotout";

		protected override string Filename => "Skill_slotout";

		protected override int GetBeanKey(Skill_slotout bean)
		{
			return bean.GroupID;
		}

		public List<string> GetAttributes(LocalSave.CardOne one)
		{
			List<string> list = new List<string>();
			Skill_slotout beanById = GetBeanById(one.CardID);
			if (beanById != null)
			{
				if (beanById.BaseAttributes.Length != beanById.AddAttributes.Length)
				{
					SdkManager.Bugly_Report("Skill_slotoutModel_Extra", Utils.FormatString("GetAttributes[{0}] attributes is error.", one.CardID));
					return list;
				}
				int i = 0;
				for (int num = beanById.BaseAttributes.Length; i < num; i++)
				{
					string str = beanById.BaseAttributes[i];
					float num2 = beanById.AddAttributes[i];
					Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
					string text;
					if (goodData.percent)
					{
						num2 = (float)goodData.value + (float)(one.level - 1) * num2 * 100f;
						text = (num2 / 100f).ToString();
					}
					else
					{
						text = (goodData.value + (one.level - 1) * (long)num2).ToString();
					}
					str = Utils.FormatString("{0} {1} {2}", goodData.goodType, goodData.GetSymbolString(), text);
					list.Add(str);
				}
			}
			return list;
		}
	}
}
