




public class AI5012 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		int num = GameLogic.Random(2, 5);
		for (int i = 0; i < num; i++)
		{
			actionSequence2.AddAction(GetActionSequence(5001 + GameLogic.Random(0, 4), 2000, 1000));
		}
		actionSequence2.AddAction(new AIMove1028(m_Entity));
		AddAction(GetActionWaitRandom("actionwait1", 200, 500));
		AddAction(actionSequence2);
		bReRandom = true;
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
		actionSequence.AddAction(new AIMove1018(m_Entity, 1000));
		actionSequence.AddAction(GetActionWait("actionwait", movetime));
		return actionSequence;
	}
}
