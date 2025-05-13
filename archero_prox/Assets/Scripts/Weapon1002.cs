




using UnityEngine;

public class Weapon1002 : WeaponBase
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
				CreateBulletOverride(Vector3.zero, (float)i * 45f - 45f);
			}
		}
		else
		{
			base.OnAttack(args);
		}
	}
}
