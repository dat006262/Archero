




using UnityEngine;

public class Weapon5016 : Weapon1020
{
	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			BulletBase bulletBase = CreateBulletOverride(Vector3.zero, 60f - (float)i * 60f);
			bulletBase.SetTarget(Target, ParabolaSize);
			bulletBase.OnBulletCache = OnBulletCache;
		}
	}
}
