




using Dxx.Util;
using UnityEngine;

public class AI5016 : AIBase
{
	private CallData[] calls = new CallData[2]
	{
		new CallData(3025, 1, int.MaxValue, 1, 2, 3),
		new CallData(3027, 1, int.MaxValue, 1, 2, 3)
	};

	private float[] hplimit = new float[2]
	{
		0.2f,
		0.5f
	};

	private bool[] hpused = new bool[2];

	private float recoverhp = 0.25f;

	private WeightRandomCount weight = new WeightRandomCount(1, 4);

	private int callid;

	private int attackcount;

	protected override void OnInitOnce()
	{
		IsDelayTime = false;
		m_Entity.mAniCtrlBase.SetDontPlayHittedAction();
		for (int i = 0; i < calls.Length; i++)
		{
			InitCallData(calls[i]);
		}
		m_Entity.AddSkill(1100013);
		AddFirst();
	}

	protected override void OnInit()
	{
		switch (weight.GetRandom())
		{
		case 0:
			callid = calls[GameLogic.Random(0, 2)].CallID;
			if (GetCanCall(callid))
			{
				AddAction(GetActionCall(callid));
			}
			break;
		case 1:
			AddAction(new AIMove1022(m_Entity, 15f));
			AddAction(GetActionAttackWait(5032, 200, 200));
			break;
		case 2:
			AddAction(new AIMove1022(m_Entity, 12f));
			AddAction(GetActionAttackWait(5033, 200, 200));
			break;
		case 3:
			JumpAction();
			JumpAction();
			JumpAction();
			AddAction(GetActionWait(string.Empty, 1000));
			break;
		}
		RecoverHPAction();
		bReRandom = true;
	}

	private void AddFirst()
	{
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.m_AniCtrl.SetString("Skill", "Begin");
			m_Entity.m_AniCtrl.SendEvent("Skill");
		}));
		AddAction(GetActionWait(string.Empty, 1800));
	}

	private void RecoverHPAction()
	{
		int num = 0;
		int num2 = hplimit.Length;
		while (true)
		{
			if (num < num2)
			{
				if (!hpused[num] && m_Entity.m_EntityData.GetHPPercent() <= hplimit[num])
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		hpused[num] = true;
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.m_AniCtrl.SetString("Continuous", "Begin");
			m_Entity.mAniCtrlBase.SetAnimationRevert("Continuous", revert: true);
			m_Entity.m_AniCtrl.SendEvent("Continuous");
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Continuous", 1f);
		}));
		AddAction(GetActionWait(string.Empty, 850));
		AddAction(GetActionDelegate(delegate
		{
			GameLogic.SendBuff(m_Entity, m_Entity, 1042);
		}));
		AddAction(GetActionWait(string.Empty, 2000));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.SetAnimationRevert("Continuous", revert: false);
			m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
			m_Entity.m_AniCtrl.SendEvent("Continuous");
		}));
		AddAction(GetActionWait(string.Empty, 850));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Continuous", -1f);
			m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
		}));
	}

	private void JumpAction()
	{
		AddAction(GetActionWait(string.Empty, 200));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.m_AniCtrl.SetString("Skill", "Jump");
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 1f);
			m_Entity.m_AniCtrl.SendEvent("Skill");
		}));
		AddAction(GetActionWait(string.Empty, 400));
		AddAction(GetActionDelegate(delegate
		{
			int num = 8;
			Vector3 position = m_Entity.position;
			for (int i = 0; i < 4; i++)
			{
				float num2 = (float)i * 90f;
				for (int j = 0; j < 3; j++)
				{
					float num3 = num2 + GameLogic.Random(0f, 90f);
					float x = MathDxx.Sin(num3);
					float z = MathDxx.Cos(num3);
					GameLogic.Release.Bullet.CreateBullet(m_Entity, 5034, position + new Vector3(x, 0.5f, z), num3);
				}
			}
		}));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -1f);
		}));
	}

	protected override void OnAIDeInit()
	{
	}
}
