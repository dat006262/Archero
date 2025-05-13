




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class AI5033 : AIBase
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
			ActionSequence actionSequence4 = actionSequence;
			actionSequence4.AddAction(GetActionAttack(string.Empty, 5083));
			actionSequence4.AddAction(GetActionWaitRandom("actionwait", 500, 1000));
			AddAction(actionSequence4);
			break;
		}
		case 1:
		{
			ActionSequence actionSequence = new ActionSequence();
			actionSequence.name = "actionseq";
			actionSequence.m_Entity = m_Entity;
			ActionSequence actionSequence3 = actionSequence;
			actionSequence3.AddAction(GetActionAttack(string.Empty, 5085));
			actionSequence3.AddAction(GetActionWaitRandom("actionwait", 600, 1000));
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
				m_Entity.m_AniCtrl.SendEvent("Continuous");
				seq = DOTween.Sequence();
				int num = 30;
				for (int i = 0; i < num; i++)
				{
					seq.AppendCallback(delegate
					{
						float num2 = 0f;
						if ((bool)m_Entity.m_HatredTarget)
						{
							num2 = Utils.getAngle(m_Entity.m_HatredTarget.position - m_Entity.position);
						}
						num2 += (float)GameLogic.Random(-180, 180);
						float x = MathDxx.Sin(num2);
						float z = MathDxx.Cos(num2);
						GameLogic.Release.Bullet.CreateBullet(m_Entity, 5086, m_Entity.position + new Vector3(x, 0f, z) * 1.5f, num2);
					});
					seq.AppendInterval(0.06f);
				}
				seq.AppendCallback(delegate
				{
					m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
				});
			}));
			actionSequence2.AddAction(GetActionWaitRandom("actionwait", 2500, 3000));
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
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}
}
