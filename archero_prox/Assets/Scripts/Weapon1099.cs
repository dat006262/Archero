




using Dxx.Util;
using UnityEngine;

public class Weapon1099 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 4;
		for (int i = 0; i < num; i++)
		{
			float bulletAngle = Utils.GetBulletAngle(i, num, 120f);
			float x = MathDxx.Sin(bulletAngle);
			float z = MathDxx.Cos(bulletAngle);
			CreateBulletOverride(new Vector3(x, 0f, z) * 2f, bulletAngle);
		}
	}
}
