




using Dxx.Util;

public class AI5028 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 2);

	protected override void OnInit()
	{
		switch (mWeightRandom.GetRandom())
		{
		case 0:
			AddAction(GetMove());
			break;
		case 1:
			AddAction(GetJump());
			break;
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetMove()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionRemoveMove());
		actionSequence2.AddAction(new AIMove1011(m_Entity, 600, 1000));
		EntityBase entityBase = GameLogic.FindTarget(m_Entity);
		if (entityBase != null)
		{
			actionSequence2.AddAction(GetActionRotateToEntity(entityBase));
		}
		actionSequence2.AddAction(new AIMove1048(m_Entity, 600));
		actionSequence2.AddAction(GetActionWait(string.Empty, 500));
		if (entityBase != null)
		{
			actionSequence2.AddAction(GetActionRotateToEntity(entityBase));
		}
		actionSequence2.AddAction(new AIMove1048(m_Entity, 600));
		actionSequence2.AddAction(GetActionWait(string.Empty, 500));
		if (entityBase != null)
		{
			actionSequence2.AddAction(GetActionRotateToEntity(entityBase));
		}
		actionSequence2.AddAction(new AIMove1048(m_Entity, 600));
		actionSequence2.AddAction(GetActionWait(string.Empty, 1000));
		return actionSequence2;
	}

	private ActionBase GetJump()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1049(m_Entity));
		actionSequence2.AddAction(GetActionWait(string.Empty, 1000));
		return actionSequence2;
	}
}
