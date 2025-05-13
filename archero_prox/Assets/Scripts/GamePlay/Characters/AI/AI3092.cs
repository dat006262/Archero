




public class AI3092 : AIBase
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
		actionSequence2.AddAction(new AIMove1052(m_Entity, 3));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 600, 1200));
		return actionSequence2;
	}

	private ActionBase GetActionAttackOne()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		if (m_Entity.IsElite)
		{
			actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 400));
			actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 400));
		}
		else
		{
			actionSequence2.AddAction(GetActionAttackWait(m_Entity.m_Data.WeaponID, 1000));
		}
		actionSequence2.AddAction(new AIMove1052(m_Entity, 2));
		actionSequence2.AddAction(GetActionWait(string.Empty, 400));
		return actionSequence2;
	}
}
