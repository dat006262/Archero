




public class AI5025 : AIBase
{
	private int callid = 3005;

	protected override void OnInitOnce()
	{
		InitCallData(callid, 2, int.MaxValue, 1, 2, 8);
	}

	protected override void OnInit()
	{
		int num = GameLogic.Random(0, 100);
		if (num < 40 && GetCanCall(callid))
		{
			AddAction(GetActionWait(string.Empty, 500));
			AddActionAddCall(callid, 4501);
		}
		else
		{
			AddAction(GetActionAttackOne());
		}
		AddAction(GetActionMoveOne());
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}

	private ActionBase GetActionMoveOne()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1002(m_Entity, 300, 600));
		actionSequence2.AddAction(GetActionWait("actionwait1", 200));
		return actionSequence2;
	}

	private ActionBase GetActionAttackOne()
	{
		int num = 400;
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		int num2 = GameLogic.Random(3, 6);
		for (int i = 0; i < num2; i++)
		{
			actionSequence2.AddAction(GetActionAttackWait(5060, num, num));
		}
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, 300, 500));
		actionSequence2.AddAction(GetActionMoveOne());
		return actionSequence2;
	}
}
