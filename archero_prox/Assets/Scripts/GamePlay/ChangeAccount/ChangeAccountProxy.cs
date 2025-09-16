




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class ChangeAccountProxy : Proxy, IProxy, INotifier
{
	public class Transfer
	{
		public Action callback_sure;

		public Action callback_confirm;
	}

	public new const string NAME = "ChangeAccountProxy";

	public ChangeAccountProxy(object data)
		: base("ChangeAccountProxy", data)
	{
	}
}
