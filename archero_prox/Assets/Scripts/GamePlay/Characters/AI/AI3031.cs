




public class AI3031 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1052(m_Entity, 3));
		if (m_Entity.IsElite)
		{
			ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
			actionChooseRandom.m_Entity = m_Entity;
			ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
			actionChooseRandom2.AddAction(10, GetActionAttackWait(m_Entity.m_Data.WeaponID, 1000, 1000));
			actionChooseRandom2.AddAction(10, GetActionAttackWait(1100, 1000, 1000));
			AddAction(actionChooseRandom2);
		}
		else
		{
			AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 1500, 1500));
		}
	}
}
