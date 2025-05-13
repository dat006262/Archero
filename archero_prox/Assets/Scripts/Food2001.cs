




using Dxx.Util;

public class Food2001 : FoodBase
{
	protected override void OnGetGoods(EntityBase entity)
	{
		string str = Utils.FormatString("{0} + {1}", "Gold", (int)data);
		entity.m_EntityData.ExcuteAttributes(str);
	}
}
