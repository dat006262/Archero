




using Dxx.Util;
using UnityEngine;

public class Weapon1078 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		if (m_Entity.IsElite)
		{
			for (int i = 0; i < 3; i++)
			{
				action.AddActionDelegate(delegate
				{
					int num2 = 3;
					for (int k = 0; k < num2; k++)
					{
						CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(k, num2, 120f));
					}
				});
				action.AddActionWait(0.1f);
			}
		}
		else
		{
			int num = 3;
			for (int j = 0; j < num; j++)
			{
				CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(j, num, 120f));
			}
		}
	}
}
