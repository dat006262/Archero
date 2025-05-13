




using UnityEngine;

public class Bullet5039 : BulletBase
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

	protected override void OnUpdate()
	{
		Transform mTransform = base.mTransform;
		Vector3 position = base.mTransform.position;
		float x = position.x;
		Vector3 position2 = base.mTransform.position;
		float y = position2.y - base.FrameDistance;
		Vector3 position3 = base.mTransform.position;
		mTransform.position = new Vector3(x, y, position3.z);
		Vector3 position4 = base.mTransform.position;
		if (position4.y <= 0f)
		{
			overDistance();
		}
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
