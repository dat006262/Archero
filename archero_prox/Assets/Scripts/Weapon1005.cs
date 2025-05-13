




using UnityEngine;

public class Weapon1005 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 4; i++)
		{
			CreateBulletOverride(Vector3.zero, 90f * (float)i);
		}
	}
}
