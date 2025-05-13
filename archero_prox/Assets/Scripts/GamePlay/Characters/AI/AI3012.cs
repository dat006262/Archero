




public class AI3012 : AIBase
{
	protected override void OnInit()
	{
		if (m_Entity.IsElite)
		{
			int num = 200;
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence2 = actionSequence;
			actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, num, num));
			actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, num, num));
			actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, num, num));
			actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 1000, 1400));
			actionSequence2.AddAction(new AIMove1033(m_Entity, 0f, 0, move2target: false));
			AddAction(actionSequence2);
		}
		else
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
		actionSequence2.AddAction(new AIMove1052(m_Entity, 3));
		actionSequence2.AddAction(GetActionWait("actionwait1", 400));
		return actionSequence2;
	}

	private ActionBase GetActionAttackOne()
	{
		int num = 200;
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, num, num));
		actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, num, num));
		actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, num, num));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 1000, 1400));
		actionSequence2.AddAction(GetActionMoveOne());
		actionSequence2.AddAction(GetActionMoveOne());
		return actionSequence2;
	}
}
