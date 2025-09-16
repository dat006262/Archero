




using PureMVC.Interfaces;
using PureMVC.Patterns;

public class CardLevelUpProxy : Proxy, IProxy, INotifier
{
	public new const string NAME = "CardLevelUpProxy";

	public CardLevelUpProxy(object data)
		: base("CardLevelUpProxy", data)
	{
	}
}
