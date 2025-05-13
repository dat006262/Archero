




using UnityEngine;

public class AI3102 : AIBase
{
	private int bulletid;

	private float prev_scale;

	private int count = 8;

	private float delay = 0.15f;

	private ThunderContinueMgr.ThunderContinueReceive receive;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		GameObject child = GameLogic.EffectGet("Effect/Monster/3097_yellow");
		child.SetParentNormal(m_Entity.m_Body.Body);
		Entity3097BaseCtrl entity3097BaseCtrl = CInstance<BattleResourceCreator>.Instance.Get3097Base(m_Entity.m_Body.EffectMask.transform.parent);
		entity3097BaseCtrl.SetTexture("3097_yellow");
		bulletid = m_Entity.m_Data.WeaponID;
		prev_scale = 1f;
		if (m_Entity.IsElite)
		{
			bulletid = 1095;
			prev_scale = 1.5f;
		}
	}

	protected override void OnInit()
	{
		int num = GameLogic.Random(500, 1500);
		AddAction(GetActionWait("actionwaitr1", num));
		AddAction(GetActionDelegate(delegate
		{
			receive = ThunderContinueMgr.GetThunderContinue(new ThunderContinueMgr.ThunderContinueData
			{
				entity = m_Entity,
				bulletid = bulletid,
				count = count,
				delay = delay,
				prev_scale = prev_scale
			});
		}));
		AddAction(GetActionWait("actionwaitr1", 4000 - num));
	}

	protected override void OnAIDeInit()
	{
		if (receive != null)
		{
			receive.Deinit();
		}
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
