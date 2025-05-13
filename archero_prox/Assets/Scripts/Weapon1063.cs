




using Dxx.Util;
using UnityEngine;

public class Weapon1063 : WeaponBase
{
	protected override void OnInstall()
	{
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		if (m_Entity.IsElite)
		{
			if (MathDxx.RandomBool())
			{
				int num = 4;
				for (int i = 0; i < num; i++)
				{
					action.AddActionDelegate(delegate
					{
						CreateBulletOverride(Vector3.zero, 0f);
					});
					action.AddActionWait(0.18f);
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					CreateBulletOverride(Vector3.zero, (float)j * 90f + 45f);
				}
			}
		}
		else
		{
			base.OnAttack(args);
		}
	}
}
