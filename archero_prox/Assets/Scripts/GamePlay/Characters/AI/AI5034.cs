




using DG.Tweening;
using Dxx.Util;

public class AI5034 : AIBase
{
	private WeightRandomCount weight = new WeightRandomCount(2, 4);

	private float attackadd = 0.3f;

	private Sequence seq;

	protected override void OnInit()
	{
		switch (weight.GetRandom())
		{
		case 0:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence3 = actionSequence;
			actionSequence3.AddAction(GetActionAttack(string.Empty, 5087));
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
			actionSequence2.AddAction(GetActionAttack(string.Empty, 5088));
			actionSequence2.AddAction(GetActionWaitRandom("actionwait", 600, 1000));
			AddAction(actionSequence2);
			break;
		}
		case 2:
			AddAction(GetSprint());
			AddAction(GetSprint());
			AddAction(GetActionWaitRandom("actionwait", 500, 1000));
			break;
		case 3:
			AddAction(new AIMove1002(m_Entity, 700, 1400));
			break;
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	private ActionSequence GetSprint()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.3f);
		}));
		actionSequence2.AddAction(GetActionAttack(string.Empty, 5090));
		actionSequence2.AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.3f);
		}));
		return actionSequence2;
	}
}
