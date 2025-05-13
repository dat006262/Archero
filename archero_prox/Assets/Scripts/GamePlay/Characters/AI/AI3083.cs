




public class AI3083 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait", 1000, 1500));
		actionSequence2.AddAction(GetActionMoveOne(500, 1500));
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
