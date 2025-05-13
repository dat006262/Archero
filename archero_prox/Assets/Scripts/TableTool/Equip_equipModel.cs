




using Dxx.Util;
using System.Collections.Generic;

namespace TableTool
{
	public class Equip_equipModel : LocalModel<Equip_equip, int>
	{
		private const string _Filename = "Equip_equip";

		private Dictionary<int, List<int>> mQualities = new Dictionary<int, List<int>>();

		private List<int> mEquipExps = new List<int>();

		protected override string Filename => "Equip_equip";

		protected override int GetBeanKey(Equip_equip bean)
		{
			return bean.Id;
		}

		public void Init()
		{
			IList<Equip_equip> allBeans = GetAllBeans();
			int i = 0;
			for (int count = allBeans.Count; i < count; i++)
			{
				Equip_equip equip_equip = allBeans[i];
				if (equip_equip.Overlying == 0)
				{
					add_quality(equip_equip.Id, equip_equip.Quality);
				}
				else
				{
					mEquipExps.Add(equip_equip.Id);
				}
			}
		}

		private void add_quality(int id, int quality)
		{
			if (mQualities.TryGetValue(quality, out List<int> value))
			{
				value.Add(id);
				return;
			}
			value = new List<int>();
			value.Add(id);
			mQualities.Add(quality, value);
		}

		public List<int> GetQuality(int quality)
		{
			if (mQualities.TryGetValue(quality, out List<int> value))
			{
				return value;
			}
			return null;
		}

		public int RandomByQuality(int quality)
		{
			List<int> quality2 = GetQuality(quality);
			if (quality2 == null)
			{
				return 0;
			}
			int index = GameLogic.Random(0, quality2.Count);
			return quality2[index];
		}

		public int RandomEquipExp()
		{
			int index = GameLogic.Random(0, mEquipExps.Count);
			return mEquipExps[index];
		}

		public List<Goods_goods.GoodData> GetEquipAttributes(LocalSave.EquipOne one)
		{
			List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();
			Equip_equip beanById = GetBeanById(one.EquipID);
			int level = one.Level;
			int i = 0;
			for (int num = beanById.Attributes.Length; i < num; i++)
			{
				Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById.Attributes[i]);
				if (goodData.percent)
				{
					goodData.value += (level - 1) * beanById.AttributesUp[i] * 100;
				}
				else
				{
					goodData.value += (level - 1) * beanById.AttributesUp[i];
				}
				list.Add(goodData);
			}
			return list;
		}

