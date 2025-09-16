




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class EquipInfoModuleProxy : Proxy, IProxy, INotifier
{
	public enum InfoType
	{
		eNormal,
		eBuy
	}

	public class Transfer
	{
		public LocalSave.EquipOne one;

		public InfoType type;

		public BlackItemOnectrl buy_itemone;

		public Action<BlackItemOnectrl> buy_callback;

		public Action updatecallback;

		public Action wearcallback;
	}

	public new const string NAME = "EquipInfoModuleProxy";

	public EquipInfoModuleProxy(object data)
		: base("EquipInfoModuleProxy", data)
	{
	}
}
