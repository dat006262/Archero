




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class AI5041 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(1, 2);

	private SequencePool mSeqPool = new SequencePool();

	private float yellow_prev_scale = 1f;

	private int yellow_count = 8;

	private float yellow_delay = 0.1f;

	private int blue_index;

	private float blue_startangle;

	private int red_count = 11;

	private GameObject effect_color;

	private ThunderContinueMgr.ThunderContinueReceive receive;

	private Entity3097BaseCtrl mBaseCtrl;

	protected override void OnInitOnce()
	{
		mBaseCtrl = CInstance<BattleResourceCreator>.Instance.Get3097Base(m_Entity.m_Body.ZeroMask.transform);
		change_color("3097_blue");
		mSeqPool.Get().AppendInterval(3f).AppendCallback(delegate
		{
			deinit_receive();
			receive = ThunderContinueMgr.GetThunderContinue(new ThunderContinueMgr.ThunderContinueData
			{
				entity = m_Entity,
				bulletid = 5114,
				count = yellow_count,
				delay = yellow_delay,
				prev_scale = yellow_prev_scale
			});
		})
			.SetLoops(-1);
	}

	private void change_color(string value)
	{
		if (effect_color != null)
		{
			GameLogic.EffectCache(effect_color);
			effect_color = null;
		}
		effect_color = GameLogic.EffectGet(Utils.FormatString("Effect/Monster/{0}", value));
		effect_color.SetParentNormal(m_Entity.m_Body.Body);
		mBaseCtrl.SetTexture(value);
		m_Entity.m_Body.SetTexture(value);
	}

	protected override void OnInit()
	{
		AddAction(GetActionWait("actionwaitr1", 100));
		switch (mWeightRandom.GetRandom())
		{
		case 0:
		{
			int num2 = 3;
			for (int j = 0; j < 3; j++)
			{
				AddActionDelegate(delegate
				{
					change_color("3097_blue");
					blue_startangle = GameLogic.Random(0f, 360f);
					mSeqPool.Get().AppendCallback(delegate
					{
						float num3 = (float)blue_index * 36f + blue_startangle;
						float x = MathDxx.Sin(num3);
						float z = MathDxx.Cos(num3);
						GameLogic.Release.Bullet.CreateBullet(m_Entity, 1079, m_Entity.m_Body.LeftBullet.transform.position + new Vector3(x, 0f, z) * 0.5f, num3);
						blue_index++;
					}).AppendInterval(0.1f)
						.SetLoops(10);
				});
				if (j < num2 - 1)
				{
					AddAction(GetActionWait("actionwaitr1", 1000));
				}
			}
			AddAction(GetActionWait("actionwaitr1", 500));
			break;
		}
		case 1:
		{
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				AddAction(GetActionDelegate(delegate
				{
					change_color("3097_red");
					float num4 = GameLogic.Random(0f, 360f);
					for (int k = 0; k < red_count; k++)
					{
						int num5 = k;
						GameLogic.Release.Bullet.CreateBullet(m_Entity, 1080, m_Entity.m_Body.LeftBullet.transform.position, (float)num5 * 360f / (float)red_count + num4);
					}
				}));
				if (i < num - 1)
				{
					AddAction(GetActionWait("actionwaitr1", 600));
				}
			}
			AddAction(GetActionWait("actionwaitr1", 500));
			break;
		}
		}
		bReRandom = true;
	}

	private void deinit_receive()
	{
		if (receive != null)
		{
			receive.Deinit();
			receive = null;
		}
	}

	protected override void OnAIDeInit()
	{
		deinit_receive();
		mSeqPool.Clear();
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
