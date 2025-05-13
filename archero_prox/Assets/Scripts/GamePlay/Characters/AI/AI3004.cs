




public class AI3004 : AIBase
{
	protected override void OnElite()
	{
	}

	protected override void OnInit()
	{
		if (GameLogic.Hold.Guide.GetFlowerAttack())
		{
			if (!m_Entity.IsElite)
			{
				AddAction(GetActionWaitRandom("actionwaitr1", 300, 600));
				AddAction(GetActionWaitRandom("actionwaitr1", 300, 600));
				AddAction(GetActionAttack("actionattack", m_Entity.m_Data.WeaponID));
				AddAction(GetActionWaitRandom("actionwaitr2", 100, 500));
			}
			else
			{
				AddAction(GetActionWaitRandom("actionwaitr1", 200, 400));
				AddAction(GetActionAttack("actionattack", m_Entity.m_Data.WeaponID));
				AddAction(GetActionWaitRandom("actionwaitr2", 100, 300));
			}
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
