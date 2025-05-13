




public class AI3097 : AIBase
{
	protected override void OnInitOnce()
	{
		base.OnInitOnce();
	}

	protected override void OnInit()
	{
		if (m_Entity.IsElite)
		{
			AddAction(GetActionWaitRandom("actionwaitr1", 400, 800));
			AddAction(GetActionAttackSpecial("actionattack", m_Entity.m_Data.WeaponID));
			AddAction(GetActionWaitRandom("actionwaitr2", 200, 400));
			AddAction(GetActionWaitRandom("actionwaitr3", 300, 600));
		}
		else
		{
			AddAction(GetActionWaitRandom("actionwaitr1", 600, 1200));
			AddAction(GetActionAttackSpecial("actionattack", m_Entity.m_Data.WeaponID));
			AddAction(GetActionWaitRandom("actionwaitr2", 400, 800));
			AddAction(GetActionWaitRandom("actionwaitr3", 500, 1000));
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
