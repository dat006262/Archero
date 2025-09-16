




public class AIHero : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		actionChooseRandom2.AddAction(20, GetActionMoveOne());
		actionChooseRandom2.AddAction(10, GetActionAttack("actionattack2", 1001));
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
		actionSequence2.AddAction(new AIMove1002(m_Entity, GameLogic.Random(50, 150)));
		return actionSequence2;
	}
}
