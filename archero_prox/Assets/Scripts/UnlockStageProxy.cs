




using PureMVC.Interfaces;
using PureMVC.Patterns;

public class UnlockStageProxy : Proxy, IProxy, INotifier
{
	public class Transfer
	{
		public int StageID;
	}

	public new const string NAME = "UnlockStageProxy";

	public UnlockStageProxy(object data)
		: base("UnlockStageProxy", data)
	{
	}
}
