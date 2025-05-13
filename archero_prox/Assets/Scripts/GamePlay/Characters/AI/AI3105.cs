




public class AI3105 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1002(m_Entity, 500, 1000));
		AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
		AddAction(new AIMove1002(m_Entity, 500, 1000));
		AddAction(GetActionWaitRandom(string.Empty, 500, 1000));
	}

	protected override void OnAIDeInit()
	{
	}
}
