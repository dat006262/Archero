




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class EquipCombineProxy : Proxy, IProxy, INotifier
{
	public class Transfer
	{
		public Action onClose;
	}

	public new const string NAME = "EquipCombineProxy";

	public EquipCombineProxy(object data)
		: base("EquipCombineProxy", data)
	{
	}
}
