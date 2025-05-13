




using Dxx.Util;
using System;

public class SkillAlone1072 : SkillAloneBase
{
	private float hitratio;

	protected override void OnInstall()
	{
		hitratio = float.Parse(base.m_SkillData.Args[0]);
		m_Entity.m_EntityData.Modify_Light45(1);
		EntityBase entity = m_Entity;
		entity.OnLight45 = (Action<EntityBase>)Delegate.Combine(entity.OnLight45, new Action<EntityBase>(OnLight45));
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_Light45(-1);
		EntityBase entity = m_Entity;
		entity.OnLight45 = (Action<EntityBase>)Delegate.Remove(entity.OnLight45, new Action<EntityBase>(OnLight45));
	}

	private void OnLight45(EntityBase target)
	{
		if (target != null)
		{
			BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 3013, target.m_Body.EffectMask.transform.position, 0f);
			bulletBase.AddCantHit(target);
			bulletBase.mBulletTransmit.SetAttack(MathDxx.CeilToInt(hitratio * (float)m_Entity.m_EntityData.GetAttackBase()));
		}
	}
}
