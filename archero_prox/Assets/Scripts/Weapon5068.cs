




using UnityEngine;

public class Weapon5068 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 2; i++)
		{
			action.AddActionDelegate(delegate
			{
				GameLogic.Hold.Sound.PlayBulletCreate(2005001, m_Entity.position);
				for (int j = 0; j < 5; j++)
				{
					CreateBulletOverride(Vector3.zero, j * 30 - 60);
				}
			});
			action.AddActionWait(0.3f);
		}
	}
}
