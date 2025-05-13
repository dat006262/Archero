




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AI5038 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 3);

	private ThunderContinueMgr.ThunderContinueReceive receive;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
	}

	protected override void OnInit()
	{
		switch (mWeightRandom.GetRandom())
		{
		case 0:
		{
			AddAction(GetActionDelegate(delegate
			{
				m_Entity.m_AniCtrl.SendEvent("Skill");
			}));
			AddAction(GetActionWait(string.Empty, 1000));
			ActionParallel actionParallel = new ActionParallel();
			actionParallel.m_Entity = m_Entity;
			ActionParallel actionParallel2 = actionParallel;
			int width = GameLogic.Release.MapCreatorCtrl.width;
			List<bool> list = new List<bool>();
			int num = 7;
			for (int i = 0; i < num; i++)
			{
				list.Add(item: true);
			}
			for (int j = 0; j < width - num; j++)
			{
				list.Add(item: false);
			}
			list.RandomSort();
			for (int k = 0; k < width; k++)
			{
				if (list[k])
				{
					actionParallel2.Add(get_thunder(k));
				}
			}
			AddAction(actionParallel2);
			break;
		}
		case 1:
			AddAction(GetActionAttack(string.Empty, 5105, rotate: false));
			break;
		case 2:
			AddAction(GetActionDelegate(delegate
			{
				m_Entity.m_AniCtrl.SendEvent("Skill");
			}));
			AddAction(GetActionWait(string.Empty, 1000));
			AddAction(GetActionDelegate(delegate
			{
				if (receive != null)
				{
					receive.Deinit();
				}
				receive = ThunderContinueMgr.GetThunderContinue(new ThunderContinueMgr.ThunderContinueData
				{
					entity = m_Entity,
					bulletid = 5106,
					count = 4,
					delay = 0.15f
				});
			}));
			break;
		}
		AddAction(GetActionWaitRandom(string.Empty, 500, 1200));
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
		if (receive != null)
		{
			receive.Deinit();
		}
	}

	private ActionBase get_thunder(int row)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		int width = GameLogic.Release.MapCreatorCtrl.width;
		int height = GameLogic.Release.MapCreatorCtrl.height;
		for (int i = 0; i < height; i++)
		{
			int index = i;
			actionSequence2.AddAction(GetActionDelegate(delegate
			{
				Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(row, index);
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 1083, worldPosition, 0f);
			}));
			actionSequence2.AddAction(GetActionWait(string.Empty, 80));
		}
		return actionSequence2;
	}
}
