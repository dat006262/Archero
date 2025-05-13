




public class AI3002 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.ConditionBase = base.GetIsAlive;
		actionSequence.AddAction(new AIMove1004(m_Entity));
		ActionSequence actionSequence2 = new ActionSequence();
		actionSequence2.name = "actionseq1";
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitr3", 500, 800));
		actionSequence2.AddAction(actionSequence);
		AddAction(actionSequence2);
	}

	protected override void OnAIDeInit()
	{
	}
}
