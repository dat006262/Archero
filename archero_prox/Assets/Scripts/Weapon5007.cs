




using UnityEngine;

public class Weapon5007 : WeaponBase
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
			int cc = i;
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					GameLogic.Hold.Sound.PlayBulletCreate(2003004, m_Entity.position);
					if (cc % 2 == 0)
					{
						for (int j = 0; j < 5; j++)
						{
							CreateBulletOverride(new Vector3((float)j * 0.2f - 0.4f, 0f, 0f), j * 20 - 40);
						}
					}
					else
					{
						for (int k = 0; k < 5; k++)
						{
							CreateBulletOverride(new Vector3((float)k * 0.4f - 0.8f, 0f, 0f), k * 20 - 40);
						}
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
