




public class AI3106 : AIBase
{
	protected override void OnInit()
	{
		AddAction(GetActionMoveOne());
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionMoveOne()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMoveBomberman(m_Entity, 3));
		actionSequence2.AddAction(GetActionWait("actionwait1", 600));
		return actionSequence2;
	}
}
