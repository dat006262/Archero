




using System.Collections.Generic;

public class CharModuleMediator : MediatorBase
{
	public new const string NAME = "CharModuleMediator";

	public override List<string> OnListNotificationInterests => new List<string>();

	public CharModuleMediator()
		: base("CharacterUIPanel2")
	{
	}
}
