




using UnityEngine;

public class Weapon5026 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					GameLogic.Hold.Sound.PlayBulletCreate(2003004, m_Entity.position);
					for (int j = 0; j < 8; j++)
					{
						CreateBulletOverride(Vector3.zero, (float)j * 45f + GameLogic.Random(-30f, 30f));
					}
				}
			});
			action.AddAction(new ActionBasic.ActionWait
			{
				waitTime = 0.15f
			});
		}
	}
}
