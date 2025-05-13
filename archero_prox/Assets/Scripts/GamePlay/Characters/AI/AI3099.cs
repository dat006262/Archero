




using DG.Tweening;
using UnityEngine;

public class AI3099 : AIBase
{
	protected Sequence seq;

	private int bulletid;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		GameObject child = GameLogic.EffectGet("Effect/Monster/3097_red");
		child.SetParentNormal(m_Entity.m_Body.Body);
		Entity3097BaseCtrl entity3097BaseCtrl = CInstance<BattleResourceCreator>.Instance.Get3097Base(m_Entity.m_Body.EffectMask.transform.parent);
		entity3097BaseCtrl.SetTexture("3097_red");
		bulletid = m_Entity.m_Data.WeaponID;
		if (m_Entity.IsElite)
		{
			bulletid = 1094;
		}
	}

	protected override void OnInit()
	{
		AddAction(GetActionWait("actionwaitr1", 2000));
		AddAction(GetActionDelegate(delegate
		{
			float num = GameLogic.Random(0f, 360f);
			for (int i = 0; i < 9; i++)
			{
				int num2 = i;
				GameLogic.Release.Bullet.CreateBullet(m_Entity, bulletid, m_Entity.m_Body.LeftBullet.transform.position, (float)(num2 * 360) / 9f + num);
			}
		}));
		AddAction(GetActionWait("actionwaitr1", 1000));
	}

	protected override void OnAIDeInit()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}
}
