




using UnityEngine;

public class Weapon5069 : WeaponBase
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
					CreateBulletOverride(Vector3.zero, (float)(j * 20) - 10f);
				}
			});
			action.AddActionWait(0.2f);
		}
	}
}
