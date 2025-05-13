




using DG.Tweening;
using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AI5042 : AIBase
{
	private List<int> callids = new List<int>
	{
		3107,
		3109,
		3111
	};

	private int calldelay1;

	private int calldelay2 = 10000;

	private float prev_scale = 1f;

	private SequencePool mSeqPool = new SequencePool();

	private SequencePool mSeqPool2 = new SequencePool();

	private float delay = 0.25f;

	private List<Vector3> poslist = new List<Vector3>();

	private List<GameObject> prevs = new List<GameObject>();

	private List<BulletRedLineCtrl> mLines = new List<BulletRedLineCtrl>();

	private float startangle;

	private static float[] angles = new float[4]
	{
		40f,
		140f,
		220f,
		320f
	};

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		mSeqPool2.Get().AppendInterval(2f).AppendCallback(delegate
		{
			startangle = ((!MathDxx.RandomBool()) ? 45f : 0f);
			showlines(show: true);
		})
			.AppendInterval(0.6f)
			.AppendCallback(delegate
			{
				showlines(show: false);
				CreateBullets();
			})
			.SetLoops(-1);
	}

	protected override void OnInit()
	{
		AddAction(GetActionWait("actionwaitr1", calldelay1));
		AddAction(GetActionDelegate(delegate
		{
			mSeqPool.Clear();
			poslist.Clear();
			prevs.Clear();
			for (int i = 0; i < callids.Count; i++)
			{
				AI5042 aI = this;
				int index = i;
				Sequence s = mSeqPool.Get();
				s.AppendInterval((float)index * delay);
				s.AppendCallback(delegate
				{
					Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition(index % 4);
					for (int j = 0; j < 20; j++)
					{
						if ((vector - aI.m_Entity.position).magnitude < 2f)
						{
							vector = GameLogic.Release.MapCreatorCtrl.RandomPosition(index % 4);
						}
					}
					aI.poslist.Add(vector);
					GameObject gameObject = GameLogic.EffectGet("Game/PrevEffect/TowerPrev_3057");
					gameObject.transform.position = aI.m_Entity.position;
					gameObject.transform.localScale = Vector3.one * aI.prev_scale;
					aI.prevs.Add(gameObject);
					gameObject.transform.DOMove(aI.poslist[index], aI.delay);
				});
				s.AppendInterval(1f);
				s.AppendCallback(delegate
				{
					aI.CallOneInternal(aI.callids[index], aI.poslist[index], showcalleffect: true);
				});
			}
		}));
		AddAction(GetActionWait("actionwaitr1", calldelay2));
	}

	private void showlines(bool show)
	{
		if (mLines.Count == 0)
		{
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1102_RedLine"));
				gameObject.SetParentNormal(m_Entity.m_Body.LeftBullet.transform);
				BulletRedLineCtrl component = gameObject.GetComponent<BulletRedLineCtrl>();
				mLines.Add(component);
			}
		}
		if (show)
		{
			int j = 0;
			for (int count = mLines.Count; j < count; j++)
			{
				BulletRedLineCtrl bulletRedLineCtrl = mLines[j];
				bulletRedLineCtrl.gameObject.SetActive(value: true);
				if (startangle == 0f)
				{
					bulletRedLineCtrl.transform.rotation = Quaternion.Euler(0f, (float)j * 90f + startangle, 0f);
				}
				else
				{
					bulletRedLineCtrl.transform.rotation = Quaternion.Euler(0f, angles[j], 0f);
				}
				bulletRedLineCtrl.UpdateLine(throughinsidewall: false, 0.5f);
				bulletRedLineCtrl.PlayLineWidth();
			}
		}
		else
		{
			int k = 0;
			for (int count2 = mLines.Count; k < count2; k++)
			{
				mLines[k].gameObject.SetActive(value: false);
			}
		}
	}

	private void ClearLines()
	{
		for (int i = 0; i < mLines.Count; i++)
		{
			BulletRedLineCtrl bulletRedLineCtrl = mLines[i];
			if ((bool)bulletRedLineCtrl)
			{
				UnityEngine.Object.Destroy(bulletRedLineCtrl.gameObject);
			}
		}
		mLines.Clear();
	}

	private void CreateBullets()
	{
		for (int i = 0; i < 4; i++)
		{
			float num = 0f;
			num = ((startangle != 0f) ? angles[i] : ((float)i * 90f + startangle));
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 1102, m_Entity.m_Body.LeftBullet.transform.position, num);
		}
	}

	protected override void OnAIDeInit()
	{
		mSeqPool.Clear();
		mSeqPool2.Clear();
		int i = 0;
		for (int count = prevs.Count; i < count; i++)
		{
			prevs[i].transform.DOKill();
			GameLogic.EffectCache(prevs[i]);
		}
		prevs.Clear();
		ClearLines();
	}
}
