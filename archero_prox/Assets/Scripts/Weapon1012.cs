




using UnityEngine;

public class Weapon1012 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 8; i++)
		{
			int index = i;
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					CreateBulletOverride(Vector3.zero, -45 * index);
				}
			});
			action.AddAction(new ActionBasic.ActionWait
			{
				waitTime = 0.037f
			});
		}
	}
}
