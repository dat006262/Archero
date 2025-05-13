




using UnityEngine;

public class Weapon5052 : WeaponBase
{
	private float angle = 15f;

	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 7;
		for (int i = 0; i < num; i++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, 0f);
			});
			if (i < num - 1)
			{
				action.AddActionWait(0.2f);
			}
		}
	}
}
