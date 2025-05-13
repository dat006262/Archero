




using Dxx.Util;
using UnityEngine;

public class Weapon5018 : WeaponBase
{
	private float mAngle;

	protected override void OnInstall()
	{
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, num, 150f));
		}
	}
}
