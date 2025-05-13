




public class AI3069 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1044(m_Entity));
		AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
	}

	protected override void OnAIDeInit()
	{
	}
}
