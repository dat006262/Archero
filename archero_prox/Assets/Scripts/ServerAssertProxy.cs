




using PureMVC.Patterns;

public class ServerAssertProxy : Proxy
{
	public class Transfer
	{
		public long assertendtime;
	}

	public new const string NAME = "ServerAssertProxy";

	public ServerAssertProxy(object data)
		: base("ServerAssertProxy", data)
	{
	}
}
