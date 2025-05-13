




using PureMVC.Patterns;

public class PurChaseOKProxy : Proxy
{
	public class Transfer
	{
		public int purchase_state;

		public string id;

		public string receipt;
	}

	public new const string NAME = "PurChaseOKProxy";

	public PurChaseOKProxy(object data)
		: base("PurChaseOKProxy", data)
	{
	}
}
