




public class AI3065 : AIBase
{
	protected override void OnInitOnce()
	{
		base.OnInitOnce();
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1002(m_Entity, 1000, 1000));
		if (m_Entity.IsElite)
		{
			ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
			actionChooseRandom.m_Entity = m_Entity;
			ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
			actionChooseRandom2.AddAction(10, GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
			actionChooseRandom2.AddAction(10, GetActionAttack(string.Empty, 1101));
			AddAction(actionChooseRandom2);
		}
		else
		{
			AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
		}
	}

	protected override void OnAIDeInit()
	{
	}
}
