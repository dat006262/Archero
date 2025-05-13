




using Dxx.Util;
using UnityEngine;

public class Weapon1065 : WeaponBase
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
			int num = 3;
			for (int i = 0; i < num; i++)
			{
				CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, num, 90f));
			}
		}
		else
		{
			base.OnAttack(args);
		}
	}
}
