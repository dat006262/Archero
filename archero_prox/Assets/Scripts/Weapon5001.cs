




using UnityEngine;

public class Weapon5001 : WeaponBase
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
		for (int i = 0; i < 3; i++)
		{
			CreateBulletOverride(Vector3.zero, i * 30 - 30);
		}
	}
}
