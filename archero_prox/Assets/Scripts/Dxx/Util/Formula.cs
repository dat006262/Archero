




using TableTool;

namespace Dxx.Util
{
	public class Formula
	{
		public static float GetDefence(long defence)
		{
			return (float)defence / ((float)(defence + GameConfig.MapGood.GetStandardDefence()) + 100f);
		}

		public static long GetNeedDiamond(long gold)
		{
			int num = LocalSave.Instance.mShop.get_buy_golds(0);
			int num2 = LocalSave.Instance.mShop.get_buy_golds(1);
			int num3 = LocalSave.Instance.mShop.get_buy_golds(2);
			int num4 = LocalModelManager.Instance.Shop_Shop.get_buy_gold_diamond(0);
			int num5 = LocalModelManager.Instance.Shop_Shop.get_buy_gold_diamond(1);
			int num6 = LocalModelManager.Instance.Shop_Shop.get_buy_gold_diamond(2);
			if (gold <= num)
			{
				float num7 = (float)gold / (float)num;
				return MathDxx.CeilToInt(num7 * (float)num4);
			}
			if (gold <= num2)
			{
				long num8 = gold - num;
				float num9 = (float)num8 / (float)(num2 - num);
				return MathDxx.CeilToInt(num9 * (float)(num5 - num4)) + num4;
			}
			long num10 = gold - num2;
			float num11 = (float)num10 / (float)(num3 - num2);
			return MathDxx.CeilToInt(num11 * (float)(num6 - num5)) + num5;
		}
	}
}
