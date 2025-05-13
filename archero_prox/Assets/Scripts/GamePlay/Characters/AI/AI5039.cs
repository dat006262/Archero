




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AI5039 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 3);

	private List<GameObject> mPrevs = new List<GameObject>();

	private List<Vector3> poslist = new List<Vector3>();

	private int count = 30;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
	}

	protected override void OnInit()
	{
		switch (mWeightRandom.GetRandom())
		{
		case 0:
			AddAction(GetActionDelegate(delegate
			{
				CachePrevs();
				poslist.Clear();
				for (int j = 0; j < count; j++)
				{
					GameObject gameObject = GameLogic.EffectGet("Game/PrevEffect/prev_circle");
					Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition();
					gameObject.transform.position = vector;
					poslist.Add(vector);
					mPrevs.Add(gameObject);
				}
			}));
			AddAction(GetActionWait(string.Empty, 500));
			AddAction(GetActionDelegate(delegate
			{
				int i = 0;
				for (int num = poslist.Count; i < num; i++)
				{
					BulletSlopeBase bulletSlopeBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 5109, poslist[i] + new Vector3(0f, 20f, 0f), 0f) as BulletSlopeBase;
					bulletSlopeBase.SetEndPos(poslist[i]);
				}
			}));
			break;
		case 1:
			AddAttack(5108, -0.6f);
			break;
		case 2:
			AddAction(GetActionAttack(string.Empty, 5110, rotate: false));
			break;
		}
		AddAction(GetActionWaitRandom(string.Empty, 500, 1200));
		bReRandom = true;
	}

	private void CachePrevs()
	{
		int i = 0;
		for (int num = mPrevs.Count; i < num; i++)
		{
			GameLogic.EffectCache(mPrevs[i]);
		}
		mPrevs.Clear();
	}

	protected override void OnAIDeInit()
	{
		CachePrevs();
	}

	private void AddAttack(int attackid, float slowspeed)
	{
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", slowspeed);
		}));
		AddAction(GetActionAttack(string.Empty, attackid, rotate: false));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f - slowspeed);
		}));
	}
}
