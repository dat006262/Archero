




using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1078 : SkillAloneBase
{
	private long clockindex;

	private int bulletid;

	private int createweight;

	private float hitratio;

	protected override void OnInstall()
	{
		bulletid = int.Parse(base.m_SkillData.Args[0]);
		createweight = int.Parse(base.m_SkillData.Args[1]);
		hitratio = float.Parse(base.m_SkillData.Args[2]);
		EntityBase entity = m_Entity;
		entity.Event_OnAttack = (Action)Delegate.Combine(entity.Event_OnAttack, new Action(OnAttack));
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.Event_OnAttack = (Action)Delegate.Remove(entity.Event_OnAttack, new Action(OnAttack));
	}

	private void OnAttack()
	{
		if (GameLogic.Random(0f, 100f) <= (float)createweight)
		{
			GameLogic.Release.MapCreatorCtrl.RandomItem(m_Entity.position, 100, out float endx, out float endz);
			Vector3 vector = new Vector3(endx, 0f, endz);
			BulletSlopeBase bulletSlopeBase = GameLogic.Release.Bullet.CreateSlopeBullet(m_Entity, bulletid, vector + new Vector3(0f, 21f, 0f), vector);
			bulletSlopeBase.mBulletTransmit.SetAttack(MathDxx.CeilToInt(hitratio * (float)m_Entity.m_EntityData.GetAttackBase()));
		}
	}
}
