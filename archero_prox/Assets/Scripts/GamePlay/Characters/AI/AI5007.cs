




public class AI5007 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(10, GetActionMoveOne());
		actionChooseRandom2.AddAction(10, GetActionAttackOne());
		AddAction(actionChooseRandom2);
	}

	protected override void OnAIDeInit()
	{
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}

	private ActionBase GetActionMoveOne()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1025(m_Entity, 3));
		actionSequence2.AddAction(GetActionWait("actionwait1", 400));
		return actionSequence2;
	}

	private ActionBase GetActionAttackOne()
	{
		int num = 500;
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttackWait(5057, num, num));
		actionSequence2.AddAction(GetActionAttackWait(5057, num, num));
		actionSequence2.AddAction(GetActionAttackWait(5057, num, num));
		actionSequence2.AddAction(GetActionAttackWait(5057, num, num));
		actionSequence2.AddAction(GetActionAttackWait(5057, num, num));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 800, 800));
		actionSequence2.AddAction(GetActionMoveOne());
		return actionSequence2;
	}
}
