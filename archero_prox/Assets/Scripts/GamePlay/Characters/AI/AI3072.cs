




public class AI3072 : AIBase
{
	private float angle;

	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.ConditionBase = Conditions;
		if (m_Entity.IsElite)
		{
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr1", 200, 400));
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr1", 100, 300));
			actionSequence.AddAction(GetActionDelegate(delegate
			{
				m_Entity.m_AttackCtrl.RotateHero(angle);
				angle += 30f;
				angle %= 360f;
			}));
			actionSequence.AddAction(GetActionAttack("actionattack", m_Entity.m_Data.WeaponID, rotate: false));
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr2", 100, 200));
		}
		else
		{
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr1", 300, 600));
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr1", 300, 600));
			actionSequence.AddAction(GetActionAttack("actionattack", m_Entity.m_Data.WeaponID));
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr2", 100, 500));
		}
		AddAction(actionSequence);
	}

	protected override void OnAIDeInit()
	{
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
