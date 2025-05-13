




using Dxx.Util;

public class AI5010 : AIGroundBase
{
	private int ran;

	private WeightRandomCount weight = new WeightRandomCount(1);

	private int callid = 3044;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		weight.Add(0, 10);
		weight.Add(1, 10);
		weight.Add(2, 10);
		InitCallData(callid, 3, int.MaxValue, 2, 2, 3);
	}

	protected override void OnInit()
	{
		ran = weight.GetRandom();
		if (ran == 0)
		{
			AddAction(GetActionAttacks(5018, 200, 200));
		}
		else if (ran == 1)
		{
			AddAction(GetCall());
		}
		else
		{
			AddAction(new AIMove1026(m_Entity, 4));
			AddActionWait(0.1f);
			AddAction(GetActionAttacks(5018, 200, 200));
		}
		bReRandom = true;
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

	private ActionBase GetCall()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.ConditionBase1Data = callid;
		actionSequence.ConditionBase1 = base.GetCanCall;
		actionSequence.AddAction(GetActionCall(callid));
		actionSequence.AddAction(GetActionWait("actionwait", 500));
		return actionSequence;
	}
}
