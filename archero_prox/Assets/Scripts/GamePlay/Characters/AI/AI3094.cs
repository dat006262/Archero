




using Dxx.Util;

public class AI3094 : AIBase
{
	private WeightRandomCount weight = new WeightRandomCount(2);

	protected override void OnInitOnce()
	{
		weight.Add(0, 15);
		weight.Add(1, 10);
	}

	protected override void OnInit()
	{
		switch (weight.GetRandom())
		{
		case 0:
			AddAction(GetActionMoveOne());
			break;
		case 1:
			AddAction(GetActionMoveTwo());
			break;
		}
		bReRandom = true;
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
		actionSequence2.AddAction(new AIMove1052(m_Entity, 3));
		return actionSequence2;
	}

	private ActionBase GetActionMoveTwo()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq2";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.ConditionBase = (() => m_Entity.m_HatredTarget != null);
		actionSequence2.AddAction(GetActionAttack("actionattack2", m_Entity.m_Data.WeaponID));
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom2", 400, 600));
		return actionSequence2;
	}
}
