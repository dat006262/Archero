




using PureMVC.Patterns;
using System;

public class AdInsideProxy : Proxy
{
	public enum EnterSource
	{
		eKey,
		eGameTurn
	}

	public class Transfer
	{
		public EnterSource source;

		public Action finish_callback;
	}

	public new const string NAME = "AdInsideProxy";

	public AdInsideProxy(object data)
		: base("AdInsideProxy", data)
	{
	}
}
