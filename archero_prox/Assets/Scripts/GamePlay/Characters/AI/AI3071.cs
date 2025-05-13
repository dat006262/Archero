




public class AI3071 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1045(m_Entity, 8f));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait", 300, 600));
		actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence3 = actionSequence;
		actionSequence3.AddAction(GetActionAttack("attack", m_Entity.m_Data.WeaponID));
		actionSequence3.AddAction(GetActionWaitRandom("actionwait", 300, 600));
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(20, actionSequence2);
		actionChooseRandom2.AddAction(10, actionSequence3);
		AddAction(actionChooseRandom2);
	}
}
