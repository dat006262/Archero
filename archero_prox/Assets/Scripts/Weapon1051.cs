




using UnityEngine;

public class Weapon1051 : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		int num = 6;
		for (int i = 0; i < num; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 60f);
		}
	}
}
