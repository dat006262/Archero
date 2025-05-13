




using Dxx.Util;
using UnityEngine;

public class Weapon5081 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		AddAttack4();
	}

	private void AddAttack4()
	{
		int count = 4;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(index, count, 120f));
				}
			});
		}
	}
}
