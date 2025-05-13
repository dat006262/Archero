




using UnityEngine;

public class Weapon5090 : WeaponSprintBase
{
	private float onedis = 2.4f;

	private float alldis;

	protected override void OnInit()
	{
		distance = 9f;
		delaytime = 0.6f;
		alldis = 0f;
		base.OnInit();
	}

	protected override void OnUpdateMove(float currentdis)
	{
		alldis += currentdis;
		if (alldis >= onedis)
		{
			alldis -= onedis;
			if ((bool)m_Entity.m_HatredTarget)
			{
				BulletManager bullet = GameLogic.Release.Bullet;
				EntityBase entity = m_Entity;
				Vector3 position = m_Entity.m_Body.EffectMask.transform.position;
				Vector3 eulerAngles = m_Entity.eulerAngles;
				BulletBase bulletBase = bullet.CreateBullet(entity, 5089, position, eulerAngles.y + (float)GameLogic.Random(-15, 15) + 90f);
				bulletBase.SetPosFromTarget(10f);
				BulletManager bullet2 = GameLogic.Release.Bullet;
				EntityBase entity2 = m_Entity;
				Vector3 position2 = m_Entity.m_Body.EffectMask.transform.position;
				Vector3 eulerAngles2 = m_Entity.eulerAngles;
				BulletBase bulletBase2 = bullet2.CreateBullet(entity2, 5089, position2, eulerAngles2.y - (float)GameLogic.Random(-15, 15) - 90f);
				bulletBase2.SetPosFromTarget(10f);
			}
		}
	}
}
