




using UnityEngine;

public class Weapon1103 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 5; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 30f - 60f);
		}
	}
}
