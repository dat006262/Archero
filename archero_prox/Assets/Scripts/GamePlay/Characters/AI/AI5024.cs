




public class AI5024 : AIBase
{
	private bool call;

	private int count;

	private int range = 3;

	private int callid = 3003;

	protected override void OnInitOnce()
	{
		InitCallData(callid, 3, int.MaxValue, 1, 4, 10);
	}

	protected override void OnInit()
	{
		AddAction(GetActionWaitRandom("actionwait1", 600, 1000));
		call = (GameLogic.Random(0, 100) < 50);
		if (call && GetCanCall(callid))
		{
			AddActionAddCall(callid, 4501);
		}
		else
		{
			count = GameLogic.Random(2, 4);
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = string.Empty;
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence2 = actionSequence;
			for (int i = 0; i < count; i++)
			{
				actionSequence2.AddAction(GetActionSequence(5058, 50));
			}
			AddAction(actionSequence2);
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionSequence(int attackId, int waitTime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.AddAction(GetActionAttack("actionattack", attackId));
		actionSequence.AddAction(GetActionWait("actionwait", waitTime));
		return actionSequence;
	}
}
