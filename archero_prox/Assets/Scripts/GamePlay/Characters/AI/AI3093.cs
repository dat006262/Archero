




public class AI3093 : AIBase
{
	protected override void OnInit()
	{
		AddAction(GetActionAttack());
	}

	private ActionBase GetActionAttack()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.ConditionBase = (() => m_Entity.m_HatredTarget != null);
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 200, 600));
		actionSequence2.AddAction(new AIMove1051(m_Entity, 1f, 0.4f));
		actionSequence2.AddAction(GetActionWait(string.Empty, 100));
		actionSequence2.AddAction(new AIMove1052(m_Entity, 3));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 300, 700));
		return actionSequence2;
	}
}
