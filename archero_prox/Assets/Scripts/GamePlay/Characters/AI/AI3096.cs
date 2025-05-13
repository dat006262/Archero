




public class AI3096 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(15, GetActionMoveOne());
		actionChooseRandom2.AddAction(10, GetActionMoveTwo());
		if (m_Entity.IsElite)
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence2 = actionSequence;
			actionSequence2.AddAction(GetActionAttack(string.Empty, 1099));
			actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 900, 900));
			actionChooseRandom2.AddAction(10, actionSequence2);
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
		actionSequence2.AddAction(new AIMove1052(m_Entity, 3));
		return actionSequence2;
	}

	private ActionBase GetActionMoveTwo()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack("actionattack2", m_Entity.m_Data.WeaponID));
		actionSequence2.AddAction(GetActionAttack("actionattack2", m_Entity.m_Data.WeaponID, rotate: false));
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom2", 400, 600));
		return actionSequence2;
	}
}
