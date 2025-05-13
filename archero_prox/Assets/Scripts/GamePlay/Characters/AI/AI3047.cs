




public class AI3047 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1052(m_Entity, 3));
		AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 1500, 1500));
	}
}
