




public class AI3025 : AIBase
{
	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		if (m_Entity.IsElite)
		{
			m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 3500L);
		}
	}

	protected override void OnInit()
	{
		if (m_Entity.IsElite)
		{
			AddAction(new AIMove1022(m_Entity, 4.5f));
			AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 300, 300));
		}
		else
		{
			AddAction(new AIMove1022(m_Entity, 3.3f));
			AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 1000, 1000));
		}
	}

	protected override void OnAIDeInit()
	{
	}
}
