




using UnityEngine;

public class Weapon1006 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 4; i++)
		{
			int index = i;
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					CreateBulletOverride(Vector3.zero, -90 * index);
				}
			});
			action.AddAction(new ActionBasic.ActionWait
			{
				waitTime = 0.04f
			});
		}
	}
}
