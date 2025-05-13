




using UnityEngine;

public class Weapon5035 : WeaponBase
{
	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 8; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 45f);
		}
	}
}
