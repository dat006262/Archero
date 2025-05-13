




public class AI3091 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		if (m_Entity.IsElite)
		{
			actionChooseRandom2.AddAction(15, GetActionMoveOne(600, 200));
			actionChooseRandom2.AddAction(15, GetActionMoveTwo(1097, 400, 300));
		}
		else
		{
			actionChooseRandom2.AddAction(15, GetActionMoveOne(600, 400));
			actionChooseRandom2.AddAction(15, GetActionMoveTwo(m_Entity.m_Data.WeaponID, 400, 600));
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

	private ActionBase GetActionMoveOne(int movetime, int waittime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1002(m_Entity, movetime));
		actionSequence2.AddAction(GetActionWait("actionwait1", waittime));
		return actionSequence2;
	}

	private ActionBase GetActionMoveTwo(int bulletid, int waitmin, int waitmax)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack("actionattack2", bulletid));
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom2", waitmin, waitmax));
		return actionSequence2;
	}
}
