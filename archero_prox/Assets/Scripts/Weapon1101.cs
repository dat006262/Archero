




using Dxx.Util;
using UnityEngine;

public class Weapon1101 : WeaponBase
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
		int num2 = 3;
		for (int i = 0; i < num2; i++)
		{
			CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, num2, 90f) + num);
		}
	}
}
