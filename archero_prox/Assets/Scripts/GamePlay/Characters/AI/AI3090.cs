




public class AI3090 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(10, GetActionMoveOne());
		if (m_Entity.IsElite)
		{
			actionChooseRandom2.AddAction(20, GetActionMoveElite());
		}
		else
		{
			actionChooseRandom2.AddAction(20, GetActionMoveTwo());
		}
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
		actionSequence2.AddAction(new AIMove1002(m_Entity, 600));
		actionSequence2.AddAction(GetActionWait("actionwait1", 400));
		return actionSequence2;
	}

	private ActionBase GetActionMoveTwo()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack("actionattack2", m_Entity.m_Data.WeaponID));
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom2", 200, 600));
		actionSequence2.AddAction(new AIMove1002(m_Entity, 400, 800));
		actionSequence2.AddAction(GetActionWait("actionwait1", 400));
		return actionSequence2;
	}

	private ActionBase GetActionMoveElite()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack("actionattack2", 1092));
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom2", 200, 400));
		actionSequence2.AddAction(new AIMove1002(m_Entity, 400, 800));
		actionSequence2.AddAction(GetActionWait("actionwait1", 200));
		return actionSequence2;
	}
}
