




using Dxx.Util;
using UnityEngine;

public class AIBabyGroundBase : AIBabyBase
{
	protected int groundIndex = -1;

	protected override ActionBase GetAILogic()
	{
		if (groundIndex < 0)
		{
			UnityEngine.Debug.LogError(Utils.FormatString("AIBabyGroundBase[{0}] groundIndex == -1", GetType().ToString()));
		}
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.AddAction(new AIMoveBabyGround(m_Entity, groundIndex, 1.1f, 2f));
		ActionBase actionWait = GetActionWait(string.Empty, 500);
		actionWait.ConditionBase = (() => !FindTarget());
		actionSequence.AddAction(actionWait);
		actionSequence.AddAction(GetActionWait(string.Empty, 100));
		if (GameLogic.Hold.BattleData.Challenge_AttackEnable())
		{
			ActionBase actionAttack = GetActionAttack("actionattack", AttackID);
			actionAttack.ConditionBase = base.FindTarget;
			actionSequence.AddAction(actionAttack);
		}
		return actionSequence;
	}
}
