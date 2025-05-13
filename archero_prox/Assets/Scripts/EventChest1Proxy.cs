




using PureMVC.Patterns;

public class EventChest1Proxy : Proxy
{
	public new const string NAME = "EventChest1Proxy";

	public EventChest1Proxy(object data)
		: base("EventChest1Proxy", data)
	{
	}
}
