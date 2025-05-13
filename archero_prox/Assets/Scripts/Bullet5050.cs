




using UnityEngine;

public class Bullet5050 : BulletBase
{
	protected override void OnOverDistance()
	{
		for (int i = 0; i < 4; i++)
		{
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			Vector3 position = mTransform.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			bullet.CreateBullet(entity, 5051, new Vector3(x, 1f, position2.z), (float)i * 90f + 45f);
		}
	}
}
