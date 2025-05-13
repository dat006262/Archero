




using UnityEngine;

public class Weapon1036 : WeaponBase
{
	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			CreateBulletOverride(Vector3.zero, 45f - (float)i * 45f);
		}
	}
}
