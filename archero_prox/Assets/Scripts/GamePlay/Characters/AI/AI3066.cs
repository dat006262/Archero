




public class AI3066 : AIBase
{
	protected override void OnInit()
	{
		if (!GameLogic.Hold.Guide.GetNeedGuide())
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.ConditionBase = Conditions;
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr1", 300, 600));
			actionSequence.AddAction(GetActionAttack("actionattack", 1048));
			actionSequence.AddAction(GetActionWaitRandom("actionwaitr2", 100, 500));
			ActionSequence actionSequence2 = new ActionSequence();
			actionSequence2.name = "actionseq1";
			actionSequence2.AddAction(GetActionWaitRandom("actionwaitr3", 300, 600));
			actionSequence2.AddAction(actionSequence);
			AddAction(actionSequence2);
		}
	}

	protected override void OnAIDeInit()
	{
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
