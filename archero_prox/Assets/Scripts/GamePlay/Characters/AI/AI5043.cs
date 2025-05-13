




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AI5043 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(2);

	private int callid = 3033;

	private int maxcount = 8;

	private List<Vector3> list = new List<Vector3>();

	protected override void OnInitOnce()
	{
		int width = GameLogic.Release.MapCreatorCtrl.width;
		int height = GameLogic.Release.MapCreatorCtrl.height;
		list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(0, 0)));
		list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(0, height - 1)));
		list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(width - 1, 0)));
		list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(width - 1, height - 1)));
		mWeightRandom.Add(1, 20);
		mWeightRandom.Add(2, 10);
		mWeightRandom.Add(3, 10);
		InitCallData(callid, maxcount, int.MaxValue, 3, 1, 2);
	}

	protected override void OnInit()
	{
		switch (mWeightRandom.GetRandom())
		{
		case 0:
			AddAction(GetCall1());
			break;
		case 1:
			AddAction(GetCall2());
			break;
		case 2:
			AddActionDelegate(delegate
			{
				m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", 90000L);
			});
			AddAction(GetActionAttack(string.Empty, 5115));
			AddActionDelegate(delegate
			{
				m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", -90000L);
			});
			break;
		case 3:
			AddActionDelegate(delegate
			{
				m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", 90000L);
			});
			AddAction(GetActionAttack(string.Empty, 5116));
			AddActionDelegate(delegate
			{
				m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", -90000L);
			});
			break;
		}
		AIMove1031 aIMove = new AIMove1031(m_Entity, GameLogic.Random(1f, 2f));
		aIMove.peradd = 0.3f;
		AddAction(aIMove);
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private ActionBase GetCall1()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.ConditionBase1Data = callid;
		actionSequence.ConditionBase1 = base.GetCanCall;
		actionSequence.AddAction(GetActionCall(callid));
		actionSequence.AddAction(GetActionWait("actionwait", 500));
		return actionSequence;
	}

	private ActionBase GetCall2()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.ConditionBase1Data = callid;
		actionSequence.ConditionBase1 = base.GetCanCall;
		actionSequence.AddAction(GetActionDelegate(delegate
		{
			m_Entity.m_AniCtrl.SendEvent("Call");
		}));
		actionSequence.AddAction(GetActionWait(string.Empty, 600));
		actionSequence.AddAction(GetActionDelegate(delegate
		{
			int aliveCount = GetAliveCount(callid);
			int value = maxcount - aliveCount;
			value = MathDxx.Clamp(value, 0, list.Count);
			list.RandomSort();
			for (int i = 0; i < value; i++)
			{
				CallOneInternal(callid, list[i], showcalleffect: true);
				AddCallCount(callid);
			}
		}));
		actionSequence.AddAction(GetActionWait("actionwait", 1200));
		return actionSequence;
	}
}
