




using DG.Tweening;
using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AITowerCallBase : AIBase
{
	protected int callid;

	protected int callcount;

	protected int calldelay = 6000;

	protected float prev_scale = 1f;

	private SequencePool mSeqPool = new SequencePool();

	private float delay = 0.25f;

	private List<Vector3> poslist = new List<Vector3>();

	private List<GameObject> prevs = new List<GameObject>();

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		if (callid == 0)
		{
			SdkManager.Bugly_Report("AITowerCallBase", Utils.FormatString("{0}.callid == 0", GetType().ToString()));
		}
		if (callcount == 0)
		{
			SdkManager.Bugly_Report("AITowerCallBase", Utils.FormatString("{0}.callcount == 0", GetType().ToString()));
		}
	}

	protected override void OnInit()
	{
		int num = GameLogic.Random(500, 1500);
		AddAction(GetActionWait("actionwaitr1", num));
		AddAction(GetActionDelegate(delegate
		{
			mSeqPool.Clear();
			poslist.Clear();
			prevs.Clear();
			for (int i = 0; i < callcount; i++)
			{
				AITowerCallBase aITowerCallBase = this;
				int index = i;
				Sequence s = mSeqPool.Get();
				s.AppendInterval((float)index * delay);
				s.AppendCallback(delegate
				{
					Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition(index % 4);
					for (int j = 0; j < 20; j++)
					{
						if ((vector - aITowerCallBase.m_Entity.position).magnitude < 2f)
						{
							vector = GameLogic.Release.MapCreatorCtrl.RandomPosition(index % 4);
						}
					}
					aITowerCallBase.poslist.Add(vector);
					GameObject gameObject = GameLogic.EffectGet("Game/PrevEffect/TowerPrev_3057");
					gameObject.transform.position = aITowerCallBase.m_Entity.position;
					gameObject.transform.localScale = Vector3.one * aITowerCallBase.prev_scale;
					aITowerCallBase.prevs.Add(gameObject);
					gameObject.transform.DOMove(aITowerCallBase.poslist[index], aITowerCallBase.delay);
				});
				s.AppendInterval(1f);
				s.AppendCallback(delegate
				{
					aITowerCallBase.CallOneInternal(aITowerCallBase.callid, aITowerCallBase.poslist[index], showcalleffect: true);
				});
			}
		}));
		AddAction(GetActionWait("actionwaitr1", calldelay - num));
	}

	protected override void OnAIDeInit()
	{
		mSeqPool.Clear();
		int i = 0;
		for (int count = prevs.Count; i < count; i++)
		{
			prevs[i].transform.DOKill();
			GameLogic.EffectCache(prevs[i]);
		}
		prevs.Clear();
	}
}
