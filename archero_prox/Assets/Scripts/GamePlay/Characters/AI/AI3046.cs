




public class AI3046 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1022(m_Entity, 6f));
		AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 1000, 1000));
	}

	protected override void OnAIDeInit()
	{
	}
}
