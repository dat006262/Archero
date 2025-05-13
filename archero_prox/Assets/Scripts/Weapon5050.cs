




using UnityEngine;

public class Weapon5050 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 30f - 30f);
		}
	}
}
