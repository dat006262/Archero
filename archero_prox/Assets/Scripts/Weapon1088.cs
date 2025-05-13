




using UnityEngine;

public class Weapon1088 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		float num = GameLogic.Random(0f, 360f);
		for (int i = 0; i < 8; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)(i * 45) + num);
		}
	}
}