		public List<Goods_goods.GoodShowData> GetEquipShowAttrs(LocalSave.EquipOne one)
		{
			List<Goods_goods.GoodShowData> list = new List<Goods_goods.GoodShowData>();
			int num = one.Level;
			Equip_equip beanById = GetBeanById(one.EquipID);
			int i = 0;
			for (int num2 = beanById.Attributes.Length; i < num2; i++)
			{
				Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById.Attributes[i]);
				num = MathDxx.Clamp(num, 0, one.CurrentMaxLevel);
				if (goodData.percent)
				{
					goodData.value += (num - 1) * beanById.AttributesUp[i] * 100;
				}
				else
				{
					goodData.value += (num - 1) * beanById.AttributesUp[i];
				}
				goodData.value = (long)((float)goodData.value * (1f + GameLogic.SelfAttributeShow.GetUpPercent(one.Position)));
				Goods_goods.GoodShowData goodShowData = Goods_goods.GetGoodShowData(goodData);
				if (one.IsBaby && !goodData.goodType.Contains("EquipBaby:"))
				{
					string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Attr_BabyParentContent");
					goodShowData.goodType = Utils.FormatString("{0}{1}", languageByTID, goodShowData.goodType);
				}
				list.Add(goodShowData);
			}
			return list;
		}

		public List<string> GetEquipAttributesNext(LocalSave.EquipOne one)
		{
			LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
			equipOne.EquipID = one.EquipID;
			equipOne.Level = one.Level + 1;
			List<Goods_goods.GoodData> equipAttributes = GetEquipAttributes(one);
			List<Goods_goods.GoodData> equipAttributes2 = GetEquipAttributes(equipOne);
			List<string> list = new List<string>();
			Equip_equip beanById = GetBeanById(one.EquipID);
			int i = 0;
			for (int num = beanById.Attributes.Length; i < num; i++)
			{
				Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById.Attributes[i]);
				string empty = string.Empty;
				if (goodData.percent)
				{
					empty += Utils.FormatString("+ {0}%", beanById.AttributesUp[i]);
				}
				else
				{
					equipAttributes[i].value = (long)((float)equipAttributes[i].value * (1f + GameLogic.SelfAttributeShow.GetUpPercent(one.Position)));
					equipAttributes2[i].value = (long)((float)equipAttributes2[i].value * (1f + GameLogic.SelfAttributeShow.GetUpPercent(one.Position)));
					empty += Utils.FormatString("+ {0}", equipAttributes2[i].value - equipAttributes[i].value);
				}
				list.Add(empty);
			}
			return list;
		}

		public List<string> GetEquipShowAddAttributes(LocalSave.EquipOne one)
		{
			List<string> list = new List<string>();
			Equip_equip beanById = GetBeanById(one.EquipID);
			int i = 0;
			for (int num = beanById.AdditionSkills.Length; i < num; i++)
			{
				string empty = string.Empty;
				if (!int.TryParse(beanById.AdditionSkills[i], out int result))
				{
					string text = beanById.AdditionSkills[i];
					Goods_goods.GoodShowData goodShowData = Goods_goods.GetGoodShowData(text);
					if (one.IsBaby && !text.Contains("EquipBaby:"))
					{
						string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Attr_BabyParentContent");
						goodShowData.goodType = Utils.FormatString("{0}{1}", languageByTID, goodShowData.goodType);
					}
					empty = goodShowData.ToString();
				}
				else
				{
					empty = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("技能描述{0}", result));
				}
				list.Add(empty);
			}
			return list;
		}

		public List<string> GetEquipAddAttributes(LocalSave.EquipOne one)
		{
			List<string> list = new List<string>();
			Equip_equip data = one.data;
			int i = 0;
			for (int num = data.AdditionSkills.Length; i < num; i++)
			{
				string s = data.AdditionSkills[i];
				if (!int.TryParse(s, out int _))
				{
					list.Add(data.AdditionSkills[i]);
				}
			}
			return list;
		}

		public List<int> GetSkills(LocalSave.EquipOne one)
		{
			List<int> list = new List<int>();
			Equip_equip beanById = GetBeanById(one.EquipID);
			int i = 0;
			for (int num = beanById.AdditionSkills.Length; i < num; i++)
			{
				if (int.TryParse(beanById.AdditionSkills[i], out int result))
				{
					list.Add(result);
				}
			}
			return list;
		}

		public List<int> GetAdditionSkills_ui(LocalSave.EquipOne one)
		{
			List<int> list = new List<int>();
			Equip_equip beanById = GetBeanById(one.EquipID);
			int i = 0;
			for (int num = beanById.AdditionSkills.Length; i < num; i++)
			{
				if (int.TryParse(beanById.AdditionSkills[i], out int result))
				{
					list.Add(result);
				}
			}
			return list;
		}

		public List<int> GetListByPosition(int position)
		{
			List<int> list = new List<int>();
			IList<Equip_equip> allBeans = GetAllBeans();
			int i = 0;
			for (int count = allBeans.Count; i < count; i++)
			{
				Equip_equip equip_equip = allBeans[i];
				if (equip_equip.Position == position)
				{
					list.Add(equip_equip.Id);
				}
			}
			return list;
		}

		public int GetAttributeAllCount(int equipid)
		{
			Equip_equip beanById = GetBeanById(equipid);
			if (beanById == null)
			{
				return 0;
			}
			int num = beanById.Attributes.Length;
			int num2 = beanById.Skills.Length;
			return num + num2;
		}
	}
}
