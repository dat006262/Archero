




public class AI3014 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionRemoveMove());
		actionSequence2.AddAction(GetActionWait("actionwaitrandom", 1100));
		actionSequence2.AddAction(new AIMove1011(m_Entity, 600, 1500));
		int num = 1;
		if (m_Entity.IsElite)
		{
			num = 3;
		}
		for (int i = 0; i < num; i++)
		{
			actionSequence2.AddAction(new AIMove1014(m_Entity, 400));
			actionSequence2.AddAction(GetActionWait("actionwaitrandom", 400));
		}
		AddAction(actionSequence2);
	}

	protected override void OnAIDeInit()
	{
	}
}
