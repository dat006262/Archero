




public class AI5002 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooserandom";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetHaveHatred;
		actionChooseRandom2.AddAction(20, GetActionSequence(5005, 1500));
		actionChooseRandom2.AddAction(20, GetActionSequence(5006, 1500));
		actionChooseRandom2.AddAction(20, GetActionSequence(5007, 1500));
		AddAction(GetActionWaitRandom("actionwait1", 200, 500));
		AddAction(actionChooseRandom2);
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionSequence(int attackId, int waitTime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.AddAction(GetActionAttack("actionattack", attackId));
		actionSequence.AddAction(GetActionWait("actionwait", waitTime));
		return actionSequence;
	}
}
