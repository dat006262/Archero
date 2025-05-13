




using Dxx.Util;
using UnityEngine;

public class Weapon5080 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		AddAttack3();
	}

	private void AddAttack3()
	{
		int count = 3;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			action.AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(index, count, 90f));
				}
			});
		}
	}
}
