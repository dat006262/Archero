




public class AI1805 : AIBase
{
	protected override void OnInit()
	{
		AddAction(GetActionMoveTwo());
		AddAction(GetActionWaitRandom("actionwaitrandom2", 400, 600));
	}

	private ActionBase GetActionMoveTwo()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.ConditionBase = (() => m_Entity.m_HatredTarget != null);
		actionSequence2.AddAction(GetActionAttack("actionattack2", m_Entity.m_Data.WeaponID));
		return actionSequence2;
	}
}
