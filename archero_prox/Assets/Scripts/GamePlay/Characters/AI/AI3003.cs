




public class AI3003 : AIBase
{
	protected override void OnInit()
	{
		if (!m_Entity.IsElite)
		{
			AddAction(GetActionWaitRandom("actionwaitr1", 600, 1000));
			AddAction(GetActionWaitRandom("actionwaitr1", 300, 700));
			AddAction(GetActionAttack("actionattack", m_Entity.m_Data.WeaponID));
			AddAction(GetActionWaitRandom("actionwaitr2", 400, 600));
		}
		else
		{
			AddAction(GetActionWaitRandom("actionwaitr1", 600, 1000));
			AddAction(GetActionAttack("actionattack", m_Entity.m_Data.WeaponID));
			AddAction(GetActionWaitRandom("actionwaitr2", 200, 400));
		}
	}

	protected override void OnAIDeInit()
	{
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
