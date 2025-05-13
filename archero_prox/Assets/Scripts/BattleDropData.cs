




using Dxx.Util;

public class BattleDropData
{
	public FoodType type;

	public FoodOneType childtype;

	public object data;

	public BattleDropData(FoodType type, object data)
	{
		FoodOneType foodOneType = FoodOneType.eGold01;
		switch (type)
		{
		case FoodType.eGold:
		{
			int num = (int)data;
			foodOneType = ((num < 10) ? FoodOneType.eGold01 : ((num < 100) ? FoodOneType.eGold02 : ((num >= 1000) ? FoodOneType.eGold04 : FoodOneType.eGold03)));
			Init(type, foodOneType, data);
			break;
		}
		case FoodType.eEquip:
			Init(type, foodOneType, data);
			break;
		default:
			SdkManager.Bugly_Report("GameData.BattleDropData", Utils.FormatString("new BattleDropData type[{0}] is error!", type));
			break;
		}
	}

	public BattleDropData(FoodType type, FoodOneType childtype, object data)
	{
		Init(type, childtype, data);
	}

	private void Init(FoodType type, FoodOneType childtype, object data)
	{
		this.type = type;
		this.childtype = childtype;
		this.data = data;
	}

	public override string ToString()
	{
		return Utils.FormatString("FoodType:{0} FoodOneType:{1} data:{2}", type, childtype, data);
	}
}
