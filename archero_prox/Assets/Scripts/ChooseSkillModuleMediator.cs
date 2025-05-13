




using System.Collections.Generic;

public class ChooseSkillModuleMediator : MediatorBase
{
	public new const string NAME = "ChooseSkillModuleMediator";

	public override List<string> OnListNotificationInterests
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("BATTLE_CHOOSESKILL_ACTION_END");
			list.Add("BATTLE_CHOOSESKILL_SKILL_CHOOSE");
			return list;
		}
	}

	public ChooseSkillModuleMediator()
		: base("ChooseSkillUIPanel")
	{
	}
}
