




using Dxx.Util;
using UnityEngine;

public class Weapon5079 : WeaponBase
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
					float num = -45 * index;
					float x = MathDxx.Sin(num);
					float z = MathDxx.Cos(num);
					CreateBulletOverride(new Vector3(x, 0f, z), num);
				}
			});
			action.AddAction(new ActionBasic.ActionWait
			{
				waitTime = 0.037f
			});
		}
	}
}
