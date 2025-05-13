




using Dxx.Util;
using UnityEngine;

public class Weapon1016 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		if (m_Entity.IsElite && MathDxx.RandomBool())
		{
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, num, 90f));
			}
			return;
		}
		for (int j = 0; j < 3; j++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, 0f);
			});
			if (j < 2)
			{
				action.AddActionWait(0.1f);
			}
		}
	}
}
