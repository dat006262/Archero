




public class AI5027 : AIBase
{
	public const int DivideCount = 6;

	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.ConditionBase = base.GetIsAlive;
		actionSequence.AddAction(new AIMove1004(m_Entity));
		ActionSequence actionSequence2 = new ActionSequence();
		actionSequence2.name = "actionseq1";
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitr3", 200, 400));
		actionSequence2.AddAction(actionSequence);
		AddAction(actionSequence2);
	}

	protected override void OnAIDeInit()
	{
	}

	protected override void OnDeadBefore()
	{
		Divide(3073, 6);
	}
}
