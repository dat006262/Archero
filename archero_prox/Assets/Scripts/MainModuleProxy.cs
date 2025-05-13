




using PureMVC.Interfaces;
using PureMVC.Patterns;

public class MainModuleProxy : Proxy, IProxy, INotifier
{
	public new const string NAME = "MainModule";

	public MainModuleProxy()
		: base("MainModule")
	{
	}
}
