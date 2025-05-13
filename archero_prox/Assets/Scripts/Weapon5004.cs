




using UnityEngine;

public class Weapon5004 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 5; i++)
		{
			action.AddActionDelegate(delegate
			{
				GameLogic.Hold.Sound.PlayBulletCreate(2005001, m_Entity.position);
				for (int j = 0; j < 1; j++)
				{
					CreateBulletOverride(Vector3.zero, 0f);
				}
			});
			action.AddActionWait(0.2f);
		}
	}
}
