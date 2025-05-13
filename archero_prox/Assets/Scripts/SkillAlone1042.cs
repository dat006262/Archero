




using Dxx.Util;
using UnityEngine;

public class SkillAlone1042 : SkillAloneBase
{
	private float time;

	private float delaytime;

	private float hitratio;

	private float range;

	private long clockindex;

	protected override void OnInstall()
	{
		delaytime = float.Parse(base.m_SkillData.Args[0]);
		hitratio = float.Parse(base.m_SkillData.Args[1]);
		range = float.Parse(base.m_SkillData.Args[2]);
		clockindex = TimeClock.Register("SkillAlone1042", delaytime, OnAttack);
	}

	protected override void OnUninstall()
	{
		TimeClock.Unregister(clockindex);
	}

	private void CreateSkillAlone()
	{
	}

	private void OnAttack()
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, range, sameteam: false);
		if (nearEntity != null)
		{
			GameObject gameObject = GameLogic.Release.MapEffect.Get("Game/SkillPrefab/SkillAlone1042_One");
			gameObject.transform.position = nearEntity.m_Body.EffectMask.transform.position;
			long beforehit = -MathDxx.CeilToInt((float)m_Entity.m_EntityData.GetAttackBase() * hitratio);
			GameLogic.SendHit_Skill(nearEntity, beforehit);
		}
	}
}
