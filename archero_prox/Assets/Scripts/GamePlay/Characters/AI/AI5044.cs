




using Dxx.Util;
using System.Collections.Generic;

public class AI5044 : AIGroundBase
{
	private int ran;

	private WeightRandomCount weight = new WeightRandomCount(1);

	private List<int> bulletids = new List<int>
	{
		5117,
		5118
	};

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		weight.Add(0, 10);
		weight.Add(1, 10);
		weight.Add(2, 10);
		weight.Add(3, 10);
	}

	protected override void OnInit()
	{
		ran = weight.GetRandom();
		switch (ran)
		{
		case 0:
		{
			int num2 = GameLogic.Random(2, 4);
			for (int j = 0; j < num2; j++)
			{
				AddAction(GetActionAttacks(get_random_bulletid(), 100, 100));
			}
			break;
		}
		case 1:
			AddAction(new AIMove1026(m_Entity, 4));
			break;
		case 2:
		{
			int num = 2;
			for (int i = 0; i < num; i++)
			{
				AddAction(GetActionAttacks(5119, 100, 100));
			}
			break;
		}
		case 3:
			AddAction(GetActionAttacks(5120, 100, 100));
			break;
		}
		bReRandom = true;
	}

	private int get_random_bulletid()
	{
		int index = GameLogic.Random(0, bulletids.Count);
		return bulletids[index];
	}

	private ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = string.Empty;
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack(string.Empty, attackid));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, attacktime, attackmaxtime));
		return actionSequence2;
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
