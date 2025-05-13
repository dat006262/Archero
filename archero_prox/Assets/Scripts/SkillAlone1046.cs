




using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1046 : SkillAloneBase
{
	private float angle;

	private Vector3 offsetpos;

	protected override void OnInstall()
	{
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
		Vector3 eulerAngles = m_Entity.eulerAngles;
		angle = eulerAngles.y + 90f;
		offsetpos = new Vector3(MathDxx.Sin(angle), 0f, MathDxx.Cos(angle));
		CreateBullet(-1f);
		CreateBullet(1f);
	}

	private void CreateBullet(float dis)
	{
		Bullet3014 bullet = GameLogic.Release.Bullet.CreateBullet(m_Entity, 3014) as Bullet3014;
		bullet.transform.position = m_Entity.position + offsetpos * dis;
		Transform transform = bullet.transform;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
		BulletTransmit bulletAttribute = new BulletTransmit(m_Entity, 3014, clear: true);
		bullet.SetBulletAttribute(bulletAttribute);
	}
}
