




public class AI3006 : AIBase
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
			actionChooseRandom2.AddAction(10, new AIMove1008(m_Entity, 1f, 400));
		}
		else
		{
			actionChooseRandom2.AddAction(10, GetActionMoveOne());
		}
		actionChooseRandom2.AddAction(20, GetActionMoveTwo());
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
		actionSequence2.ConditionBase = (() => m_Entity.m_HatredTarget != null);
		actionSequence2.AddAction(GetActionAttack("actionattack2", m_Entity.m_Data.WeaponID));
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom2", 400, 600));
		return actionSequence2;
	}
}
