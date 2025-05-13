




public class AI5005 : AIBase
{
	protected override void OnInit()
	{
		ActionChooseRandom actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
		actionChooseRandom2.ConditionBase = base.GetIsAlive;
		ActionChooseIf actionChooseIf = new ActionChooseIf();
		actionChooseIf.m_Entity = m_Entity;
		ActionChooseIf actionChooseIf2 = actionChooseIf;
		ActionBase actionMoves = GetActionMoves(1);
		actionMoves.ConditionBase = GetHPMore40;
		actionChooseIf2.AddAction(actionMoves);
		ActionBase actionMoves2 = GetActionMoves(3);
		actionMoves2.ConditionBase = (() => GameLogic.Random(0, 100) < 34);
		actionChooseIf2.AddAction(actionMoves2);
		ActionBase actionMoves3 = GetActionMoves(4);
		actionMoves3.ConditionBase = (() => GameLogic.Random(0, 100) < 50);
		actionChooseIf2.AddAction(actionMoves3);
		ActionBase actionMoves4 = GetActionMoves(5);
		actionMoves4.ConditionBase = (() => true);
		actionChooseIf2.AddAction(actionMoves4);
		actionChooseRandom2.AddAction(5, actionChooseIf2);
		actionChooseRandom2.AddAction(5, GetActionAttackWait(5011, 3000, 4000));
		AddAction(actionChooseRandom2);
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
		actionSequence2.AddAction(new AIMove1019(m_Entity));
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
