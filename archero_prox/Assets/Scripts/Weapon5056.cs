




using UnityEngine;

public class Weapon5056 : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			CreateBulletOverride(Vector3.zero, i * 30 - 30);
		}
	}
}
