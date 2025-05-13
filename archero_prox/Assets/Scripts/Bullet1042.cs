




using UnityEngine;

public class Bullet1042 : Bullet1031
{
	protected override void OnInit()
	{
		base.OnInit();
		OnBulletCache = OnBulletCaches;
	}

	private void OnBulletCaches()
	{
		Vector3 pos = mTransform.position + moveDirection * centerz;
		GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 3007, pos, bulletAngle + 90f);
		GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 3007, pos, bulletAngle - 90f);
	}
}
