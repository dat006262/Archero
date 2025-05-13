




public class AI3048 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1052(m_Entity, 3));
		AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
	}
}
