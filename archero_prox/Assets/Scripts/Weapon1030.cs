




using UnityEngine;

public class Weapon1030 : WeaponBase
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
		int num = 36;
		float per = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			int index = i;
			action.AddActionWaitDelegate(0.01f, delegate
			{
				for (int j = 0; j < 4; j++)
				{
					CreateBulletOverride(Vector3.zero, (float)j * 90f + per * (float)index);
				}
			});
		}
	}
}
