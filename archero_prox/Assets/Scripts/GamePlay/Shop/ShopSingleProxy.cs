




using PureMVC.Interfaces;
using PureMVC.Patterns;

public class ShopSingleProxy : Proxy, IProxy, INotifier
{
	public enum SingleType
	{
		eDiamond
	}

	public class Transfer
	{
		public SingleType type;
	}

	public new const string NAME = "ShopSingleProxy";

	public ShopSingleProxy(object data)
		: base("ShopSingleProxy", data)
	{
	}
}
