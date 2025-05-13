




using UnityEngine;

public class Weapon5009 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 7;
		float num2 = 180f / (float)num;
		float num3 = (float)(num / 2) * num2;
		for (int i = 0; i < num; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * num2 - num3);
		}
	}
}
