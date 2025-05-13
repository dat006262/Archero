




using UnityEngine;

public class Weapon5101 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 2; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 60f - 30f);
		}
	}
}
