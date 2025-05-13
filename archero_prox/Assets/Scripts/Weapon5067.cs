




using UnityEngine;

public class Weapon5067 : WeaponBase
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
				for (int j = 0; j < 2; j++)
				{
					CreateBulletOverride(new Vector3(1f, 0f, 0f) * (0.5f - (float)j * 1f), 0f);
				}
			});
			action.AddActionWait(0.12f);
		}
	}
}
