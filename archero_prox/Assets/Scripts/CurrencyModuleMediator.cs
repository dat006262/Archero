




using System.Collections.Generic;

public class CurrencyModuleMediator : MediatorBase
{
	public new const string NAME = "CurrencyModuleMediator";

	public override List<string> OnListNotificationInterests
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("PUB_UI_UPDATE_CURRENCY");
			list.Add("UseCurrency");
			list.Add("GetCurrency");
			list.Add("UseCurrencyKey");
			list.Add("CurrencyKeyRotate");
			return list;
		}
	}

	public CurrencyModuleMediator()
		: base("CurrencyUIPanel")
	{
	}
}
