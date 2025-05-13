




using PureMVC.Patterns;

public class BattleProxy : Proxy
{
	public new const string NAME = "BattleProxy";

	public BattleProxy(object data)
		: base("BattleProxy", data)
	{
	}
}
