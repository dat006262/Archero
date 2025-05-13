




using UnityEngine;

public class Weapon5040 : WeaponBase
{
	private float allangle = 90f;

	private int count = 3;

	private float perangle;

	protected override void OnInstall()
	{
		perangle = allangle / (float)(count - 1);
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 4; i++)
		{
			int index2 = i;
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, perangle * (float)index2 - allangle / 2f);
			});
			action.AddActionWait(0.1f);
		}
		for (int j = 0; j < 4; j++)
		{
			int index = j;
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, (0f - perangle) * (float)index + allangle / 2f);
			});
			action.AddActionWait(0.1f);
		}
	}
}
