




public class AI3035 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1018(m_Entity, 500, 1500));
		AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 500));
	}
}
