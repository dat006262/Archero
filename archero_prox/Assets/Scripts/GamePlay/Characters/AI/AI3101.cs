




public class AI3101 : AIBase
{
	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		if (m_Entity.IsElite)
		{
			m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 7000L);
		}
	}

	protected override void OnInit()
	{
		if (m_Entity.IsElite)
		{
			AddAction(new AIMove1022(m_Entity, 4.2f));
			AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 400, 400));
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
