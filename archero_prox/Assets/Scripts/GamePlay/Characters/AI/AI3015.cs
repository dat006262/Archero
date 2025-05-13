




public class AI3015 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1015(m_Entity));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait", 300, 600));
		actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence3 = actionSequence;
		actionSequence3.AddAction(GetActionAttack("attack", m_Entity.m_Data.WeaponID));
		actionSequence3.AddAction(GetActionWaitRandom("actionwait", 600, 1000));
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(10, actionSequence2);
		if (m_Entity.IsElite)
		{
			actionChooseRandom2.AddAction(15, actionSequence3);
			actionChooseRandom2.AddAction(5, GetBulletSeq());
		}
		else
		{
			actionChooseRandom2.AddAction(20, actionSequence3);
		}
		AddAction(actionChooseRandom2);
	}

	private ActionSequence GetBulletSeq()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack(string.Empty, 5062));
		actionSequence2.AddAction(GetActionWait(string.Empty, 1350));
		return actionSequence2;
	}
}
