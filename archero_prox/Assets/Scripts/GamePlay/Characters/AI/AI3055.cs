




public class AI3055 : AIBase
{
	private int waittime;

	protected override void OnInit()
	{
		AddAction(new AIMove1022(m_Entity, 6f));
		AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, waittime, waittime));
		AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, waittime, waittime));
		AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, waittime, waittime));
	}

	protected override void OnAIDeInit()
	{
	}
}
