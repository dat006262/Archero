




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;

public class RewardSimpleProxy : Proxy, IProxy, INotifier
{
	public class Transfer
	{
		public List<Drop_DropModel.DropData> list;
	}

	public new const string NAME = "RewardSimpleProxy";

	public RewardSimpleProxy(object data)
		: base("RewardSimpleProxy", data)
	{
	}
}
