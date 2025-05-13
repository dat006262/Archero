




using Dxx.Util;

public class AI5046 : AIBase
{
	private WeightRandomCount mWeight = new WeightRandomCount(1);

	private int movecount = 3;

	protected override void OnInitOnce()
	{
		mWeight.Add(0, 10);
		mWeight.Add(1, 10);
		mWeight.Add(2, 10);
	}

	protected override void OnInit()
	{
		switch (mWeight.GetRandom())
		{
		case 0:
			AddAction(GetActionAttack(string.Empty, 5124));
			AddActionWait(1f);
			break;
		case 1:
			for (int j = 0; j < movecount; j++)
			{
				AddAction(GetActionMoveOne(200, 200));
			}
			AddActionWait(1f);
			if (MathDxx.RandomBool())
			{
				AddActionWait(0.5f);
				for (int k = 0; k < movecount; k++)
				{
					AddAction(GetActionMoveOne(200, 200));
				}
				AddActionWait(1f);
			}
			break;
		case 2:
			for (int i = 0; i < 3; i++)
			{
				AddAction(GetActionAttackWait(5127, 100, 100));
			}
			AddActionWait(0.5f);
			break;
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private bool GetHPMore40()
	{
		return m_Entity.m_EntityData.GetHPPercent() >= 0.4f;
	}

	private ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		AIMove1019 aIMove = new AIMove1019(m_Entity);
		aIMove.attackid = 5123;
		actionSequence2.AddAction(aIMove);
		actionSequence2.AddAction(GetActionWaitRandom("actionwait1", waittime, waitmaxtime));
		return actionSequence2;
	}

	private ActionBase GetActionMoves(int count)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		for (int i = 0; i < count; i++)
		{
			actionSequence2.AddAction(GetActionMoveOne(1000, 2000));
		}
		return actionSequence2;
	}
}
