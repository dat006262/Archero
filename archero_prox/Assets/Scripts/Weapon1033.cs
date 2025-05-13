




using UnityEngine;

public class Weapon1033 : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 30f - 30f);
		}
	}
}
