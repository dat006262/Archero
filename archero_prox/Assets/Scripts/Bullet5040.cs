




using UnityEngine;

public class Bullet5040 : BulletBase
{
	protected override void AwakeInit()
	{
		base.AwakeInit();
	}

	protected override void OnDeInit()
	{
		CreateGround();
		base.OnDeInit();
	}

	private void CreateGround()
	{
		BulletManager bullet = GameLogic.Release.Bullet;
		EntityBase entity = base.m_Entity;
		Vector3 position = mTransform.position;
		float x = position.x;
		Vector3 position2 = mTransform.position;
		bullet.CreateBullet(entity, 5065, new Vector3(x, 0f, position2.z), 0f);
	}
}
