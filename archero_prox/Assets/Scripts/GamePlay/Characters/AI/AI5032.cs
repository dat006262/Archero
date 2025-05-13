




using Dxx.Util;
using UnityEngine;

public class AI5032 : AIBase
{
	private WeightRandomCount weight = new WeightRandomCount(1, 4);

	private float attackadd = 0.3f;

	protected override void OnInitOnce()
	{
		bool flag = MathDxx.RandomBool();
		float num = -45f;
		if (flag)
		{
			num *= -1f;
		}
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5082, m_Entity.position + new Vector3(0f, 1f, 0f), num);
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5082, m_Entity.position + new Vector3(0f, 1f, 0f), num + 180f);
	}

	protected override void OnInit()
	{
		switch (weight.GetRandom())
		{
		case 0:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence4 = actionSequence;
			actionSequence4.AddAction(GetActionAttack(string.Empty, 5079));
			actionSequence4.AddAction(GetActionWaitRandom("actionwait", 1000, 1500));
			AddAction(actionSequence4);
			break;
		}
		case 1:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence3 = actionSequence;
			actionSequence3.AddAction(GetActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", attackadd);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", attackadd);
			}));
			actionSequence3.AddAction(GetActionAttack(string.Empty, 5080));
			actionSequence3.AddAction(GetActionAttack(string.Empty, 5081));
			actionSequence3.AddAction(GetActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0f - attackadd);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f - attackadd);
			}));
			actionSequence3.AddAction(GetActionWaitRandom("actionwait", 1000, 1500));
			AddAction(actionSequence3);
			break;
		}
		case 2:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence2 = actionSequence;
			actionSequence2.AddAction(GetActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", attackadd);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", attackadd);
			}));
			actionSequence2.AddAction(GetActionAttack(string.Empty, 5081));
			actionSequence2.AddAction(GetActionAttack(string.Empty, 5080));
			actionSequence2.AddAction(GetActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0f - attackadd);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f - attackadd);
			}));
			actionSequence2.AddAction(GetActionWaitRandom("actionwait", 1000, 1500));
			AddAction(actionSequence2);
			break;
		}
		case 3:
			AddAction(new AIMove1002(m_Entity, 500, 1000));
			break;
		}
		bReRandom = true;
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
