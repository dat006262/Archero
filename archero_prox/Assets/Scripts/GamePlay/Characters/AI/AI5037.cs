




using Dxx.Util;

public class AI5037 : AIBase
{
	private WeightRandomCount weight = new WeightRandomCount(1, 3);

	protected override void OnInitOnce()
	{
		m_Entity.m_EntityData.ExcuteAttributes("ReboundWall", 1L);
		m_Entity.m_EntityData.ExcuteAttributes("ReboundWallMin", 1L);
		m_Entity.m_EntityData.ExcuteAttributes("ReboundWallMax", 1L);
	}

	protected override void OnInit()
	{
		switch (weight.GetRandom())
		{
		case 0:
			AddAction(GetActionSequence(5091, 2000, 1000));
			break;
		case 1:
			AddAction(GetActionSequence(5093, 2000, 1000));
			break;
		case 2:
			AddAction(GetActionSequence(5094, 2000, 1000));
			break;
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionSequence(int attackId, int waitTime, int movetime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.ConditionBase = base.GetHaveHatred;
		actionSequence.AddAction(GetActionAttack("actionattack", attackId));
		actionSequence.AddAction(GetActionWait("actionwait", waitTime));
		actionSequence.AddAction(new AIMove1018(m_Entity, movetime));
		return actionSequence;
	}
}
