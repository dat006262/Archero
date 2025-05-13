




using UnityEngine;

public class Weapon5027 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			action.AddActionDelegate(delegate
			{
				GameLogic.Hold.Sound.PlayBulletCreate(2003004, m_Entity.position);
				int num = 7;
				float num2 = 180f / (float)(num - 1);
				for (int j = 0; j < num; j++)
				{
					CreateBulletOverride(Vector3.zero, (float)j * num2 - 90f);
				}
			});
			action.AddActionWait(0.2f);
		}
	}
}
