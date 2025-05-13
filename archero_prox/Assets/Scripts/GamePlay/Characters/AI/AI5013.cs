




public class AI5013 : AIBase
{
	private bool call;

	private int count;

	private int rangemin = 3;

	private int rangemax = 4;

	private int callid = 3004;

	protected override void OnInitOnce()
	{
		InitCallData(callid, 2, int.MaxValue, 1, rangemin, rangemax);
	}

	protected override void OnInit()
	{
		AddAction(GetActionWaitRandom("actionwait1", 600, 1000));
		call = (GameLogic.Random(0, 100) < 50);
		if (call && GetCanCall(callid) && GameLogic.Release.MapCreatorCtrl.GetCanCall(m_Entity, rangemin, rangemax))
		{
			AddAction(GetActionCall(callid));
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
				actionSequence2.AddAction(GetActionSequence(5025 + GameLogic.Random(0, 3), 50));
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
