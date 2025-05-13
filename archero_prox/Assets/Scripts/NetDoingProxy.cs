




using PureMVC.Patterns;

public class NetDoingProxy : Proxy
{
	public class Transfer
	{
		public NetDoingType type;
	}

	public new const string NAME = "NetDoingProxy";

	public NetDoingProxy(object data)
		: base("NetDoingProxy", data)
	{
	}
}
