




using Dxx.Util;
using UnityEngine;

public class AI5045 : AIGroundBase
{
	private int ran;

	private WeightRandomCount weight = new WeightRandomCount(1);

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		weight.Add(0, 10);
		weight.Add(1, 10);
	}

	protected override void OnInit()
	{
		ran = weight.GetRandom();
		switch (ran)
		{
		case 0:
			AddAction(GetActionAttacks(5121, 100, 100));
			break;
		case 1:
		{
			AIMove1026 aIMove = new AIMove1026(m_Entity, 4);
			aIMove.onDown = onCreateBullets;
			aIMove.onUp = onCreateBullets;
			AddAction(aIMove);
			break;
		}
		}
		bReRandom = true;
	}

	private void onCreateBullets()
	{
		int num = 6;
		float num2 = GameLogic.Random(0f, 360f);
		for (int i = 0; i < num; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 5122, m_Entity.position + new Vector3(0f, 1f, 0f), num2 + (float)i * 360f / (float)num);
		}
	}

	private ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = string.Empty;
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack(string.Empty, attackid));
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, attacktime, attackmaxtime));
		return actionSequence2;
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
