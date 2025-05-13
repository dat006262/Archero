




public class AI5006 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		int num = GameLogic.Random(1, 4);
		for (int i = 0; i < num; i++)
		{
			int attackid = GameLogic.Random(5012, 5015);
			actionSequence2.AddAction(GetActionAttacks(attackid, 100, 100));
		}
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.AddAction(10, new AIMove1023(m_Entity, 3));
		actionChooseRandom2.AddAction(10, new AIMove1024(m_Entity, 4));
		actionSequence2.AddAction(actionChooseRandom2);
		AddAction(actionSequence2);
		bReRandom = true;
	}

	private ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttackSpecial(string.Empty, attackid));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, attacktime, attackmaxtime));
		return actionSequence2;
	}
}
