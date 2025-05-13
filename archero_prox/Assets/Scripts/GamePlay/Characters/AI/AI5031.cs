




using Dxx.Util;

public class AI5031 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(1, 4);

	private int ran;

	protected override void OnInit()
	{
		ran = mWeightRandom.GetRandom();
		switch (ran)
		{
		case 0:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence3 = actionSequence;
			if (MathDxx.RandomBool())
			{
				actionSequence3.AddAction(GetActionAttack("attack", 5073));
			}
			else
			{
				actionSequence3.AddAction(GetActionAttack("attack", 5074));
			}
			actionSequence3.AddAction(GetActionWaitRandom("actionwait", 500, 1000));
			AddAction(actionSequence3);
			break;
		}
		case 1:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence2 = actionSequence;
			actionSequence2.AddAction(GetActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 2f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 2f);
			}));
			int num = GameLogic.Random(4, 9);
			int num2 = GameLogic.Random(0, 2);
			for (int i = 0; i < num; i++)
			{
				int attackId = 5075 + num2;
				num2 = 1 - num2;
				actionSequence2.AddAction(GetActionAttack("attack", attackId));
			}
			actionSequence2.AddAction(GetActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -2f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -2f);
			}));
			actionSequence2.AddAction(GetActionWaitRandom("actionwait", 500, 1000));
			AddAction(actionSequence2);
			break;
		}
		case 2:
			AddAction(GetActionMoveOne(500, 1500));
			break;
		case 3:
			AddAction(new AIMove1054(m_Entity));
			AddAction(GetActionWaitRandom(string.Empty, 500, 800));
			AddAction(new AIMove1054(m_Entity));
			AddAction(GetActionWaitRandom(string.Empty, 500, 800));
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
