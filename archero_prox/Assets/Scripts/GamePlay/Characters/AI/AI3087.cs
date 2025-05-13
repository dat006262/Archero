




public class AI3087 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1006(m_Entity, 1800, 2500));
		AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
	}

	protected override void OnAIDeInit()
	{
	}
}
