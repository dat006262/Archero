




public class AI3026 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttackSpecial("attack", m_Entity.m_Data.WeaponID));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait", 1700, 3300));
		actionSequence2.AddAction(GetActionRemoveAttack());
		actionSequence2.AddAction(GetActionMoveOne(700, 1700));
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(10, actionSequence2);
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
		actionSequence2.AddAction(new AIMove1018(m_Entity, waittime, waitmaxtime));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait1", 200, 400));
		return actionSequence2;
	}
}
