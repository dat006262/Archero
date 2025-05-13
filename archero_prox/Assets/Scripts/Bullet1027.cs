




using UnityEngine;

public class Bullet1027 : Bullet1024
{
	private float rotateangle = 0.5f;

	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnUpdate()
	{
		bulletAngle += rotateangle;
		base.transform.rotation = Quaternion.Euler(0f, bulletAngle, 0f);
		CheckBulletLength();
	}
}
