




using UnityEngine;

public class Weapon1028 : WeaponBase
{
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
		int num = 12;
		float num2 = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			CreateBulletOverride(Vector3.zero, num2 * (float)i);
		}
	}
}
