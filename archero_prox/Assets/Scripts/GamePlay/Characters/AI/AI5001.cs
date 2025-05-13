




public class AI5001 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooserandom";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetHaveHatred;
		actionChooseRandom2.AddAction(20, GetActionSequence(5001, 2000, 1000));
		actionChooseRandom2.AddAction(20, GetActionSequence(5002, 2000, 1000));
		actionChooseRandom2.AddAction(20, GetActionSequence(5003, 2500, 1000));
		actionChooseRandom2.AddAction(20, GetActionSequence(5004, 2500, 1000));
		AddAction(GetActionWaitRandom("actionwait1", 200, 500));
		AddAction(actionChooseRandom2);
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionSequence(int attackId, int waitTime, int movetime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.AddAction(GetActionAttack("actionattack", attackId));
		actionSequence.AddAction(GetActionWait("actionwait", waitTime));
		actionSequence.AddAction(new AIMove1018(m_Entity, movetime));
		return actionSequence;
	}
}
