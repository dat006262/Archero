




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class LevelUpProxy : Proxy, IProxy, INotifier
{
	public class Transfer
	{
		public int level;

		public Action onclose;
	}

	public new const string NAME = "LevelUpProxy";

	public LevelUpProxy(object data)
		: base("LevelUpProxy", data)
	{
	}
}
