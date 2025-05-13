




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AI5029 : AIBase
{
	private WeightRandomCount mWeight = new WeightRandomCount(1);

	protected override void OnInitOnce()
	{
		mWeight.Add(1, 10);
		mWeight.Add(2, 15);
		mWeight.Add(3, 10);
	}

	protected override void OnInit()
	{
		switch (mWeight.GetRandom())
		{
		case 1:
			AddAction(GetActionAttack(string.Empty, 5062));
			AddAction(GetActionWaitRandom(string.Empty, 750, 1150));
			break;
		case 2:
			AddAction(GetActionAttack(string.Empty, 5078));
			AddAction(GetActionWaitRandom(string.Empty, 750, 1150));
			break;
		case 3:
			AddAction(GetActionDelegate(delegate
			{
				m_Entity.m_AniCtrl.SendEvent("Call");
			}));
			AddAction(GetActionWait(string.Empty, 750));
			AddAction(GetActionDelegate(delegate
			{
				List<Vector3> roundNotSame = GameLogic.Release.MapCreatorCtrl.GetRoundNotSame(m_Entity.position, 4, 2);
				for (int i = 0; i < roundNotSame.Count; i++)
				{
					BulletManager bullet = GameLogic.Release.Bullet;
					EntityBase entity = m_Entity;
					Vector3 vector = roundNotSame[i];
					float x = vector.x;
					Vector3 vector2 = roundNotSame[i];
					bullet.CreateBullet(entity, 5064, new Vector3(x, -1f, vector2.z), 0f);
				}
			}));
			AddAction(GetActionWait(string.Empty, 1350));
			break;
		}
		bReRandom = true;
	}
}
