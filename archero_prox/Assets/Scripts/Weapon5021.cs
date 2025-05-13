




using UnityEngine;

public class Weapon5021 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		GameLogic.Hold.Sound.PlayBulletCreate(2005001, m_Entity.position);
		int num = 4;
		float num2 = 60f;
		float num3 = num2 / (float)(num - 1);
		float num4 = num3 * (float)(num - 1) / 2f;
		for (int i = 0; i < 5; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * num3 - num4);
		}
	}
}
