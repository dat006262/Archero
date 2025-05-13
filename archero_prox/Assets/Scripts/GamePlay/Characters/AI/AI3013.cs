




public class AI3013 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(5, GetActionMoveOne(300, 600));
		if (m_Entity.IsElite)
		{
			actionChooseRandom2.AddAction(5, GetActionAttackWait(1090, 1000, 1500));
		}
		AddAction(actionChooseRandom2);
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionWaitRandom("actionwait1", 500, 1000));
		actionSequence2.AddAction(new AIMove1013(m_Entity));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait1", waittime, waitmaxtime));
		return actionSequence2;
	}
}
