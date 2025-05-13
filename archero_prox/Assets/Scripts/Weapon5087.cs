




using UnityEngine;

public class Weapon5087 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		float num = 1f;
		float num2 = 90f;
		GameLogic.Hold.Sound.PlayBulletCreate(2003004, m_Entity.position);
		for (int i = 0; i < 20; i++)
		{
			CreateBulletOverride(new Vector3(GameLogic.Random(0f - num, num), 0f, GameLogic.Random(0f - num, num)), GameLogic.Random(0f - num2, num2));
		}
	}
}
