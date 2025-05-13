




using Dxx.Util;
using UnityEngine;

public class SkillAlone1076 : SkillAloneBase
{
	private long clockindex;

	private int bulletid;

	private float delaytime;

	private float hitratio;

	protected override void OnInstall()
	{
		UnityEngine.Debug.Log("1076 create");
		bulletid = int.Parse(base.m_SkillData.Args[0]);
		delaytime = float.Parse(base.m_SkillData.Args[1]);
		hitratio = float.Parse(base.m_SkillData.Args[2]);
		clockindex = TimeClock.Register("SkillAlone1076", delaytime, OnAttack);
	}

	protected override void OnUninstall()
	{
		TimeClock.Unregister(clockindex);
	}

	private void OnAttack()
	{
		EntityBase entityBase = GameLogic.Release.Entity.FindCanAttackRandom(m_Entity);
		if ((bool)entityBase)
		{
			float angle = Utils.getAngle(entityBase.position - m_Entity.position);
			angle += GameLogic.Random(-10f, 10f);
			float d = Vector3.Distance(entityBase.position, m_Entity.position) - 1f;
			Vector3 pos = entityBase.position + new Vector3(MathDxx.Sin(angle + 180f), 0f, MathDxx.Cos(angle + 180f)) * d;
			pos.y = 1f;
			BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, bulletid, pos, angle);
			bulletBase.mBulletTransmit.SetAttack(MathDxx.CeilToInt(hitratio * (float)m_Entity.m_EntityData.GetAttackBase()));
		}
	}
}
