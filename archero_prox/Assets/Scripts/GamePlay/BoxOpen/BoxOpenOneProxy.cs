




using PureMVC.Patterns;

public class BoxOpenOneProxy : Proxy
{
	public new const string NAME = "BoxOpenOneProxy";

	public BoxOpenOneProxy(object data)
		: base("BoxOpenOneProxy", data)
	{
	}
}
