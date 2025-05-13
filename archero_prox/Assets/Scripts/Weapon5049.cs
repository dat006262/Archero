




using UnityEngine;

public class Weapon5049 : WeaponBase
{
	private int count = 2;

	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < count; i++)
		{
			int index = i;
			action.AddActionDelegate(delegate
			{
				for (int j = 0; j < 4; j++)
				{
					BulletBase bulletBase = CreateBulletOverride(Vector3.zero, (float)(j * 30) - 45f);
					bulletBase.SetArgs(100011 + ((index % 2 != 0) ? 1 : 0));
				}
			});
			if (i < count - 1)
			{
				action.AddActionWait(0.15f);
			}
		}
	}
}
