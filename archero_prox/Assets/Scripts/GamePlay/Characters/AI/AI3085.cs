




public class AI3085 : AIBase
{
	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		if (m_Entity.IsElite)
		{
			m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 10000L);
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", 0.5f);
		}
	}

	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		ActionChooseRandom actionChooseRandom;
		if (m_Entity.IsElite)
		{
			actionChooseRandom = new ActionChooseRandom();
			actionChooseRandom.m_Entity = m_Entity;
			ActionChooseRandom actionChooseRandom2 = actionChooseRandom;
			actionChooseRandom2.AddAction(10, GetLeftAttack());
			actionChooseRandom2.AddAction(10, GetActionAttack("attack", m_Entity.m_Data.WeaponID));
			actionSequence2.AddAction(actionChooseRandom2);
			actionSequence2.AddAction(GetActionWaitRandom("actionwait", 200, 400));
			actionSequence2.AddAction(GetActionMoveOne(250, 750));
		}
		else
		{
			actionSequence2.AddAction(GetActionAttack("attack", m_Entity.m_Data.WeaponID));
			actionSequence2.AddAction(GetActionWaitRandom("actionwait", 1000, 1500));
			actionSequence2.AddAction(GetActionMoveOne(500, 1500));
		}
		actionChooseRandom = new ActionChooseRandom();
		actionChooseRandom.name = "actionchooser";
		actionChooseRandom.m_Entity = m_Entity;
		ActionChooseRandom actionChooseRandom3 = actionChooseRandom;
		actionChooseRandom3.ConditionBase = base.GetIsAlive;
		actionChooseRandom3.AddAction(10, actionSequence2);
		AddAction(actionChooseRandom3);
	}

	private ActionBase GetLeftAttack()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.5f);
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.2f);
		}));
		actionSequence2.AddAction(GetActionAttack(string.Empty, 1091));
		actionSequence2.AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.5f);
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.2f);
		}));
		return actionSequence2;
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(new AIMove1018(m_Entity, waittime, waitmaxtime));
		actionSequence2.AddAction(GetActionWaitRandom("actionwait1", 200, 400));
		return actionSequence2;
	}
}
