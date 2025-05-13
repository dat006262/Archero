




using UnityEngine;

public class Weapon5028 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 2; i++)
		{
			int index = i;
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, (index != 0) ? getRandomAngle() : 0f);
			});
			action.AddActionWait(0.2f);
		}
	}

	private float getRandomAngle()
	{
		return GameLogic.Random(3f, 10f) * (float)((GameLogic.Random(0, 2) == 0) ? 1 : (-1));
	}
}
