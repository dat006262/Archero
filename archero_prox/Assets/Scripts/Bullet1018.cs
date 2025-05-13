




using UnityEngine;

public class Bullet1018 : BulletBase
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

	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		PosFromStart2Target += Random.Range(-1.5f, 1.5f);
	}

	private void CreateGround()
	{
		BulletManager bullet = GameLogic.Release.Bullet;
		EntityBase entity = base.m_Entity;
		Vector3 position = mTransform.position;
		float x = position.x;
		Vector3 position2 = mTransform.position;
		bullet.CreateBullet(entity, 1019, new Vector3(x, 0f, position2.z), 0f);
	}
}
