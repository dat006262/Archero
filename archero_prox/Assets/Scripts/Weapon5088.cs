




using Dxx.Util;
using UnityEngine;

public class Weapon5088 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		float num = GameLogic.Random(-20f, 20f);
		int num2 = 5;
		for (int i = 0; i < num2; i++)
		{
			CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, num2, 120f) + num);
		}
	}
}
