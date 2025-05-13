




using Dxx.Util;
using UnityEngine;

public class SkillAlone1071 : SkillAloneBase
{
	private long clockindex;

	private float delaytime;

	private float hitratio;

	protected override void OnInstall()
	{
		delaytime = float.Parse(base.m_SkillData.Args[0]);
		hitratio = float.Parse(base.m_SkillData.Args[1]);
		clockindex = TimeClock.Register("SkillAlone1071", delaytime, OnAttack);
	}

	protected override void OnUninstall()
	{
		TimeClock.Unregister(clockindex);
	}

	private void OnAttack()
	{
		Vector3 position = m_Entity.position;
		float angle = GameLogic.Random(0f, 360f);
		Vector3 b = new Vector3(MathDxx.Sin(angle), 1f, MathDxx.Cos(angle)) * 2f;
		float rota = GameLogic.Random(0f, 360f);
		BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 3012, position + b, rota);
		bulletBase.mBulletTransmit.SetAttack(MathDxx.CeilToInt(hitratio * (float)m_Entity.m_EntityData.GetAttackBase()));
	}
}
