




using System;
using UnityEngine;

public class SkillAlone1074 : SkillAloneBase
{
	private int attack = 20;

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
		float num = GameLogic.Random(160, 180);
		CreateBullet(num);
		CreateBullet(0f - num);
	}

	private void CreateBullet(float offsetangle)
	{
		if (!(m_Entity.m_HatredTarget == null))
		{
			Vector3 position = m_Entity.m_HatredTarget.position;
			Bullet3006 bullet = GameLogic.Release.Bullet.CreateBullet(m_Entity, 3006) as Bullet3006;
			bullet.transform.position = m_Entity.m_Body.LeftBullet.transform.position;
			Transform transform = bullet.transform;
			Vector3 eulerAngles = m_Entity.eulerAngles;
			transform.rotation = Quaternion.Euler(0f, eulerAngles.y + offsetangle, 0f);
			BulletTransmit bulletAttribute = new BulletTransmit(m_Entity, 3006, clear: true);
			bullet.SetBulletAttribute(bulletAttribute);
			bullet.SetEndPos(position, offsetangle);
		}
	}
}
