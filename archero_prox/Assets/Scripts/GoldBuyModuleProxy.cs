




using PureMVC.Patterns;
using System;

public class GoldBuyModuleProxy : Proxy
{
	public class Transfer
	{
		public CoinExchangeSource buytype;

		public long gold;

		public Action<int> callback;
	}

	public new const string NAME = "GoldBuy";

	public GoldBuyModuleProxy(object data)
		: base("GoldBuy", data)
	{
	}
}
