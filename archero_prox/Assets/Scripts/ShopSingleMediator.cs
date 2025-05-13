




using System.Collections.Generic;

public class ShopSingleMediator : MediatorBase
{
	public new const string NAME = "ShopSingleMediator";

	public override List<string> OnListNotificationInterests
	{
		get
		{
			List<string> onListNotificationInterests = base.OnListNotificationInterests;
			onListNotificationInterests.Add("ShopUI_Update");
			return onListNotificationInterests;
		}
	}

	public ShopSingleMediator()
		: base("ShopSingleUIPanel")
	{
	}
}
